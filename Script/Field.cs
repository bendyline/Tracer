/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL.Data
{
    public class Field : IDataStoreField
    {
        private String name;
        private String displayName;
        private bool required;
        private FieldType fieldType;
        private FieldUserInterfaceType userInterfaceType;
        private FieldUserInterfaceOptions userInterfaceOptions;
        private FieldChoiceCollection fieldChoices;

        public event PropertyChangedEventHandler PropertyChanged;

        [ScriptName("b_required")]
        public bool Required
        {
            get
            {
                return this.required;
            }

            set
            {
                this.required = value;
            }
        }

        [ScriptName("o_userInterfaceOptions")]
        public FieldUserInterfaceOptions UserInterfaceOptions
        {
            get
            {
                if (this.userInterfaceOptions == null)
                {
                    this.userInterfaceOptions = new FieldUserInterfaceOptions();
                }

                return this.userInterfaceOptions;
            }

            set
            {
                if (this.userInterfaceOptions == value)
                {
                    return;
                }

                this.userInterfaceOptions = value;

                this.NotifyPropertyChanged("UserInterfaceOptions");
            }
        }

        public FieldChoiceCollection Choices
        {
            get
            {
                if (this.fieldChoices == null)
                {
                    this.fieldChoices = new FieldChoiceCollection();
                }

                return this.fieldChoices;

            }
        }

        public String DisplayName
        {
            get
            {
                if (this.displayName == null)
                {
                    return this.name;
                }

                return this.displayName;
            }

            set
            {
                if (this.displayName == value)
                {
                    return;
                }

                this.displayName = value;

                this.NotifyPropertyChanged("DisplayName");
            }
        }

        public String Name
        {
            get
            {
                return this.name;
            }
        }

        public FieldType Type
        {
            get
            {
                return this.fieldType;
            }
        }

        public FieldUserInterfaceType UserInterfaceType
        {
            get
            {
                return this.userInterfaceType;
            }
            set
            {
                if (this.userInterfaceType == value)
                {
                    return;
                }

                this.userInterfaceType = value;

                this.NotifyPropertyChanged("UserInterfaceType");
            }
        } 

        public Field(String name, FieldType fieldType)
        {
            this.name = name;
            this.fieldType = fieldType;
        }

        protected void NotifyPropertyChanged(String propertyName)
        {
            if (this.PropertyChanged != null)
            {
                PropertyChangedEventArgs pcea = new PropertyChangedEventArgs(propertyName);

                this.PropertyChanged(this, pcea);
            }
        }
    }
}
