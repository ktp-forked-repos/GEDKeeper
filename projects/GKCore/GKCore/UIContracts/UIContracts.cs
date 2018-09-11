﻿/*
 *  "GEDKeeper", the personal genealogical database editor.
 *  Copyright (C) 2009-2018 by Sergey V. Zhdanovskih.
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

using System.Collections.Generic;
using System.IO;
using BSLib;
using GKCommon.GEDCOM;
using GKCore.Charts;
using GKCore.Controllers;
using GKCore.Interfaces;
using GKCore.Lists;
using GKCore.Maps;
using GKCore.Stats;
using GKCore.Types;

namespace GKCore.UIContracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMapBrowser
    {
        bool ShowPoints { get; set; }
        bool ShowLines { get; set; }
        ExtList<GeoPoint> MapPoints { get; }

        int AddPoint(double latitude, double longitude, string hint);
        void ClearPoints();
        void DeletePoint(int index);
        void BeginUpdate();
        void EndUpdate();
        void InitMap();
        void RefreshPoints();
        void SaveSnapshot(string fileName);
        void SetCenter(double latitude, double longitude, int scale);
        void ZoomToBounds();
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IMenuItem
    {
        bool Checked { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface ITextControl
    {
        void AppendText(string text);
        void Clear();
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IPortraitControl
    {
        int Height { get; set; }
        int Width { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IStatusLines
    {
        string this[int index] { get; set; }
    }


    /// <summary>
    /// 
    /// </summary>
    public interface IStatusForm
    {
        IStatusLines StatusLines { get; }
    }


    public enum ChartStyle
    {
        Bar,
        Point,
        ClusterBar
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IGraphControl
    {
        void Clear();
        void PrepareArray(string title, string xAxis, string yAxis, ChartStyle style, bool excludeUnknowns, List<StatsItem> vals);
    }


    /// <summary>
    /// The interface of the class for working with WinForms dialogs.
    /// </summary>
    public interface IStdDialogs
    {
        IFont SelectFont(IFont font);
        string GetOpenFile(string title, string context, string filter,
                           int filterIndex, string defaultExt);
        string GetSaveFile(string filter);
        string GetSaveFile(string title, string context, string filter, int filterIndex, string defaultExt,
                           string suggestedFileName, bool overwritePrompt = true);

        void ShowMessage(string msg);
        void ShowError(string msg);
        bool ShowQuestionYN(string msg);
        void ShowWarning(string msg);

        bool GetInput(string prompt, ref string value);
        bool GetPassword(string prompt, ref string value);
    }


    public interface IView
    {
    }

    public interface IAddressEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMAddress Address { get; set; }

        ITextBoxHandler Country { get; }
        ITextBoxHandler State { get; }
        ITextBoxHandler City { get; }
        ITextBoxHandler PostalCode { get; }
        ITextBoxHandler AddressLine { get; }

        ISheetList PhonesList { get; }
        ISheetList MailsList { get; }
        ISheetList WebsList { get; }
    }

    public interface IAssociationEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMAssociation Association { get; set; }

        ITextBoxHandler Person { get; }
        IComboBoxHandler Relation { get; }
    }

    public interface ICommunicationEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMCommunicationRecord Communication { get; set; }

        ITextBoxHandler Corresponder { get; }
        IComboBoxHandler CorrType { get; }
        ITextBoxHandler Date { get; }
        IComboBoxHandler Dir { get; }
        ITextBoxHandler Name { get; }

        ISheetList NotesList { get; }
        ISheetList MediaList { get; }
    }

    public interface IEventEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMCustomEvent Event { get; set; }

        IComboBoxHandler EventType { get; }
        IComboBoxHandler EventDateType { get; }

        ICheckBoxHandler Date1BC { get; }
        ICheckBoxHandler Date2BC { get; }

        IComboBoxHandler Date1Calendar { get; }
        IComboBoxHandler Date2Calendar { get; }

        ITextBoxHandler Date1 { get; }
        ITextBoxHandler Date2 { get; }

        IComboBoxHandler Attribute { get; }
        ITextBoxHandler Place { get; }
        ITextBoxHandler EventName { get; }
        ITextBoxHandler Cause { get; }
        ITextBoxHandler Agency { get; }

        ISheetList NotesList { get; }
        ISheetList MediaList { get; }
        ISheetList SourcesList { get; }

        void SetLocationMode(bool active);
    }

    public interface IFamilyEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMFamilyRecord Family { get; set; }

        void SetTarget(TargetMode targetType, GEDCOMIndividualRecord target);
        void LockEditor(bool locked);
        void SetHusband(string value);
        void SetWife(string value);

        ISheetList NotesList { get; }
        ISheetList MediaList { get; }
        ISheetList SourcesList { get; }
        ISheetList ChildrenList { get; }
        ISheetList EventsList { get; }

        IComboBoxHandler MarriageStatus { get; }
        IComboBoxHandler Restriction { get; }
    }

    public interface IGroupEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMGroupRecord Group { get; set; }

        ITextBoxHandler Name { get; }

        ISheetList NotesList { get; }
        ISheetList MediaList { get; }
        ISheetList MembersList { get; }
    }

    public interface ILanguageEditDlg : ICommonDialog
    {
        GEDCOMLanguageID LanguageID { get; set; }
    }

    public interface ILanguageSelectDlg : ICommonDialog
    {
        int SelectedLanguage { get; set; }
    }

    public interface ILocationEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMLocationRecord LocationRecord { get; set; }

        IMapBrowser MapBrowser { get; }
        ISheetList MediaList { get; }
        ISheetList NotesList { get; }
        IListView GeoCoordsList { get; }
        ITextBoxHandler Name { get; }
        ITextBoxHandler Latitude { get; }
        ITextBoxHandler Longitude { get; }
    }

    public interface IMediaEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMMultimediaRecord MediaRec { get; set; }

        ISheetList NotesList { get; }
        ISheetList SourcesList { get; }

        IComboBoxHandler MediaType { get; }
        IComboBoxHandler StoreType { get; }
        ITextBoxHandler Name { get; }
        ITextBoxHandler File { get; }
        IButtonHandler FileSelectButton { get; }
    }

    public interface INameEditDlg : ICommonDialog, IView
    {
        NameEntry IName { get; set; }
    }

    public interface INoteEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMNoteRecord NoteRecord { get; set; }

        ITextBoxHandler Note { get; }
    }

    public interface INoteEditDlgEx : ICommonDialog, IBaseEditor
    {
        GEDCOMNoteRecord NoteRecord { get; set; }
    }

    public interface IOrganizerWin : IView
    {
        ISheetList AdrList { get; }
        ISheetList PhonesList { get; }
        ISheetList MailsList { get; }
        ISheetList WebsList { get; }
    }

    public interface IPersonalNameEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMPersonalName PersonalName { get; set; }

        ILabelHandler SurnameLabel { get; }
        ITextBoxHandler Surname { get; }
        ITextBoxHandler Name { get; }
        ITextBoxHandler Patronymic { get; }
        IComboBoxHandler NameType { get; }
        ITextBoxHandler NamePrefix { get; }
        ITextBoxHandler Nickname { get; }
        ITextBoxHandler SurnamePrefix { get; }
        ITextBoxHandler NameSuffix { get; }
        ITextBoxHandler MarriedSurname { get; }
        IComboBoxHandler Language { get; }
    }

    public interface IPersonEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMIndividualRecord Person { get; set; }
        GEDCOMIndividualRecord Target { get; set; }
        TargetMode TargetMode { get; set; }
        void SetNeedSex(GEDCOMSex needSex);

        ISheetList EventsList { get; }
        ISheetList SpousesList { get; }
        ISheetList AssociationsList { get; }
        ISheetList GroupsList { get; }
        ISheetList UserRefList { get; }
        ISheetList NamesList { get; }
        ISheetList NotesList { get; }
        ISheetList MediaList { get; }
        ISheetList SourcesList { get; }

        IPortraitControl Portrait { get; }
        ITextBoxHandler Father { get; }
        ITextBoxHandler Mother { get; }
        ITextBoxHandler Surname { get; }
        ITextBoxHandler Name { get; }
        IComboBoxHandler Patronymic { get; }
        ITextBoxHandler NamePrefix { get; }
        ITextBoxHandler Nickname { get; }
        ITextBoxHandler SurnamePrefix { get; }
        ITextBoxHandler NameSuffix { get; }
        ITextBoxHandler MarriedSurname { get; }

        ILabelHandler SurnameLabel { get; }
        IComboBoxHandler RestrictionCombo { get; }
        IComboBoxHandler SexCombo { get; }

        ICheckBoxHandler Patriarch { get; }
        ICheckBoxHandler Bookmark { get; }

        void SetParentsAvl(bool avail, bool locked);
        void SetFatherAvl(bool avail, bool locked);
        void SetMotherAvl(bool avail, bool locked);
        //void UpdatePortrait(bool totalUpdate);

        void SetPortrait(IImage portrait);
        void SetPortraitAvl(bool avail, bool locked);
    }

    public interface IRepositoryEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMRepositoryRecord Repository { get; set; }

        ISheetList NotesList { get; }
        ITextBoxHandler Name { get; }
    }

    public interface IResearchEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMResearchRecord Research { get; set; }

        ISheetList TasksList { get; }
        ISheetList CommunicationsList { get; }
        ISheetList GroupsList { get; }
        ISheetList NotesList { get; }

        ITextBoxHandler Name { get; }
        IComboBoxHandler Priority { get; }
        IComboBoxHandler Status { get; }
        ITextBoxHandler StartDate { get; }
        ITextBoxHandler StopDate { get; }
        INumericBoxHandler Percent { get; }
    }

    public interface ISexCheckDlg : ICommonDialog
    {
        string IndividualName { get; set; }
        GEDCOMSex Sex { get; set; }
    }

    public interface ISourceCitEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMSourceCitation SourceCitation { get; set; }

        ITextBoxHandler Page { get; }
        IComboBoxHandler Certainty { get; }
        IComboBoxHandler Source { get; }
    }

    public interface ISourceEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMSourceRecord SourceRecord { get; set; }

        ISheetList NotesList { get; }
        ISheetList MediaList { get; }
        ISheetList RepositoriesList { get; }

        ITextBoxHandler ShortTitle { get; }
        ITextBoxHandler Author { get; }
        ITextBoxHandler Title { get; }
        ITextBoxHandler Publication { get; }
        ITextBoxHandler Text { get; }
    }

    public interface ITaskEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMTaskRecord Task { get; set; }

        ISheetList NotesList { get; }
        IComboBoxHandler Priority { get; }
        ITextBoxHandler StartDate { get; }
        ITextBoxHandler StopDate { get; }
        IComboBoxHandler GoalType { get; }
        ITextBoxHandler Goal { get; }
        IButtonHandler GoalSelect { get; }
    }

    public interface IUserRefEditDlg : ICommonDialog, IBaseEditor, IView
    {
        GEDCOMUserReference UserRef { get; set; }

        IComboBoxHandler Ref { get; }
        IComboBoxHandler RefType { get; }
    }

    public interface IFilePropertiesDlg : ICommonDialog, IBaseEditor, IView
    {
        IListView RecordStats { get; }

        ITextBoxHandler Language { get; }
        ITextBoxHandler Name { get; }
        ITextBoxHandler Address { get; }
        ITextBoxHandler Tel { get; }
    }

    public interface IPortraitSelectDlg : ICommonDialog, IBaseEditor
    {
        GEDCOMMultimediaLink MultimediaLink { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IRecordSelectDialog : ICommonDialog, IBaseEditor, IView
    {
        string FastFilter { get; set; }
        string Filter { get; set; }
        GEDCOMRecordType RecType { get; set; }
        TargetMode TargetMode { get; set; }
        GEDCOMIndividualRecord Target { get; set; }
        GEDCOMSex NeedSex { get; set; }
        GEDCOMRecord ResultRecord { get; set; }
    }

    public interface IDayTipsDlg : ICommonDialog
    {
        bool ShowTipsChecked { get; set; }

        void Init(string caption, bool showTipsChecked, StringList tips);
    }

    public interface ITreeFilterDlg : ICommonDialog, IView
    {
    }

    public interface ICircleChartWin : IView
    {
        string Caption { get; set; }
        ICircleChart CircleChart { get; }
    }

    public interface ICommonFilterDlg : ICommonDialog, IView
    {
    }

    public interface IMapsViewerWin : IView
    {
        IMapBrowser MapBrowser { get; }
        object TreeRoot { get; }
        IComboBoxHandler PersonsCombo { get; }
        ITreeViewHandler PlacesTree { get; }
        IButtonHandler SelectPlacesBtn { get; }

        void AddPlace(GEDCOMPlace place, GEDCOMCustomEvent placeEvent);
    }

    public interface IPersonsFilterDlg : ICommonDialog, IView
    {
    }

    public interface IStatisticsWin : IView
    {
        IGraphControl Graph { get; }
        IListView ListStats { get; }
        IListView Summary { get; }
        IComboBoxHandler StatsType { get; }
    }

    public interface IMediaViewerWin : IView
    {
        string Caption { get; set; }
        void SetViewImage(IImage img, GEDCOMFileReferenceWithTitle fileRef);
        void SetViewMedia(string mediaFile);
        void SetViewText(string text);
        void SetViewRTF(string text);
        void SetViewHTML(Stream stm);
        void DisposeViewControl();
    }

    public interface ISlideshowWin : IView, IStatusForm
    {
        void SetImage(IImage image);
    }

    public interface ITreeChartWin : IView
    {
        string Caption { get; set; }
        ITreeChartBox TreeBox { get; }
        TreeChartKind ChartKind { get; set; }
    }

    public interface IRelationshipCalculatorDlg : ICommonDialog, IView
    {
        ILabelHandler Label1 { get; }
        ILabelHandler Label2 { get; }
        ITextBoxHandler Person1 { get; }
        ITextBoxHandler Person2 { get; }
        ITextBoxHandler Result { get; }
    }
}
