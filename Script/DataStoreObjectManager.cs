using System;
using System.Collections.Generic;

namespace BL.Data
{
    public class DataStoreObjectManager
    {
        private IDataStore store;

        private Dictionary<String, ISerializableCollection> objectCollectionsByType;
        private Dictionary<String, SerializableType> serializableTypes;

        private Operation provisionedOperation;
        private bool isProvisioned;
        private int dataTypesLoaded = 0;

        public bool IsProvisioned
        {
            get
            {
                return this.isProvisioned;
            }
        }

        public IDataStore Store
        { 
            get
            {
                return this.store;
            }
        }

        public DataStoreObjectManager(IDataStore coreStore)
        {
            this.store = coreStore;
            this.objectCollectionsByType = new Dictionary<String, ISerializableCollection>();
            this.serializableTypes = new Dictionary<string, SerializableType>();
        }

        public void EnsureProvisionedAndLoaded(AsyncCallback callback, object state)
        {
            if (this.isProvisioned)
            {
                if (callback != null)
                {
                    CallbackResult.NotifySynchronousSuccess(callback, state, this);
                }

                return;
            }

            bool isNew = false;

            if (this.provisionedOperation == null)
            {
                this.provisionedOperation = new Operation();
                isNew = true;
            }

            this.provisionedOperation.CallbackStates.Add(CallbackState.Wrap(callback, state));

            if (isNew)
            {
                this.store.EnsureProvisioned(this.HandleDataStoreProvisioned, null);
            }
        }

        private void HandleDataStoreProvisioned(IAsyncResult result)
        {
            dataTypesLoaded += 1000;

            // Log.DA("Now loading " + this.store.Types);

            foreach (IDataStoreType dt in this.store.Types)
            {
                dataTypesLoaded++;
                dt.EnsureAllItemsSet().Retrieve(this.HandleDataTypeLoaded, null);
            }

            dataTypesLoaded -= 999;

            this.HandleDataTypeLoaded(null);
        }

        private void HandleDataTypeLoaded(IAsyncResult result)
        {
            dataTypesLoaded--;

            Operation o = this.provisionedOperation;

            if (this.dataTypesLoaded <= 0 && this.provisionedOperation != null)
            {
                this.isProvisioned = true;

                // Log.DAO("Data types all loaded", result);

                this.provisionedOperation.CompleteAsAsyncDone(null);
                this.provisionedOperation = null;
            }
        }

        public IDataStoreType Type(String typeName)
        {
            return this.store.Type(typeName);
        }

        public IDataStoreItemSet AllLocalItems(String typeName)
        {
            return this.store.Type(typeName).EnsureAllItemsSet();
        }


        public void Save()
        {
            foreach (IDataStoreType type in this.store.Types)
            {
                type.Save(null, null);
             //   type.EnsureAllItemsSet().
            }
        }


        public void EnsureType(String typeName, ISerializableCollection localItemCollection)
        {
            IDataStoreType dsType = this.store.Type(typeName);

            SerializableObject newProto = localItemCollection.Create();

            SerializableType newType = newProto.SerializableType;
            
            // Log.DAO("Initializing new data store type.", dsType);

            foreach (SerializableProperty sp in newType.Properties)
            {
                dsType.AssumeField(sp.Name, FieldTypeFromPropertyType(sp.Type));
            }

            this.serializableTypes[typeName] = newType;
            this.objectCollectionsByType[typeName] = localItemCollection;

            // Log.DAO("Done initializing new data store type.", dsType);
        }

        public static FieldType FieldTypeFromPropertyType(SerializablePropertyType propertyType)
        {
            switch (propertyType)
            {
                case SerializablePropertyType.Bool:
                    return FieldType.BoolChoice;

                case SerializablePropertyType.Choice:
                    return FieldType.ShortText;

                case SerializablePropertyType.Date:
                    return FieldType.DateTime;

                case SerializablePropertyType.Integer:
                    return FieldType.Integer;

                case SerializablePropertyType.Number:
                    return FieldType.BigNumber;

                case SerializablePropertyType.Object:
                    throw new Exception("Not implemented.");

                default:
                    return FieldType.ShortText;
            }
        }
    }
}
