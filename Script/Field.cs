/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

#if NET
using Bendyline.Base;
using System.ComponentModel;

namespace Bendyline.Data
#elif SCRIPTSHARP
using System.Runtime.CompilerServices;

namespace BL.Data
#endif
{
    public class Field : IDataStoreField
    {
        private String name;
        private String displayName;
        private bool required;
        private bool allowNull;
        private FieldType fieldType;
        private FieldInterfaceType interfaceType;
        private FieldInterfaceTypeOptions interfaceTypeOptions;
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

        [ScriptName("b_allowNull")]
        public bool AllowNull
        {
            get
            {
                return this.allowNull;
            }

            set
            {
                this.allowNull = value;
            }
        }

        [ScriptName("o_interfaceTypeOptions")]
        public FieldInterfaceTypeOptions InterfaceTypeOptions
        {
            get
            {
                if (this.interfaceTypeOptions == null)
                {
                    this.interfaceTypeOptions = new FieldInterfaceTypeOptions();
                }

                return this.interfaceTypeOptions;
            }

            set
            {
                if (this.interfaceTypeOptions == value)
                {
                    return;
                }

                this.interfaceTypeOptions = value;

                this.NotifyPropertyChanged("InterfaceTypeOptions");
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

        public FieldInterfaceType InterfaceType
        {
            get
            {
                return this.interfaceType;
            }
            set
            {
                if (this.interfaceType == value)
                {
                    return;
                }

                this.interfaceType = value;

                this.NotifyPropertyChanged("InterfaceType");
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
