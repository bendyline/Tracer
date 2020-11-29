/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System.Collections;

#if NET
using Bendyline.Base;
using Bendyline.Base.ScriptCompatibility;

namespace Bendyline.Data
#elif SCRIPTSHARP

namespace BL.Data
#endif
{
    public abstract class FieldChoiceCollectionBase : IEnumerable, ISerializableCollection, INotifyCollectionAndStateChanged
    {

        public abstract event NotifyCollectionChangedEventHandler CollectionChanged;

        public abstract int Count { get;  }

        public abstract object this[int index] { get; }


        public abstract IEnumerator GetEnumerator();

        public abstract void Clear();
        public abstract SerializableObject Create();
        public abstract void Add(SerializableObject o);

        public abstract void Remove(SerializableObject o);

    }
}
