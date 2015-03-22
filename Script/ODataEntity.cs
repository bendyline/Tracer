/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

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
        private String activeSaveJson = null;
        private bool additionalSaveNeeded = false;

        public event EventHandler Saving;
        public event EventHandler Saved;
        private Operation saveOperation;

        public bool IsSaving
        {
            get
            {
                return this.saveOperation != null;
            }
        }

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

        public bool IsValid
        {
            get
            {
                Query validCriteria = ((ODataEntityType)this.Type).ValidCriteria;

                if (validCriteria == null)
                {
                    return true;
                }

                return validCriteria.ItemMatches(this);
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
                if (f.Name != "Id" && f.Name != "CreatedDate" && f.Name != "ModifiedDate")
                {
                    String value = this.GetStringValue(f.Name);

                    if (value != null)
                    {
                        if (f.Type == FieldType.BoolChoice)
                        {
                            if (value == "true")
                            {
                                result.Append(",\"" + f.Name + "\":true");
                            }
                            else
                            {
                                result.Append(",\"" + f.Name + "\":false");
                            }
                        }
                        else if (f.Type == FieldType.Integer || f.Type == FieldType.Geopoint || f.Type == FieldType.BigNumber)
                        {
                            result.Append(",\"" + f.Name + "\":" + value);
                        }
                        else if (f.Type == FieldType.DateTime)
                        {
                            result.Append(",\"" + f.Name + "\":\"" + value + "\"");
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

            if (this.LocalStatus == ItemLocalStatus.Unchanged)
            {
                CallbackResult.NotifySynchronousSuccess(callback, state, this);

                if (this.Saved != null)
                {
                    this.Saved(this, EventArgs.Empty);
                }

                return;
            }

            if (this.saveOperation == null)
            {
                this.saveOperation = new Operation();
                isNew = true;
            }

            this.saveOperation.AddCallback(callback, state);

            String json = GetJson();

            if (!isNew)
            {
                if (json != this.activeSaveJson)
                {
                    this.additionalSaveNeeded = true;
                }

                return;
            }

            if (this.Saving != null)
            {
                this.Saving(this, EventArgs.Empty);
            }

            this.activeSaveJson = json;
            this.SendSaveRequest();
        }

        private void SendSaveRequest()
        {
            String endpoint = ((ODataEntityType)this.Type).Url;

            XmlHttpRequest xhr = new XmlHttpRequest();

            if (this.LocalStatus == ItemLocalStatus.Update || this.LocalStatus == ItemLocalStatus.Deleted)
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
            xhr.OnReadyStateChange = new Action(this.HandleSaveComplete);

            this.saveRequest = xhr;

            xhr.Send(this.activeSaveJson);
        }

        private void HandleSaveComplete()
        {
            if (this.saveRequest != null && this.saveRequest.ReadyState == ReadyState.Loaded)
            {
                ItemLocalStatus previousStatus = this.LocalStatus;

                if (this.saveRequest.Status < 400)
                {
                    String responseContent = this.saveRequest.ResponseText;

                    if (!String.IsNullOrEmpty(responseContent))
                    {
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

                if ({2}==0 && oc[""Id""] !=null)
                {{
                    this.setId(oc[""Id""]);
                }}    
", results, this.Type.Fields, previousStatus);
                    }
                }

                this.saveRequest = null;

                if (this.additionalSaveNeeded)
                {
                    this.additionalSaveNeeded = false;
                    this.activeSaveJson = this.GetJson();
                    this.SendSaveRequest();
                }
                else
                {
                    this.SetLocalStatus(ItemLocalStatus.Unchanged);

                    if (this.Saved != null)
                    {
                        this.Saved(this, EventArgs.Empty);
                    }

                    Operation o = this.saveOperation;

                    this.saveOperation = null;

                    o.CompleteAsAsyncDone(this);

                }
            }
        }
    }
}