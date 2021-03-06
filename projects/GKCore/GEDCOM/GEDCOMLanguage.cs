﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2019 by Sergey V. Zhdanovskih.
 *
 *  This file is part of "GEDKeeper".
 *
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;

namespace GKCommon.GEDCOM
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GEDCOMLanguage : GEDCOMTag
    {
        private GEDCOMLanguageID fValue;

        public GEDCOMLanguageID Value
        {
            get { return fValue; }
            set { fValue = value; }
        }

        public override void Clear()
        {
            base.Clear();
            fValue = GEDCOMLanguageID.Unknown;
        }

        public override bool IsEmpty()
        {
            return base.IsEmpty() && (fValue == GEDCOMLanguageID.Unknown);
        }

        public override void Assign(GEDCOMTag source)
        {
            GEDCOMLanguage srcLang = (source as GEDCOMLanguage);
            if (srcLang == null)
                throw new ArgumentException(@"Argument is null or wrong type", "source");

            base.Assign(source);

            fValue = srcLang.fValue;
        }

        public override string ParseString(string strValue)
        {
            fValue = GEDCOMUtils.GetLanguageVal(strValue);
            return string.Empty;
        }

        protected override string GetStringValue()
        {
            return GEDCOMUtils.GetLanguageStr(fValue);
        }

        public GEDCOMLanguage(GEDCOMObject owner) : base(owner)
        {
            SetName(GEDCOMTagType.LANG);
        }

        public GEDCOMLanguage(GEDCOMObject owner, string tagName, string tagValue) : this(owner)
        {
            SetNameValue(tagName, tagValue);
        }

        public new static GEDCOMTag Create(GEDCOMObject owner, string tagName, string tagValue)
        {
            return new GEDCOMLanguage(owner, tagName, tagValue);
        }
    }
}
