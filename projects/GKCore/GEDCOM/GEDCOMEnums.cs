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

namespace GKCommon.GEDCOM
{
    public static class GEDCOMTagType
    {
        public const string ADR1 = "ADR1";
        public const string ADR2 = "ADR2";
        public const string ADR3 = "ADR3";
        public const string AFN = "AFN"; // AncestralFileNumber
        public const string AGNC = "AGNC";
        public const string ALIA = "ALIA";
        public const string ASSO = "ASSO";
        public const string CHAN = "CHAN";
        public const string CITY = "CITY";
        public const string CTRY = "CTRY";
        public const string DATA = "DATA";
        public const string EVEN = "EVEN";
        public const string FAMC = "FAMC";
        public const string INDI = "INDI"; // IndividualRecord
        public const string LANG = "LANG";
        public const string LATI = "LATI";
        public const string LONG = "LONG";
        public const string MAP = "MAP";
        public const string NOTE = "NOTE";
        public const string OBJE = "OBJE";
        public const string PAGE = "PAGE";
        public const string PEDI = "PEDI";
        public const string PLAC = "PLAC";
        public const string POST = "POST";
        public const string QUAY = "QUAY";
        public const string REFN = "REFN";
        public const string RELA = "RELA";
        public const string REPO = "REPO";
        public const string RESN = "RESN"; // Restriction
        public const string RFN = "RFN"; // PermanentRecordFileNumber
        public const string RIN = "RIN"; // AutomatedRecordID
        public const string SOUR = "SOUR";
        public const string STAE = "STAE";

        // non-standard extended tags (other applications)
        public const string UID = "_UID"; // 

        // non-standard extended tags (GEDKeeper)
        public const string BOOKMARK = "_BOOKMARK";
        public const string FOLDER = "_FOLDER";
        public const string GROUP = "_GROUP";
        public const string MEMBER = "_MEMBER";
        public const string PATRIARCH = "_PATRIARCH";
    }

    public enum GEDCOMRecordType
    {
        rtNone,
        rtIndividual,
        rtFamily,
        rtNote,
        rtMultimedia,
        rtSource,
        rtRepository,
        rtGroup,
        rtResearch,
        rtTask,
        rtCommunication,
        rtLocation,
        rtSubmission,
        rtSubmitter,

        rtLast/* = rtSubmitter*/
    }

    public enum GEDCOMFormat
    {
        gf_Unknown,
        gf_Native,
        gf_GENBOX,
        gf_ALTREE,
        gf_AGES,
        gf_PAF,
        gf_Ahnenblatt,
        gf_Genealogy_RusOld,
        gf_FTB,

        gf_Last = gf_FTB
    }

    public enum GEDCOMCharacterSet
    {
        csASCII,
        csANSEL,
        csUNICODE,
        csUTF8
    }

    public enum GEDCOMState
    {
        osLoading,
        osReady
    }

    public enum GEDCOMRestriction
    {
        rnNone,
        rnLocked,
        rnConfidential,
        rnPrivacy,

        rnLast = rnPrivacy
    }

    public enum GEDCOMSex
    {
        svNone,
        svMale,
        svFemale,
        svUndetermined,

        svLast = svUndetermined
    }

    public enum GEDCOMNameType
    {
        ntNone,
        ntAka,
        ntBirth,
        ntImmigrant,
        ntMaiden,
        ntMarried
    }

    public enum GEDCOMCalendar
    {
        dcGregorian,
        dcJulian,
        dcHebrew,
        dcFrench,
        dcRoman,
        dcIslamic, // GK+ (nonstandard)
        dcUnknown,

        dcLast = dcUnknown
    }

    public enum GEDCOMDateFormat
    {
        dfGEDCOMStd,
        dfSystem
    }

    public enum GEDCOMApproximated
    {
        daExact,
        daAbout,
        daCalculated,
        daEstimated
    }

    public enum GEDCOMRange
    {
        drAfter,
        drBefore,
        drBetween,
        drAnd
    }

    public enum GEDCOMBaptismDateStatus
    {
        bdsNone,
        bdsChild,
        bdsCompleted,
        bdsExcluded,
        bdsPre1970,
        bdsStillborn,
        bdsSubmitted,
        bdsUncleared
    }

    public enum GEDCOMEndowmentDateStatus
    {
        edsNone,
        edsChild,
        edsCompleted,
        edsExcluded,
        edsInfant,
        edsPre1970,
        edsStillborn,
        edsSubmitted,
        edsUncleared
    }

    public enum GEDCOMChildSealingDateStatus
    {
        cdsNone,
        cdsBIC,
        cdsExcluded,
        cdsPre1970,
        cdsStillborn,
        cdsSubmitted,
        cdsUncleared
    }

    public enum GEDCOMChildLinkageStatus
    {
        clNone,
        clChallenged,
        clDisproven,
        clProven
    }

    public enum GEDCOMOrdinanceProcessFlag
    {
        opNone,
        opYes,
        opNo
    }

    public enum GEDCOMPedigreeLinkageType
    {
        plNone,
        plAdopted,
        plBirth,
        plFoster,
        plSealing
    }

    public enum GEDCOMSpouseSealingDateStatus
    {
        sdsNone,
        sdsCanceled,
        sdsCompleted,
        sdsExcluded,
        sdsDNS,
        sdsDNSCAN,
        sdsPre1970,
        sdsSubmitted,
        sdsUncleared
    }

    public enum GEDCOMMediaType
    {
        mtUnknown,

        mtAudio,
        mtBook,
        mtCard,
        mtElectronic,
        mtFiche,
        mtFilm,
        mtMagazine,
        mtManuscript,
        mtMap,
        mtNewspaper,
        mtPhoto,
        mtTombstone,
        mtVideo,

        mtLast = mtVideo
    }

    // TODO: doc/docx/odt, xls/xlsx/ods, ppt/pptx/odp
    public enum GEDCOMMultimediaFormat
    {
        mfNone,

        mfBMP,
        mfGIF,
        mfJPG,
        mfPCX,
        mfTIF,
        mfTGA,
        mfPNG,
        mfRAW,
        mfPSD,

        mfTXT,
        mfRTF,
        mfHTM,
        mfPDF,

        mfWAV,
        mfMP3,
        mfWMA,
        mfMKA,

        mfAVI,
        mfMPG,
        mfWMV,
        mfMP4,
        mfOGV,
        mfMKV,
        mfMOV,

        mfOLE,
        mfUnknown
    }

    public enum GKCommunicationDir
    {
        cdFrom,
        cdTo
    }

    public enum GKCommunicationType
    {
        ctCall,
        ctEMail,
        ctFax,
        ctLetter,
        ctTape,
        ctVisit,

        ctLast = ctVisit
    }

    public enum GKGoalType
    {
        gtIndividual,
        gtFamily,
        gtSource,
        gtOther
    }

    public enum GKResearchPriority
    {
        rpNone,
        rpLow,
        rpNormal,
        rpHigh,
        rpTop
    }

    public enum GKResearchStatus
    {
        rsDefined,
        rsInProgress,
        rsOnHold,
        rsProblems,
        rsCompleted,
        rsWithdrawn
    }
}
