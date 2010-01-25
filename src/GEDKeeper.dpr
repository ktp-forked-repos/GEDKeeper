program GEDKeeper;

{$I GEDKeeper.inc}

uses
  Forms,
  GKMain in 'GKMain.pas' {fmGEDKeeper},
  GKCommon in 'GKCommon.pas',
  GedCom551 in 'GedCom551.pas',
  GKPersonNew in 'GKPersonNew.pas' {fmPersonNew},
  GKRecordSelect in 'GKRecordSelect.pas' {fmRecordSelect},
  GKEventEdit in 'GKEventEdit.pas' {fmEventEdit},
  GKNoteEdit in 'GKNoteEdit.pas' {fmNoteEdit},
  GKSourceEdit in 'GKSourceEdit.pas' {fmSourceEdit},
  GKChart in 'GKChart.pas' {fmChart},
  GKAbout in 'GKAbout.pas' {fmAbout},
  GKChartCore in 'GKChartCore.pas',
  GKFileProperties in 'GKFileProperties.pas' {fmFileProperties},
  GKTreeTools in 'GKTreeTools.pas' {fmTreeTools},
  GKStats in 'GKStats.pas' {fmStats},
  GKNameEdit in 'GKNameEdit.pas' {fmNameEdit},
  GKPersonEdit in 'GKPersonEdit.pas' {fmPersonEdit},
  GKExport in 'GKExport.pas',
  GKOptions in 'GKOptions.pas' {fmOptions},
  GKFamilyEdit in 'GKFamilyEdit.pas' {fmFamilyEdit},
  GKAssociationEdit in 'GKAssociationEdit.pas' {fmAssociationEdit},
  GKFilter in 'GKFilter.pas' {fmFilter},
  GKGroupEdit in 'GKGroupEdit.pas' {fmGroupEdit},
  GKPersonScan in 'GKPersonScan.pas' {fmPersonScan},
  GKLifeSettings in 'GKLifeSettings.pas' {fmLifeSettings},
  GKLifeMain in 'GKLifeMain.pas' {fmLife},
  GKMaps in 'GKMaps.pas' {fmMaps},
  GoogleMapFrames in 'ext\GoogleMapFrames.pas' {GoogleMapFrame: TFrame},
  BaseMapFrames in 'ext\BaseMapFrames.pas' {BaseMapFrame: TFrame},
  GKProgress in 'GKProgress.pas' {fmProgress},
  GKAddressEdit in 'GKAddressEdit.pas' {fmAddressEdit},
  GKImport in 'GKImport.pas',
  GKSourceCitEdit in 'GKSourceCitEdit.pas' {fmSourceCitEdit};

{$R *.res}

begin
  Application.Initialize;
  Application.Title := 'GEDKeeper';
  Application.CreateForm(TfmGEDKeeper, fmGEDKeeper);
  Application.CreateForm(TfmStats, fmStats);
  Application.Run;
end.
