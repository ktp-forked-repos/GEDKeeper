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

using System.IO;

namespace GKCommon.GEDCOM
{
    public sealed class GEDCOMMultimediaRecord : GEDCOMRecord
    {
        private readonly GEDCOMList<GEDCOMFileReferenceWithTitle> fFileReferences;

        public GEDCOMList<GEDCOMFileReferenceWithTitle> FileReferences
        {
            get { return fFileReferences; }
        }


        public GEDCOMMultimediaRecord(GEDCOMObject owner) : base(owner)
        {
            SetRecordType(GEDCOMRecordType.rtMultimedia);
            SetName(GEDCOMTagType.OBJE);

            fFileReferences = new GEDCOMList<GEDCOMFileReferenceWithTitle>(this);
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
                result = fFileReferences.Add(new GEDCOMFileReferenceWithTitle(this, tagName, tagValue));
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
            return base.IsEmpty() && fFileReferences.Count == 0;
        }

        public override void Pack()
        {
            base.Pack();
            fFileReferences.Pack();
        }

        public override void ReplaceXRefs(XRefReplacer map)
        {
            base.ReplaceXRefs(map);
            fFileReferences.ReplaceXRefs(map);
        }

        public override void SaveToStream(StreamWriter stream, int level)
        {
            base.SaveToStream(stream, level);
            fFileReferences.SaveToStream(stream, ++level);
        }

        #region Auxiliary

        public string GetFileTitle()
        {
            if (fFileReferences.Count <= 0) return string.Empty;

            GEDCOMFileReferenceWithTitle fileRef = fFileReferences[0];
            return fileRef.Title;
        }
        
        #endregion
    }
}
