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
    public class FieldChoiceCollection : FieldChoiceCollectionBase
    {
        private ArrayList choices;

        public override event NotifyCollectionChangedEventHandler CollectionChanged;


        public IEnumerable Choices
        {
            get
            {
                return this.choices;
            }
        }

        public override int Count
        {
            get
            {
                return this.choices.Count;
            }
        }

        public override object this[int index] 
        { 
            get
            {
                return this.choices[index];
            }
        }

        public override IEnumerator GetEnumerator()
        {
            return this.choices.GetEnumerator();
        }

        public FieldChoiceCollection()
        {
            this.choices = new ArrayList();
        }


        public override void Clear()
        {
            this.choices.Clear();
        }

        public override SerializableObject Create()
        {
            FieldChoice choice = new FieldChoice();

            choice.PropertyChanged += choice_PropertyChanged;

            return choice;
        }

        private void choice_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, NotifyCollectionChangedEventArgs.ItemStateChange(sender, e.PropertyName));
            }
        }

        public override void Add(SerializableObject choice)
        {
            this.choices.Add(choice);

            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, NotifyCollectionChangedEventArgs.ItemAdded(choice));
            }
        }
        public override void Remove(SerializableObject choice)
        {
            this.choices.Remove(choice);

            if (this.CollectionChanged != null)
            {
                this.CollectionChanged(this, NotifyCollectionChangedEventArgs.ItemRemoved(choice));
            }
        }
    }
}
