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
        SevenValues = 1
    }

    public class FieldUserInterfaceOptions : SerializableObject
    {
        private String rangeStartDescription;
        private String rangeEndDescription;
        private ScaleType scaleType = ScaleType.FiveValues;

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

        public FieldUserInterfaceOptions()
        {

        }
    }
}
