unit GKPersonNew;

{$I GEDKeeper.inc}

interface

uses
  Windows, SysUtils, Classes, Graphics, Controls, Forms, StdCtrls, Buttons,
  GedCom551, GKCommon;

type
  TfmPersonNew = class(TForm)
    Label1: TLabel;
    EditFamily: TEdit;
    Label2: TLabel;
    EditName: TEdit;
    Label3: TLabel;
    EditPatronymic: TComboBox;
    Label4: TLabel;
    EditSex: TComboBox;
    btnAccept: TBitBtn;
    btnCancel: TBitBtn;
    procedure FormCreate(Sender: TObject);
  private
    FTarget: TGEDCOMIndividualRecord;
    FTargetMode: TTargetMode;
    procedure SetTarget(const Value: TGEDCOMIndividualRecord);
  public
    property Target: TGEDCOMIndividualRecord read FTarget write SetTarget;
    property TargetMode: TTargetMode read FTargetMode write FTargetMode;
  end;

implementation

uses GKMain;

{$R *.dfm}

procedure TfmPersonNew.FormCreate(Sender: TObject);
var
  sx: TGEDCOMSex;
begin
  for sx := Low(TGEDCOMSex) to High(TGEDCOMSex) do EditSex.Items.Add(Sex[sx]);
end;

procedure TfmPersonNew.SetTarget(const Value: TGEDCOMIndividualRecord);
var
  iFamily, iName, iPatronymic: string;
  names: TNamesTable;
begin
  FTarget := Value;

  names := fmGEDKeeper.FNamesTable;

  if (FTarget <> nil) then begin
    GetNameParts(FTarget, iFamily, iName, iPatronymic);

    EditFamily.Text := iFamily;

    if (FTargetMode = tmAncestor) then begin
      if (TGEDCOMSex(EditSex.ItemIndex) = svMale)
      then EditName.Text := names.GetNameByPatronymic(iPatronymic, svMale);
    end else begin
      EditPatronymic.Items.Add(names.GetPatronymicByName(iName, svMale));
      EditPatronymic.Items.Add(names.GetPatronymicByName(iName, svFemale));
    end;
  end;
end;

end.
