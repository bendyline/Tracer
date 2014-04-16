/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BL.Data
{
    public class FieldChoiceCollection
    {
        private ArrayList choices;
        private Dictionary<String, FieldChoice> choicesById;

        public ArrayList Choices
        {
            get
            {
                return this.choices;
            }
        }

        public FieldChoice this[int index]
        {
            get
            {
                return (FieldChoice)this.choices[index];
            }
        }

        public IEnumerator GetEnumerator()
        {
            return this.choices.GetEnumerator();
        }

        public FieldChoiceCollection()
        {
            this.choices = new ArrayList();
            this.choicesById = new Dictionary<string, FieldChoice>();
        }

        public FieldChoice GetById(String id)
        {
            return this.choicesById[id];
        }

        public void Clear()
        {
            this.choices.Clear();
            this.choicesById.Clear();
        }

        public FieldChoice Create()
        {
            FieldChoice sens = new FieldChoice();

            return sens;
        }

        public void Add(FieldChoice template)
        {
            this.choices.Add(template);
            this.choicesById[((FieldChoice)template).Id] = (FieldChoice)template;
        }
    }
}
