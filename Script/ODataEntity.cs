﻿/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Net;


#if NET
using Bendyline.Base;
using System.Text;

namespace Bendyline.Data
#elif SCRIPTSHARP
using System.Serialization;

namespace BL.Data
#endif
{
    public class ODataEntity : Item
    {
        private string id;
        private HttpRequest saveRequest;
        private String activeSaveJson = null;
        private bool additionalSaveNeeded = false;
        public virtual event EventHandler Saving;
        public virtual event EventHandler Saved;

        private Operation saveOperation;

        public bool IsSaving
        {
            get
            {
                return this.saveOperation != null;
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
            if (this.Disconnected)
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

            ((ODataEntityType)this.Type).UpdateItemInItemSets(this);

            this.SendSaveRequest();
        }

        private void SendSaveRequest()
        {
            String endpoint = ((ODataEntityType)this.Type).Url;

            HttpRequest xhr = new HttpRequest();
            
            if (this.LocalStatus == ItemLocalStatus.Update || this.LocalStatus == ItemLocalStatus.Deleted)
            {
                xhr.Verb = "PUT";
                xhr.Url = endpoint + "(" + this.Id + "L)";
            }
            else
            {
                xhr.Verb = "POST";
                xhr.Url = endpoint;
            }

            xhr.RequestType = HttpRequestType.ODataV2JsonWrite;

    //            xhr.SetRequestHeader("DataServiceVersion", "DataServiceVersion: 3.0;NetFx");
            xhr.OnReadyStateChange = new Action(this.HandleSaveComplete);

            this.saveRequest = xhr;
            xhr.Body = this.activeSaveJson;

            xhr.Send();
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
#if SCRIPTSHARP
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
                                this.localSetValue(fiName, val);
                            }}
                        }}

                if ({2}==0 && oc[""Id""] !=null)
                {{
                    this.setId(oc[""Id""]);
                }}    
", results, this.Type.Fields, previousStatus);
#else
                        throw new NotImplementedException();
#endif

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