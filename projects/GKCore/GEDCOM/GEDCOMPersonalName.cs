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
using System.IO;

namespace GKCommon.GEDCOM
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GEDCOMPersonalName : GEDCOMTag
    {
        private string fFirstPart;
        private string fSurname;
        private string fLastPart;
        private GEDCOMPersonalNamePieces fPieces;

        public string FullName
        {
            get {
                string result = fFirstPart;
                if (!string.IsNullOrEmpty(fSurname)) {
                    result += " " + fSurname;
                    if (!string.IsNullOrEmpty(fLastPart)) {
                        result += " " + fLastPart;
                    }
                }
                return result;
            }
        }

        public string FirstPart
        {
            get { return fFirstPart; }
            set { fFirstPart = GEDCOMUtils.Trim(value); }
        }

        public string Surname
        {
            get { return fSurname; }
            set { fSurname = GEDCOMUtils.Trim(value); }
        }

        public string LastPart
        {
            get { return fLastPart; }
            set { fLastPart = GEDCOMUtils.Trim(value); }
        }

        public GEDCOMPersonalNamePieces Pieces
        {
            get { return fPieces; }
        }

        public GEDCOMNameType NameType
        {
            get { return GEDCOMUtils.GetNameTypeVal(GetTagStringValue(GEDCOMTagType.TYPE)); }
            set { SetTagStringValue(GEDCOMTagType.TYPE, GEDCOMUtils.GetNameTypeStr(value)); }
        }

        public GEDCOMLanguage Language
        {
            get { return TagClass("_LANG", GEDCOMLanguage.Create) as GEDCOMLanguage; }
        }


        // see "THE GEDCOM STANDARD Release 5.5.1", p.54 ("NAME_PERSONAL")
        protected override string GetStringValue()
        {
            string result = fFirstPart;
            if (!string.IsNullOrEmpty(fSurname)) {
                result += " /" + fSurname + "/";
                if (!string.IsNullOrEmpty(fLastPart)) {
                    result += " " + fLastPart;
                }
            }
            return result;
        }

        public override string ParseString(string strValue)
        {
            return GEDCOMUtils.ParseName(strValue, this);
        }

        public void SetNameParts(string firstPart, string surname, string lastPart)
        {
            fFirstPart = GEDCOMUtils.Trim(firstPart);
            fSurname = GEDCOMUtils.Trim(surname);
            fLastPart = GEDCOMUtils.Trim(lastPart);
        }

        public new static GEDCOMTag Create(GEDCOMObject owner, string tagName, string tagValue)
        {
            return new GEDCOMPersonalName(owner, tagName, tagValue);
        }

        public GEDCOMPersonalName(GEDCOMObject owner) : base(owner)
        {
            SetName(GEDCOMTagType.NAME);

            fPieces = new GEDCOMPersonalNamePieces(this);

            fFirstPart = string.Empty;
            fSurname = string.Empty;
            fLastPart = string.Empty;
        }

        public GEDCOMPersonalName(GEDCOMObject owner, string tagName, string tagValue) : this(owner)
        {
            SetNameValue(tagName, tagValue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fPieces.Dispose();
            }
            base.Dispose(disposing);
        }

        public override GEDCOMTag AddTag(string tagName, string tagValue, TagConstructor tagConstructor)
        {
            GEDCOMTag result;

            if (tagName == GEDCOMTagType.TYPE || tagName == GEDCOMTagType.FONE || tagName == GEDCOMTagType.ROMN || tagName == "_LANG") {
                result = base.AddTag(tagName, tagValue, tagConstructor);
            } else {
                result = fPieces.AddTag(tagName, tagValue, tagConstructor);
            }

            return result;
        }

        public override void Assign(GEDCOMTag source)
        {
            GEDCOMPersonalName otherName = (source as GEDCOMPersonalName);
            if (otherName == null)
                throw new ArgumentException(@"Argument is null or wrong type", "source");

            base.Assign(otherName);

            fFirstPart = otherName.fFirstPart;
            fSurname = otherName.fSurname;
            fLastPart = otherName.fLastPart;

            fPieces.Assign(otherName.Pieces);
        }

        public override void Clear()
        {
            base.Clear();

            fFirstPart = string.Empty;
            fSurname = string.Empty;
            fLastPart = string.Empty;

            if (fPieces != null)
                fPieces.Clear();
        }

        public override bool IsEmpty()
        {
            return base.IsEmpty()
                && string.IsNullOrEmpty(fFirstPart) && string.IsNullOrEmpty(fSurname) && string.IsNullOrEmpty(fLastPart)
                && fPieces.IsEmpty();
        }

        public override void Pack()
        {
            base.Pack();
            fPieces.Pack();
        }

        public override void ReplaceXRefs(XRefReplacer map)
        {
            base.ReplaceXRefs(map);
            fPieces.ReplaceXRefs(map);
        }

        public override void SaveToStream(StreamWriter stream, int level)
        {
            base.SaveToStream(stream, level);
            fPieces.SaveToStream(stream, level); // same level
        }

        private static bool IsUnknown(string str)
        {
            return string.Equals(str, "?") || (string.Compare(str, "unknown", true) == 0);
        }

        public float IsMatch(GEDCOMPersonalName otherName, bool onlyFirstPart)
        {
            if (otherName == null)
                return 0.0f;

            int parts = 0;
            float matches = 0;
            bool surnameMatched = false;

            if (!(string.IsNullOrEmpty(otherName.FirstPart) && string.IsNullOrEmpty(fFirstPart))) {
                parts++;
                if (otherName.FirstPart == fFirstPart)
                    matches++;
            }

            if (!onlyFirstPart) {
                if (!(string.IsNullOrEmpty(otherName.Surname) && string.IsNullOrEmpty(fSurname))) {
                    if (IsUnknown(otherName.Surname) && IsUnknown(fSurname)) {
                        // not really matched, surname isn't known,
                        // don't count as part being checked, and don't penalize
                        surnameMatched = true;
                    } else {
                        parts++;
                        if (otherName.Surname == fSurname) {
                            matches++;
                            surnameMatched = true;
                        }
                    }
                } else {
                    // pretend the surname matches
                    surnameMatched = true;
                }
            } else {
                // pretend the surname matches
                surnameMatched = true;
            }

            if (!(string.IsNullOrEmpty(otherName.Pieces.Nickname) && string.IsNullOrEmpty(fPieces.Nickname))) {
                parts++;
                if (otherName.Pieces.Nickname == fPieces.Nickname)
                    matches++;
            }

            float match = (parts == 0) ? 0.0f : (matches / parts) * 100.0f;

            // heavily penalise the surname not matching
            // for this to work correctly better matching needs to be
            // performed, not just string comparison
            if (!onlyFirstPart && !surnameMatched) {
                match *= 0.25f;
            }

            return match;
        }
    }
}
