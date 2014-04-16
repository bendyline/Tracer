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
        private XmlHttpRequest saveRequest;

        public event EventHandler Saved;
        public override String Id
        {
            get
            {
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
                    if (f.Type == FieldType.Integer)
                    {
                        result.Append(",\"" + f.Name + "\":" + this.GetStringValue(f.Name));
                    }
                    else
                    {
                        result.Append(",\"" + f.Name + "\":\"" + JsonEncode(this.GetStringValue(f.Name)) + "\"");
                    }
                }
            }

            result.Append("}");

            return result.ToString();
        }

        private String JsonEncode(String value)
        {
            if (value == null)
            {
                return null;
            }

            value = value.Replace("\"", "\\\"");

            return value;
        }
       

        public void Save()
        {
            if (this.Status == ItemStatus.Unchanged)
            {
                return;
            }

            String endpoint = ((ODataEntityType)this.Type).Url;

            String json = GetJson();

            XmlHttpRequest xhr = new XmlHttpRequest();

            xhr.Open("POST", endpoint);
            xhr.SetRequestHeader("Accept", "application/json;odata=minimalmetadata");
            xhr.SetRequestHeader("Content-Type", "application/json;odata=minimalmetadata");
         //   xhr.SetRequestHeader("DataServiceVersion", "DataServiceVersion: 3.0;NetFx");
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
            }
        }
    }
}