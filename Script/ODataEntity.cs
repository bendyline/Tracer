/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using Bendyline.Base;
using System;
using System.Collections.Generic;
using System.Net;
using System.Serialization;

namespace BL.Data
{
    public class ODataEntity : Item
    {
        private string id;
        private bool disconnected = false;
        private XmlHttpRequest saveRequest;

        public event EventHandler Saved;
        private Operation saveOperation;

        public bool Disconnected
        {
            get
            {
                return this.disconnected;
            }

            set
            {
                this.disconnected = value;
            }
        }

        public Operation SaveOperation
        {
            get
            {
                if (this.saveOperation == null)
                {
                    this.saveOperation = new Operation();
                }

                return this.saveOperation;
            }

            set
            {
                this.saveOperation = value;
            }
        }

        public override String Id
        {
            get
            {
                if (this.id == null)
                {
                    this.id = this.GetValueAsString("Id");
                }

                return this.id;
            }
        }

        public ODataStore Store
        {
            get
            {
                return (ODataStore)this.Type.Store;
            }
        }

        public ODataEntity(ODataEntityType entityType) : base(entityType)
        {
            
        }

        public void SetId(String id)
        {
            this.id = id;
        }

        private String GetJson()
        {
            StringBuilder result = new StringBuilder();

            result.Append("{");
            result.Append(" \"odata.type\":\"" + this.Store.Namespace + "." + this.Type.Name + "\"");
         
            foreach (Field f in this.Type.Fields)
            {
                if (f.Name != "Id")
                {
                    String value = this.GetStringValue(f.Name);

                    if (value != null)
                    {
                        if (f.Type == FieldType.Integer)
                        {
                            result.Append(",\"" + f.Name + "\":" + value);
                        }
                        else
                        {
                            result.Append(",\"" + f.Name + "\":\"" + JsonUtilities.Encode(value) + "\"");
                        }
                    }
                }
            }

            result.Append("}");

            return result.ToString();
        }

        public void Save(AsyncCallback callback, object state)
        {
            if (this.disconnected)
            {
                throw new Exception("Cannot save a disconnected item.");
            }

            bool isNew = false;

            if (this.Status == ItemStatus.Unchanged)
            {
                if (callback != null)
                {
                    CallbackResult cr = new CallbackResult();
                    cr.AsyncState = state;
                    cr.Data = this;
                    cr.CompletedSynchronously = true;

                    callback(cr);
                }

                return;
            }

            if (this.saveOperation == null)
            {
                this.saveOperation = new Operation();
                isNew = true;
            }

            this.saveOperation.AddCallback(callback, state);

            if (!isNew)
            {
                return;
            }

            String endpoint = ((ODataEntityType)this.Type).Url;

            String json = GetJson();

            XmlHttpRequest xhr = new XmlHttpRequest();

            if (this.Status == ItemStatus.Update)
            {
                xhr.Open("PUT", endpoint + "(" + this.Id + "L)");
            }
            else
            {
                xhr.Open("POST", endpoint);
            }
            xhr.SetRequestHeader("Accept", "application/json;odata=minimalmetadata");
            xhr.SetRequestHeader("Content-Type", "application/json");
//            xhr.SetRequestHeader("DataServiceVersion", "DataServiceVersion: 3.0;NetFx");
            xhr.Send(json);
            xhr.OnReadyStateChange = new Action(this.HandleSaveComplete);

            this.saveRequest = xhr;
        }

        private void HandleSaveComplete()
        {
            if (this.saveRequest != null && this.saveRequest.ReadyState == ReadyState.Loaded)
            {
                this.SetStatus(ItemStatus.Unchanged);
                
                String responseContent = this.saveRequest.ResponseText;

                if (String.IsNullOrEmpty(responseContent))
                {
                    return;
                }

                object results = Json.Parse(responseContent);

                Script.Literal(@"
                    var oc = {0};
                    var fieldarr = {1};

                        for (var j=0; j<fieldarr.length; j++)
                        {{
                            var fi = fieldarr[j];
                            var fiName = fi.get_name();

                            var val = oc[fi.get_name()];

                            if (val != null)
                            {{
                                this.setValue(fiName, val);
                            }}
                        }}
", results, this.Type.Fields);

                if (this.Saved != null)
                {
                    this.Saved(this, EventArgs.Empty);
                }
                

                this.saveOperation.CompleteAsAsyncDone(this);

                this.saveOperation = null;
            }
        }
    }
}