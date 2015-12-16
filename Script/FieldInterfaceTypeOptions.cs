/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;

#if NET
using Bendyline.Base;

namespace Bendyline.Data
#elif SCRIPTSHARP
using System.Runtime.CompilerServices;

namespace BL.Data
#endif
{
    public enum ScaleType
    {
        FiveValues = 0,
        SevenValues = 1,
        FiveStars = 2,
        FiveAgree = 3
    }

    public class FieldInterfaceTypeOptions : SerializableObject
    {
        private String rangeStartDescription;
        private String rangeEndDescription;
        private String placeholder;
        private Nullable<ScaleType> scaleType = null;
        private bool displayKey = false;
        private String relatedField0;
        private String relatedField1;
        private Nullable<int> fontSize;
        private Nullable<int> styleId;

        private Nullable<int> suggestedWidth = null;
        private Nullable<int> suggestedHeight = null;

        private String stringFalseValue = null;
        private String stringTrueValue = null;

        private Nullable<int> intFalseValue = null;
        private Nullable<int> intTrueValue = null;
        private Nullable<int> intMinValue = null;
        private Nullable<int> intMaxValue = null;
        private Nullable<int> intDefaultValue = null;

        [ScriptName("i_styleId")]
        public Nullable<int> StyleId
        {
            get
            {
                return this.styleId;
            }

            set
            {
                if (this.styleId == value)
                {
                    return;
                }

                this.styleId = value;

                this.NotifyPropertyChanged("StyleId");
            }
        }
       
        [ScriptName("i_fontSize")]
        public Nullable<int> FontSize
        { 
            get
            {
                return this.fontSize;
            }

            set
            {
                if (this.fontSize == value)
                {
                    return;
                }

                this.fontSize = value;

                this.NotifyPropertyChanged("FontSize");
            }
        }

        [ScriptName("i_intMinValue")]
        public Nullable<int> IntMinValue
        { 
            get
            {
                return this.intMinValue;
            }

            set
            {
                if (this.intMinValue == value)
                {
                    return;
                }

                this.intMinValue = value;

                this.NotifyPropertyChanged("IntMinValue");
            }
        }

        [ScriptName("i_suggestedWidth")]
        public Nullable<int> SuggestedWidth
        {
            get
            {
                return this.suggestedWidth;
            }

            set
            {
                if (this.suggestedWidth == value)
                {
                    return;
                }

                this.suggestedWidth = value;

                this.NotifyPropertyChanged("SuggestedWidth");
            }
        }

        [ScriptName("i_suggestedHeight")]
        public Nullable<int> SuggestedHeight
        {
            get
            {
                return this.suggestedHeight;
            }

            set
            {
                if (this.suggestedHeight == value)
                {
                    return;
                }

                this.suggestedHeight = value;

                this.NotifyPropertyChanged("SuggestedHeight");
            }
        }

        [ScriptName("i_stringFalseValue")]
        public String StringFalseValue
        {
            get
            {
                return this.stringFalseValue;
            }

            set
            {
                if (this.stringFalseValue == value)
                {
                    return;
                }

                this.stringFalseValue = value;

                this.NotifyPropertyChanged("StringFalseValue");
            }
        }

        [ScriptName("i_stringTrueValue")]
        public String StringTrueValue
        {
            get
            {
                return this.stringFalseValue;
            }

            set
            {
                if (this.stringTrueValue == value)
                {
                    return;
                }

                this.stringTrueValue = value;

                this.NotifyPropertyChanged("StringTrueValue");
            }
        }

        [ScriptName("i_intFalseValue")]
        public Nullable<int> IntFalseValue
        {
            get
            {
                return this.intFalseValue;
            }

            set
            {
                if (this.intFalseValue == value)
                {
                    return;
                }

                this.intFalseValue = value;

                this.NotifyPropertyChanged("IntFalseValue");
            }
        }

        [ScriptName("i_intTrueValue")]
        public Nullable<int> IntTrueValue
        {
            get
            {
                return this.intTrueValue;
            }

            set
            {
                if (this.intTrueValue == value)
                {
                    return;
                }

                this.intTrueValue = value;

                this.NotifyPropertyChanged("IntTrueValue");
            }
        }

        [ScriptName("i_intDefaultValue")]
        public Nullable<int> IntDefaultValue
        {
            get
            {
                return this.intDefaultValue;
            }

            set
            {
                if (this.intDefaultValue == value)
                {
                    return;
                }

                this.intDefaultValue = value;

                this.NotifyPropertyChanged("IntDefaultValue");
            }
        }

        [ScriptName("i_intMaxValue")]
        public Nullable<int> IntMaxValue
        {
            get
            {
                return this.intMaxValue;
            }

            set
            {
                if (this.intMaxValue == value)
                {
                    return;
                }

                this.intMaxValue = value;
            
                this.NotifyPropertyChanged("IntMaxValue");
            }
        }

        [ScriptName("s_relatedField0")]
        public String RelatedField0
        {
            get
            {
                return this.relatedField0;
            }

            set
            {
                if (this.relatedField0 == value)
                {
                    return;
                }

                this.relatedField0 = value;

                this.NotifyPropertyChanged("RelatedField0");
            }
        }

        [ScriptName("s_relatedField1")]
        public String RelatedField1
        {
            get
            {
                return this.relatedField1;
            }

            set
            {
                if (this.relatedField1 == value)
                {
                    return;
                }

                this.relatedField1 = value;

                this.NotifyPropertyChanged("RelatedField1");
            }
        }

        [ScriptName("b_displayKey")]
        public bool DisplayKey
        {
            get
            {
                return this.displayKey;
            }

            set
            {
                if (this.displayKey == value)
                {
                    return;
                }

                this.displayKey = value;

                this.NotifyPropertyChanged("DisplayKey");
            }
        }

        [ScriptName("i_scaleType")]
        public Nullable<ScaleType> ScaleType
        {
            get
            {
                return this.scaleType;
            }

            set
            {
                if (this.scaleType == value)
                {
                    return;
                }

                this.scaleType = value;

                this.NotifyPropertyChanged("ScaleType");
            }
        }

        [ScriptName("s_rangeStartDescription")]
        public String RangeStartDescription
        {
            get
            {
                return this.rangeStartDescription;
            }

            set
            {
                if (this.rangeStartDescription == value)
                {
                    return;
                }

                this.rangeStartDescription = value;

                this.NotifyPropertyChanged("RangeStartDescription");
            }
        }
        
        [ScriptName("s_rangeEndDescription")]
        public String RangeEndDescription
        {
            get
            {
                return this.rangeEndDescription;
            }

            set
            {
                if (this.rangeEndDescription == value)
                {
                    return;
                }

                this.rangeEndDescription = value;

                this.NotifyPropertyChanged("RangeEndDescription");
            }
        }

        [ScriptName("s_placeholder")]
        public String Placeholder
        {
            get
            {
                return this.placeholder;
            }

            set
            {
                if (this.placeholder == value)
                {
                    return;
                }

                this.placeholder = value;

                this.NotifyPropertyChanged("Placeholder");
            }
        }

        public bool IsDefined
        {
            get
            {
                return this.placeholder != null || this.rangeEndDescription != null || this.rangeStartDescription != null;
            }
        }

        public FieldInterfaceTypeOptions()
        {

        }

        public virtual bool IsEqualTo(FieldInterfaceTypeOptions fuio)
        {
            if (this.Placeholder != fuio.Placeholder)
            {
                return false;
            }

            if (this.RangeEndDescription != fuio.RangeEndDescription)
            {
                return false;
            }

            if (this.RangeStartDescription != fuio.RangeStartDescription)
            {
                return false;
            }

            if (this.ScaleType != fuio.ScaleType)
            {
                return false;
            }

            if (this.DisplayKey != fuio.DisplayKey)
            {
                return false;
            }

            return true;
        }
    }
}
