object fmStats: TfmStats
  Left = 373
  Top = 156
  BorderStyle = bsDialog
  Caption = #1057#1090#1072#1090#1080#1089#1090#1080#1082#1072
  ClientHeight = 503
  ClientWidth = 641
  Color = clBtnFace
  Font.Charset = RUSSIAN_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'Tahoma'
  Font.Style = []
  KeyPreview = True
  OldCreateOrder = False
  Position = poScreenCenter
  OnCreate = FormCreate
  OnKeyDown = FormKeyDown
  OnShow = FormShow
  PixelsPerInch = 96
  TextHeight = 13
  object GroupBox1: TGroupBox
    Left = 0
    Top = 0
    Width = 641
    Height = 169
    Align = alTop
    Caption = #1057#1074#1086#1076#1082#1072
    TabOrder = 0
    object ListCommon: TBSListView
      Left = 2
      Top = 15
      Width = 637
      Height = 152
      Align = alClient
      Columns = <
        item
          Caption = #1055#1072#1088#1072#1084#1077#1090#1088
          Width = 300
        end
        item
          Caption = #1042#1089#1077#1075#1086
          Width = 100
        end
        item
          Caption = #1052#1091#1078#1095#1080#1085#1099
          Width = 100
        end
        item
          Caption = #1046#1077#1085#1097#1080#1085#1099
          Width = 100
        end>
      ReadOnly = True
      RowSelect = True
      TabOrder = 0
      ViewStyle = vsReport
      SortColumn = 0
      SortDirection = sdAscending
    end
  end
  object Panel1: TPanel
    Left = 0
    Top = 169
    Width = 641
    Height = 334
    Align = alClient
    BevelInner = bvRaised
    BevelOuter = bvLowered
    TabOrder = 1
    object ToolBar1: TToolBar
      Left = 2
      Top = 2
      Width = 637
      Height = 23
      AutoSize = True
      ButtonHeight = 21
      Caption = 'ToolBar1'
      EdgeBorders = []
      TabOrder = 0
      object ToolButton1: TToolButton
        Left = 0
        Top = 2
        Width = 8
        Caption = 'ToolButton1'
        Style = tbsSeparator
      end
      object cbType: TComboBox
        Left = 8
        Top = 2
        Width = 233
        Height = 21
        Style = csDropDownList
        DropDownCount = 16
        ItemHeight = 13
        TabOrder = 0
        OnChange = cbTypeChange
      end
    end
    object ListStats: TBSListView
      Left = 2
      Top = 25
      Width = 637
      Height = 307
      Align = alClient
      Columns = <
        item
          Caption = 'X'
          Width = 300
        end
        item
          Caption = #1047#1085#1072#1095#1077#1085#1080#1077
          Width = 100
        end>
      ReadOnly = True
      RowSelect = True
      SortType = stText
      TabOrder = 1
      ViewStyle = vsReport
      SortColumn = 0
      SortDirection = sdAscending
    end
  end
end
