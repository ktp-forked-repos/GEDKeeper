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
using System.Collections.Generic;
using System.Globalization;

namespace GKCommon.GEDCOM
{
    public delegate void ProgressEventHandler(object sender, int progress);

    /// <summary>
    /// 
    /// </summary>
    public sealed class GEDCOMTree : GEDCOMObject
    {
        #region Tree Enumerator

        private struct TreeEnumerator : IGEDCOMTreeEnumerator
        {
            private readonly GEDCOMTree fTree;
            private readonly GEDCOMRecordType fRecType;
            private readonly int fEndIndex;
            private int fIndex;

            public TreeEnumerator(GEDCOMTree tree, GEDCOMRecordType recType)
            {
                fTree = tree;
                fIndex = -1;
                fEndIndex = tree.RecordsCount - 1;
                fRecType = recType;
            }

            public bool MoveNext(out GEDCOMRecord current)
            {
                if (fRecType == GEDCOMRecordType.rtNone)
                {
                    if (fIndex < fEndIndex)
                    {
                        fIndex++;
                        current = fTree[fIndex];
                        return true;
                    }
                } else {
                    while (fIndex < fEndIndex)
                    {
                        fIndex++;
                        GEDCOMRecord rec = fTree[fIndex];
                        if (rec.RecordType == fRecType) {
                            current = rec;
                            return true;
                        }
                    }
                }

                fIndex = fEndIndex + 1;
                current = null;
                return false;
            }

            public void Reset()
            {
                fIndex = -1;
            }
        }

        #endregion


        private readonly GEDCOMHeader fHeader;
        private readonly GEDCOMList<GEDCOMRecord> fRecords;
        private readonly Dictionary<string, GEDCOMCustomRecord> fXRefIndex;

        private GEDCOMFormat fFormat;
        private int[] fLastIDs;
        private EventHandler fOnChange;
        private EventHandler fOnChanging;
        private ProgressEventHandler fOnProgressEvent;
        private GEDCOMState fState;
        private int fUpdateCount;


        public GEDCOMFormat Format
        {
            get { return fFormat; }
            internal set { fFormat = value; }
        }

        public GEDCOMHeader Header
        {
            get { return fHeader; }
        }

        public bool IsEmpty
        {
            get { return (fRecords.Count == 0); }
        }

        public int RecordsCount
        {
            get { return fRecords.Count; }
        }

        public GEDCOMRecord this[int index]
        {
            get { return fRecords[index]; }
        }

        public GEDCOMState State
        {
            get { return fState; }
            set { fState = value; }
        }

        public event EventHandler OnChange
        {
            add { fOnChange = value; }
            remove { if (fOnChange == value) fOnChange = null; }
        }

        public event EventHandler OnChanging
        {
            add { fOnChanging = value; }
            remove { if (fOnChanging == value) fOnChanging = null; }
        }

        public ProgressEventHandler OnProgress
        {
            get { return fOnProgressEvent; }
            set { fOnProgressEvent = value; }
        }


        public GEDCOMTree()
        {
            fXRefIndex = new Dictionary<string, GEDCOMCustomRecord>();
            fRecords = new GEDCOMList<GEDCOMRecord>(this);
            fHeader = new GEDCOMHeader(this);

            ResetLastIDs();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fHeader.Dispose();
                fRecords.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Internal

        static GEDCOMTree()
        {
            GEDCOMFactory f = GEDCOMFactory.GetInstance();

            f.RegisterTag(GEDCOMTagType.ADDR, GEDCOMAddress.Create);
            f.RegisterTag(GEDCOMTagType.CHAN, GEDCOMChangeDate.Create);
            f.RegisterTag(GEDCOMTagType.DATE, GEDCOMDateValue.Create);
            f.RegisterTag(GEDCOMTagType.FAMC, GEDCOMPointer.Create);
            f.RegisterTag(GEDCOMTagType.LANG, GEDCOMLanguage.Create);
            f.RegisterTag(GEDCOMTagType.MAP, GEDCOMMap.Create);
            f.RegisterTag(GEDCOMTagType.PLAC, GEDCOMPlace.Create);
            f.RegisterTag(GEDCOMTagType.TIME, GEDCOMTime.Create);

            f.RegisterTag(GEDCOMTagType.ANCI, GEDCOMPointer.Create);
            f.RegisterTag(GEDCOMTagType.BAPL, GEDCOMIndividualOrdinance.Create);
            f.RegisterTag(GEDCOMTagType.CONL, GEDCOMIndividualOrdinance.Create);
            f.RegisterTag(GEDCOMTagType.DESI, GEDCOMPointer.Create);
            f.RegisterTag(GEDCOMTagType.ENDL, GEDCOMIndividualOrdinance.Create);
            f.RegisterTag(GEDCOMTagType.SLGC, GEDCOMIndividualOrdinance.Create);
            f.RegisterTag(GEDCOMTagType.SLGS, GEDCOMSpouseSealing.Create);

            f.RegisterTag("_LANG", GEDCOMLanguage.Create);
            f.RegisterTag(GEDCOMTagType._LOC, GEDCOMPointer.Create);
            f.RegisterTag(GEDCOMTagType._POSITION, GEDCOMCutoutPosition.Create);
        }

        private static string GetSignByRecord(GEDCOMRecord record)
        {
            string result = string.Empty;
            if (record == null) return result;

            switch (record.RecordType)
            {
                case GEDCOMRecordType.rtIndividual:
                    result = "I";
                    break;
                case GEDCOMRecordType.rtFamily:
                    result = "F";
                    break;
                case GEDCOMRecordType.rtNote:
                    result = "N";
                    break;
                case GEDCOMRecordType.rtMultimedia:
                    result = "O";
                    break;
                case GEDCOMRecordType.rtSource:
                    result = "S";
                    break;
                case GEDCOMRecordType.rtRepository:
                    result = "R";
                    break;
                case GEDCOMRecordType.rtGroup:
                    result = "G";
                    break;
                case GEDCOMRecordType.rtResearch:
                    result = "RS";
                    break;
                case GEDCOMRecordType.rtTask:
                    result = "TK";
                    break;
                case GEDCOMRecordType.rtCommunication:
                    result = "CM";
                    break;
                case GEDCOMRecordType.rtLocation:
                    result = "L";
                    break;
                case GEDCOMRecordType.rtSubmission:
                    result = "????";
                    break;
                case GEDCOMRecordType.rtSubmitter:
                    result = "SUB";
                    break;
            }

            return result;
        }

        internal GEDCOMList<GEDCOMRecord> GetRecords()
        {
            return fRecords;
        }

        #endregion

        #region XRef Search

        private void XRefIndex_AddRecord(GEDCOMCustomRecord record)
        {
            if (record == null || string.IsNullOrEmpty(record.XRef)) return;

            bool exists = fXRefIndex.ContainsKey(record.XRef);
            if (!exists) fXRefIndex.Add(record.XRef, record);
        }

        private void XRefIndex_DeleteRecord(GEDCOMRecord record)
        {
            bool exists = fXRefIndex.ContainsKey(record.XRef);
            if (exists) fXRefIndex.Remove(record.XRef);
        }

        public GEDCOMRecord XRefIndex_Find(string xref)
        {
            if (string.IsNullOrEmpty(xref)) return null;

            GEDCOMCustomRecord record;
            if (fXRefIndex.TryGetValue(xref, out record)) {
                return (record as GEDCOMRecord);
            } else {
                return null;
            }
        }

        private void ResetLastIDs()
        {
            fLastIDs = new int[(int)GEDCOMRecordType.rtLast + 1];
        }

        public string XRefIndex_NewXRef(GEDCOMRecord record)
        {
            var invNFI = GEDCOMUtils.InvariantNumberFormatInfo;
            string sign = GetSignByRecord(record);
            string xref;

            int recType = (int)record.RecordType;
            int lastId = fLastIDs[recType];

            do {
                lastId++;
                xref = sign + lastId.ToString(invNFI);
            } while (fXRefIndex.ContainsKey(xref));

            fLastIDs[recType] = lastId;
            return xref;
        }

        public void SetXRef(string oldXRef, GEDCOMCustomRecord record)
        {
            if (!string.IsNullOrEmpty(oldXRef)) {
                bool exists = fXRefIndex.ContainsKey(oldXRef);
                if (exists) fXRefIndex.Remove(oldXRef);
            }

            XRefIndex_AddRecord(record);
        }

        #endregion

        #region Main functionality

        public List<T> GetRecords<T>() where T : GEDCOMRecord
        {
            List<T> result = new List<T>();

            for (int i = 0; i < fRecords.Count; i++) {
                T rec = fRecords[i] as T;
                if (rec != null) {
                    result.Add(rec);
                }
            }

            return result;
        }

        public IGEDCOMTreeEnumerator GetEnumerator(GEDCOMRecordType recType)
        {
            return new TreeEnumerator(this, recType);
        }

        public void Clear()
        {
            fHeader.Clear();
            fRecords.Clear();
            fXRefIndex.Clear();
            ResetLastIDs();
        }

        public GEDCOMRecord AddRecord(GEDCOMRecord record)
        {
            fRecords.Add(record);
            XRefIndex_AddRecord(record);
            return record;
        }

        /*public void Delete(int index)
        {
            XRefIndex_DeleteRecord(fRecords[index]);
            fRecords.DeleteAt(index);
        }*/

        public void DeleteRecord(GEDCOMRecord record)
        {
            XRefIndex_DeleteRecord(record);
            fRecords.Delete(record);
        }

        public GEDCOMRecord Extract(int index)
        {
            XRefIndex_DeleteRecord(fRecords[index]);
            return fRecords.Extract(index);
        }

        public int IndexOf(GEDCOMRecord record)
        {
            return fRecords.IndexOf(record);
        }

        public GEDCOMRecord FindUID(string uid)
        {
            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = fRecords[i];
                if (rec.UID == uid) {
                    return rec;
                }
            }

            return null;
        }

        public void Pack()
        {
            int num = fRecords.Count;
            for (int i = 0; i < num; i++) {
                fRecords[i].Pack();
            }
        }

        #endregion

        #region Auxiliary

        public int[] GetRecordStats()
        {
            int[] stats = new int[((int)GEDCOMRecordType.rtLast)];

            int num = RecordsCount;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                int index = (int)rec.RecordType;
                stats[index] += 1;
            }

            return stats;
        }

        public GEDCOMSubmitterRecord GetSubmitter()
        {
            GEDCOMSubmitterRecord submitter = fHeader.Submitter.Value as GEDCOMSubmitterRecord;
            if (submitter == null) {
                submitter = new GEDCOMSubmitterRecord(this);
                submitter.InitNew();
                AddRecord(submitter);
                fHeader.SetTagStringValue(GEDCOMTagType.SUBM, "@" + submitter.XRef + "@");
            }
            return submitter;
        }

        public GEDCOMIndividualRecord CreateIndividual()
        {
            GEDCOMIndividualRecord result = new GEDCOMIndividualRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMFamilyRecord CreateFamily()
        {
            GEDCOMFamilyRecord result = new GEDCOMFamilyRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMNoteRecord CreateNote()
        {
            GEDCOMNoteRecord result = new GEDCOMNoteRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMSourceRecord CreateSource()
        {
            GEDCOMSourceRecord result = new GEDCOMSourceRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMRepositoryRecord CreateRepository()
        {
            GEDCOMRepositoryRecord result = new GEDCOMRepositoryRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMResearchRecord CreateResearch()
        {
            GEDCOMResearchRecord result = new GEDCOMResearchRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMCommunicationRecord CreateCommunication()
        {
            GEDCOMCommunicationRecord result = new GEDCOMCommunicationRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMTaskRecord CreateTask()
        {
            GEDCOMTaskRecord result = new GEDCOMTaskRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMMultimediaRecord CreateMultimedia()
        {
            GEDCOMMultimediaRecord result = new GEDCOMMultimediaRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMLocationRecord CreateLocation()
        {
            GEDCOMLocationRecord result = new GEDCOMLocationRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        public GEDCOMGroupRecord CreateGroup()
        {
            GEDCOMGroupRecord result = new GEDCOMGroupRecord(this);
            result.InitNew();
            result.ChangeDate.ChangeDateTime = DateTime.Now;

            AddRecord(result);
            return result;
        }

        //

        public bool DeleteIndividualRecord(GEDCOMIndividualRecord iRec)
        {
            if (iRec == null) return false;

            iRec.Clear();
            DeleteRecord(iRec);
            return true;
        }

        public bool DeleteFamilyRecord(GEDCOMFamilyRecord famRec)
        {
            if (famRec == null) return false;

            famRec.Clear();
            DeleteRecord(famRec);
            return true;
        }

        public bool DeleteGroupRecord(GEDCOMGroupRecord groupRec)
        {
            if (groupRec == null) return false;

            for (int i = groupRec.Members.Count - 1; i >= 0; i--)
            {
                GEDCOMIndividualRecord member = groupRec.Members[i].Value as GEDCOMIndividualRecord;
                groupRec.RemoveMember(member);
            }

            DeleteRecord(groupRec);
            return true;
        }

        public bool DeleteMediaRecord(GEDCOMMultimediaRecord mRec)
        {
            if (mRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                for (int j = rec.MultimediaLinks.Count - 1; j >= 0; j--)
                {
                    if (rec.MultimediaLinks[j].Value == mRec)
                    {
                        rec.MultimediaLinks.DeleteAt(j);
                    }
                }
            }

            DeleteRecord(mRec);
            return true;
        }

        public bool DeleteNoteRecord(GEDCOMNoteRecord nRec)
        {
            if (nRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                for (int j = rec.Notes.Count - 1; j >= 0; j--)
                {
                    if (rec.Notes[j].Value == nRec)
                        rec.Notes.DeleteAt(j);
                }
            }

            DeleteRecord(nRec);
            return true;
        }

        public bool DeleteRepositoryRecord(GEDCOMRepositoryRecord repRec)
        {
            if (repRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                if (rec.RecordType == GEDCOMRecordType.rtSource)
                {
                    GEDCOMSourceRecord srcRec = (GEDCOMSourceRecord) rec;
                    for (int j = srcRec.RepositoryCitations.Count - 1; j >= 0; j--)
                    {
                        if (srcRec.RepositoryCitations[j].Value == repRec)
                        {
                            srcRec.RepositoryCitations.Delete(srcRec.RepositoryCitations[j]);
                        }
                    }
                }
            }

            DeleteRecord(repRec);
            return true;
        }

        public bool DeleteResearchRecord(GEDCOMResearchRecord resRec)
        {
            if (resRec == null) return false;

            DeleteRecord(resRec);
            return true;
        }

        public bool DeleteSourceRecord(GEDCOMSourceRecord srcRec)
        {
            if (srcRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                for (int j = rec.SourceCitations.Count - 1; j >= 0; j--)
                {
                    if (rec.SourceCitations[j].Value == srcRec)
                    {
                        rec.SourceCitations.DeleteAt(j);
                    }
                }
            }

            DeleteRecord(srcRec);
            return true;
        }

        public bool DeleteTaskRecord(GEDCOMTaskRecord taskRec)
        {
            if (taskRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                if (rec.RecordType == GEDCOMRecordType.rtResearch)
                {
                    GEDCOMResearchRecord resRec = (GEDCOMResearchRecord) rec;
                    for (int j = resRec.Tasks.Count - 1; j >= 0; j--)
                    {
                        if (resRec.Tasks[j].Value == taskRec)
                        {
                            resRec.Tasks.DeleteAt(j);
                        }
                    }
                }
            }

            DeleteRecord(taskRec);
            return true;
        }

        public bool DeleteCommunicationRecord(GEDCOMCommunicationRecord commRec)
        {
            if (commRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++)
            {
                GEDCOMRecord rec = this[i];
                if (rec.RecordType == GEDCOMRecordType.rtResearch)
                {
                    GEDCOMResearchRecord resRec = (GEDCOMResearchRecord) rec;
                    for (int j = resRec.Communications.Count - 1; j >= 0; j--)
                    {
                        if (resRec.Communications[j].Value == commRec)
                        {
                            resRec.Communications.DeleteAt(j);
                        }
                    }
                }
            }

            DeleteRecord(commRec);
            return true;
        }

        public bool DeleteLocationRecord(GEDCOMLocationRecord locRec)
        {
            if (locRec == null) return false;

            int num = fRecords.Count;
            for (int i = 0; i < num; i++) {
                var evsRec = this[i] as GEDCOMRecordWithEvents;
                if (evsRec != null) {
                    for (int j = evsRec.Events.Count - 1; j >= 0; j--) {
                        GEDCOMCustomEvent ev = evsRec.Events[j];

                        if (ev.Place.Location.Value == locRec) {
                            ev.Place.DeleteTag(GEDCOMTagType._LOC);
                        }
                    }
                }
            }

            DeleteRecord(locRec);
            return true;
        }

        #endregion

        #region Utilities

        #endregion

        #region Updating

        public bool IsUpdated()
        {
            return (fUpdateCount != 0);
        }

        public void BeginUpdate()
        {
            if (fUpdateCount == 0)
            {
                SetUpdateState(true);
            }
            fUpdateCount++;
        }

        public void EndUpdate()
        {
            fUpdateCount--;
            if (fUpdateCount == 0)
            {
                SetUpdateState(false);
            }
        }

        private void SetUpdateState(bool updating)
        {
            if (updating)
            {
                Changing();
            }
            else
            {
                Changed();
            }
        }

        private void Changed()
        {
            if (fUpdateCount == 0 && fOnChange != null)
            {
                fOnChange(this, new EventArgs());
            }
        }

        private void Changing()
        {
            if (fUpdateCount == 0 && fOnChanging != null)
            {
                fOnChanging(this, new EventArgs());
            }
        }

        #endregion
    }
}
