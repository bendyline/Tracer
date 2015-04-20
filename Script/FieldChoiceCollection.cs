/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class FieldChoiceCollection : ISerializableCollection, IEnumerable
    {
        private ArrayList choices;

        public ArrayList Choices
        {
            get
            {
                return this.choices;
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.choices.GetEnumerator();
        }

        public FieldChoiceCollection()
        {
            this.choices = new ArrayList();
        }


        public void Clear()
        {
            this.choices.Clear();
        }

        public SerializableObject Create()
        {
            FieldChoice choice = new FieldChoice();

            return choice;
        }

        public void Add(SerializableObject choice)
        {
            this.choices.Add(choice);
        }
    }
}
