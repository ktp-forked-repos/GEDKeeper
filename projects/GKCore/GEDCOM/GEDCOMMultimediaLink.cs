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
using BSLib;
using GKCore;

namespace GKCommon.GEDCOM
{
    public sealed class GEDCOMMultimediaLink : GEDCOMPointer
    {
        private GEDCOMList<GEDCOMFileReference> fFileReferences;

        public GEDCOMList<GEDCOMFileReference> FileReferences
        {
            get { return fFileReferences; }
        }

        public string Title
        {
            get { return GetTagStringValue(GEDCOMTagType.TITL); }
            set { SetTagStringValue(GEDCOMTagType.TITL, value); }
        }

        /// <summary>
        /// The _PRIM tag is often added by genealogy programs to signify that this picture is the PRIMary picture,
        /// or the picture that should be used for charts.
        /// See the following programs: PhpGedView, AQ3, PAF5, FO7.
        /// </summary>
        public bool IsPrimary
        {
            get { return GetTagYNValue(GEDCOMTagType._PRIM); }
            set { SetTagYNValue(GEDCOMTagType._PRIM, value); }
        }

        /// <summary>
        /// It is acceptable to export information to sections of the referenced image files.
        /// To export this information, the use of user-defined tags _PRIM_CUTOUT and _POSITION is agreed.
        /// See the following programs: FTB.
        /// </summary>
        public bool IsPrimaryCutout
        {
            get { return GetTagYNValue(GEDCOMTagType._PRIM_CUTOUT); }
            set { SetTagYNValue(GEDCOMTagType._PRIM_CUTOUT, value); }
        }

        public GEDCOMCutoutPosition CutoutPosition
        {
            get { return TagClass(GEDCOMTagType._POSITION, GEDCOMCutoutPosition.Create) as GEDCOMCutoutPosition; }
        }


        public new static GEDCOMTag Create(GEDCOMObject owner, string tagName, string tagValue)
        {
            return new GEDCOMMultimediaLink(owner, tagName, tagValue);
        }

        public GEDCOMMultimediaLink(GEDCOMObject owner) : base(owner)
        {
            SetName(GEDCOMTagType.OBJE);
            fFileReferences = new GEDCOMList<GEDCOMFileReference>(this);
        }

        public GEDCOMMultimediaLink(GEDCOMObject owner, string tagName, string tagValue) : this(owner)
        {
            SetNameValue(tagName, tagValue);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fFileReferences.Dispose();
            }
            base.Dispose(disposing);
        }

        public override GEDCOMTag AddTag(string tagName, string tagValue, TagConstructor tagConstructor)
        {
            GEDCOMTag result;

            if (tagName == GEDCOMTagType.FILE) {
                result = fFileReferences.Add(new GEDCOMFileReference(this, tagName, tagValue));
            } else {
                result = base.AddTag(tagName, tagValue, tagConstructor);
            }

            return result;
        }

        public override void Clear()
        {
            base.Clear();
            fFileReferences.Clear();
        }

        public override bool IsEmpty()
        {
            bool result;
            if (IsPointer) {
                result = base.IsEmpty();
            } else {
                result = (Count == 0 && (fFileReferences.Count == 0));
            }
            return result;
        }

        protected override string GetStringValue()
        {
            string result = IsPointer ? base.GetStringValue() : fStringValue;
            return result;
        }

        public override string ParseString(string strValue)
        {
            fStringValue = string.Empty;
            return base.ParseString(strValue);
        }

        public override void SaveToStream(StreamWriter stream, int level)
        {
            base.SaveToStream(stream, level);
            fFileReferences.SaveToStream(stream, ++level);
        }

        #region Utilities

        public string GetUID()
        {
            string result = null;
            try {
                if (Value != null) {
                    ExtRect cutoutArea;
                    if (IsPrimaryCutout) {
                        cutoutArea = CutoutPosition.Value;
                    } else {
                        cutoutArea = ExtRect.CreateEmpty();
                    }

                    GEDCOMMultimediaRecord mmRec = (GEDCOMMultimediaRecord)Value;
                    result = mmRec.UID + "-" + GKUtils.GetRectUID(cutoutArea.Left, cutoutArea.Top, cutoutArea.Right, cutoutArea.Bottom);
                }
            } catch (Exception ex) {
                Logger.LogWrite("GEDCOMMultimediaLink.GetUID(): " + ex.Message);
                result = null;
            }
            return result;
        }

        #endregion
    }
}
