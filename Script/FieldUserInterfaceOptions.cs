/* Copyright (c) Bendyline LLC. All rights reserved. Licensed under the Apache License, Version 2.0.
    You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0. */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BL.Data
{
    public enum ScaleType
    {
        FiveValues = 0,
        SevenValues = 1,
        FiveStars = 2,
        FiveAgree = 3
    }

    public class FieldUserInterfaceOptions : SerializableObject
    {
        private String rangeStartDescription;
        private String rangeEndDescription;
        private String placeholder;
        private ScaleType scaleType = ScaleType.FiveValues;
        private bool displayKey = false;

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
        public ScaleType ScaleType
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

        public FieldUserInterfaceOptions()
        {

        }

        public virtual bool IsEqualTo(FieldUserInterfaceOptions fuio)
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
