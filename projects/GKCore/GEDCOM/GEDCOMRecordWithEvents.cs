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
using BSLib.Calendar;

namespace GKCommon.GEDCOM
{
    public abstract class GEDCOMRecordWithEvents : GEDCOMRecord, IGEDCOMRecordWithEvents
    {
        private GEDCOMList<GEDCOMCustomEvent> fEvents;
        private GEDCOMRestriction fRestriction;
        private GEDCOMList<GEDCOMPointer> fSubmittors;


        public GEDCOMList<GEDCOMCustomEvent> Events
        {
            get { return fEvents; }
        }

        public GEDCOMRestriction Restriction
        {
            get { return fRestriction; }
            set { fRestriction = value; }
        }

        public GEDCOMList<GEDCOMPointer> Submittors
        {
            get { return fSubmittors; }
        }


        protected GEDCOMRecordWithEvents(GEDCOMObject owner) : base(owner)
        {
            fEvents = new GEDCOMList<GEDCOMCustomEvent>(this);
            fSubmittors = new GEDCOMList<GEDCOMPointer>(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                fEvents.Dispose();
                fSubmittors.Dispose();
            }
            base.Dispose(disposing);
        }

        public override GEDCOMTag AddTag(string tagName, string tagValue, TagConstructor tagConstructor)
        {
            GEDCOMTag result;

            if (tagName == GEDCOMTagType.RESN) {
                fRestriction = GEDCOMUtils.GetRestrictionVal(tagValue);
                result = null;
            } else if (tagName == GEDCOMTagType.SUBM) {
                result = fSubmittors.Add(new GEDCOMPointer(this, tagName, tagValue));
            } else {
                result = base.AddTag(tagName, tagValue, tagConstructor);
            }

            return result;
        }

        public override void Clear()
        {
            base.Clear();

            fEvents.Clear();
            fRestriction = GEDCOMRestriction.rnNone;
            fSubmittors.Clear();
        }

        public override bool IsEmpty()
        {
            // Restrictions are not checked because they are not important if other fields are empty.
            return base.IsEmpty() && (fEvents.Count == 0) && (fSubmittors.Count == 0);
        }

        public override void Assign(GEDCOMTag source)
        {
            GEDCOMRecordWithEvents sourceRec = source as GEDCOMRecordWithEvents;
            if (sourceRec == null)
                throw new ArgumentException(@"Argument is null or wrong type", "source");

            base.Assign(source);

            foreach (GEDCOMCustomEvent sourceEvent in sourceRec.fEvents) {
                GEDCOMCustomEvent copy = (GEDCOMCustomEvent)Activator.CreateInstance(sourceEvent.GetType(), new object[] { this, "", "" });
                copy.Assign(sourceEvent);
                AddEvent(copy);
            }

            fRestriction = sourceRec.Restriction;
        }

        public override void MoveTo(GEDCOMRecord targetRecord, bool clearDest)
        {
            GEDCOMRecordWithEvents target = targetRecord as GEDCOMRecordWithEvents;
            if (target == null)
                throw new ArgumentException(@"Argument is null or wrong type", "targetRecord");

            base.MoveTo(targetRecord, clearDest);

            while (fEvents.Count > 0) {
                GEDCOMCustomEvent obj = fEvents.Extract(0);
                obj.ResetOwner(target);
                target.AddEvent(obj);
            }

            target.Restriction = fRestriction;

            while (fSubmittors.Count > 0) {
                GEDCOMPointer obj = fSubmittors.Extract(0);
                obj.ResetOwner(target);
                target.Submittors.Add(obj);
            }
        }

        public override void Pack()
        {
            base.Pack();
            fEvents.Pack();
            fSubmittors.Pack();
        }

        public override void ReplaceXRefs(XRefReplacer map)
        {
            base.ReplaceXRefs(map);
            fEvents.ReplaceXRefs(map);
            fSubmittors.ReplaceXRefs(map);
        }

        public override void SaveToStream(StreamWriter stream, int level)
        {
            base.SaveToStream(stream, level);

            level += 1;
            WriteTagLine(stream, level, GEDCOMTagType.RESN, GEDCOMUtils.GetRestrictionStr(fRestriction), true);
            fSubmittors.SaveToStream(stream, level);
        }

        public GEDCOMCustomEvent FindEvent(string eventName)
        {
            GEDCOMCustomEvent result = null;

            int num = fEvents.Count;
            for (int i = 0; i < num; i++) {
                GEDCOMCustomEvent evt = fEvents[i];

                if (evt.Name == eventName) {
                    result = evt;
                    break;
                }
            }

            return result;
        }

        public abstract GEDCOMCustomEvent AddEvent(GEDCOMCustomEvent evt);

        #region Auxiliary

        private static readonly float[] CA_VALUES = new float[] { 0.25f, 0.5f, 0.75f, 1.0f };

        public float GetCertaintyAssessment()
        {
            float result = 0;
            float wsum = 0;

            int num1 = fEvents.Count;
            for (int i = 0; i < num1; i++) {
                GEDCOMCustomEvent evt = fEvents[i];

                int num2 = evt.SourceCitations.Count;
                for (int k = 0; k < num2; k++) {
                    GEDCOMSourceCitation cit = evt.SourceCitations[k];

                    int ca = GEDCOMUtils.GetValidCertaintyAssessment(cit.CertaintyAssessment);
                    int weight = (ca + 1);

                    result += (CA_VALUES[ca] * weight);
                    wsum += weight;
                }
            }

            int num3 = SourceCitations.Count;
            for (int i = 0; i < num3; i++) {
                GEDCOMSourceCitation cit = SourceCitations[i];

                int ca = GEDCOMUtils.GetValidCertaintyAssessment(cit.CertaintyAssessment);
                int weight = (ca + 1);

                result += (CA_VALUES[ca] * weight);
                wsum += weight;
            }

            if (wsum != 0.0f) {
                result /= wsum;
            } else {
                result = 0.0f;
            }

            return result;
        }

        public UDN GetUDN(string eventSign)
        {
            GEDCOMCustomEvent evt = FindEvent(eventSign);
            return (evt == null) ? UDN.CreateEmpty() : evt.GetUDN();
        }

        /// <summary>
        /// In the historical chronology of the year 0 does not exist.
        /// Therefore, the digit 0 in the year value can be used as a sign of lack or error.
        /// ChronologicalYear - introduced for the purposes of uniform chronology years in the Gregorian calendar.
        /// Is estimated from -4714 BC to 3268 AD.
        /// </summary>
        /// <returns>chronological year</returns>
        public int GetChronologicalYear(string eventSign)
        {
            GEDCOMCustomEvent evt = FindEvent(eventSign);
            return (evt == null) ? 0 : evt.GetChronologicalYear();
        }

        #endregion
    }
}
