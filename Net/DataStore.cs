/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

namespace Bendyline.Data
{
    public enum DataStoreType
    {
        SharePoint
    }

    public static class DataStore
    {
        public static IDataStore Create(DataStoreType type)
        {
            switch (type)
            {
                case DataStoreType.SharePoint:
                    return CreateByTypeName("Bendyline.Base.SharePointClient.SharePointDataStore" + Utilities.SharePointClientTypeSuffix);
            }

            return null;
        }

        public static IDataStore CreateByTypeName(String typeName)
        {
            Type t = Type.GetType(typeName);

            if (t == null)
            {
                throw new InvalidOperationException(String.Format("Specified type '{0}' is not available.", typeName));
            }

            object o = Activator.CreateInstance(t);

            if (o is IDataStore)
            {
                return ((IDataStore)o);
            }

            throw new InvalidOperationException(String.Format("Specified type '{0}' is not of the appropriate type.", typeName));
        }


    }
}
