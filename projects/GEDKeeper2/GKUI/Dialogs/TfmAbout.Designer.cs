﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace GKUI.Dialogs
{
	partial class TfmAbout
	{
		private System.Windows.Forms.Label LabelProduct;
		private System.Windows.Forms.Label LabelVersion;
		private System.Windows.Forms.Label LabelCopyright;
		private System.Windows.Forms.Label LabelCite;
		private System.Windows.Forms.Label LabelMail;
		private System.Windows.Forms.Button btnClose;

		private void InitializeComponent()
		{
			this.LabelProduct = new System.Windows.Forms.Label();
			this.LabelVersion = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.LabelCopyright = new System.Windows.Forms.Label();
			this.LabelMail = new System.Windows.Forms.Label();
			this.LabelCite = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LabelProduct
			// 
			this.LabelProduct.AutoSize = true;
			this.LabelProduct.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelProduct.Location = new System.Drawing.Point(10, 7);
			this.LabelProduct.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabelProduct.Name = "LabelProduct";
			this.LabelProduct.Size = new System.Drawing.Size(28, 31);
			this.LabelProduct.TabIndex = 0;
			this.LabelProduct.Text = "?";
			this.LabelProduct.Click += new System.EventHandler(this.LabelProductClick);
			// 
			// LabelVersion
			// 
			this.LabelVersion.AutoSize = true;
			this.LabelVersion.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelVersion.Location = new System.Drawing.Point(12, 46);
			this.LabelVersion.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabelVersion.Name = "LabelVersion";
			this.LabelVersion.Size = new System.Drawing.Size(58, 17);
			this.LabelVersion.TabIndex = 1;
			this.LabelVersion.Text = "Version";
			// 
			// btnClose
			// 
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Image = global::GKResources.iBtnCancel;
			this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.btnClose.Location = new System.Drawing.Point(258, 224);
			this.btnClose.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(91, 24);
			this.btnClose.TabIndex = 0;
			this.btnClose.Text = "Закрыть";
			this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LabelCopyright
			// 
			this.LabelCopyright.AutoSize = true;
			this.LabelCopyright.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelCopyright.Location = new System.Drawing.Point(12, 73);
			this.LabelCopyright.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabelCopyright.Name = "LabelCopyright";
			this.LabelCopyright.Size = new System.Drawing.Size(226, 17);
			this.LabelCopyright.TabIndex = 1;
			this.LabelCopyright.Text = "Copyright © Serg V. Zhdanovskih";
			// 
			// LabelMail
			// 
			this.LabelMail.AutoSize = true;
			this.LabelMail.Cursor = System.Windows.Forms.Cursors.Hand;
			this.LabelMail.Font = new System.Drawing.Font("Times New Roman", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelMail.ForeColor = System.Drawing.Color.Blue;
			this.LabelMail.Location = new System.Drawing.Point(13, 195);
			this.LabelMail.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabelMail.Name = "LabelMail";
			this.LabelMail.Size = new System.Drawing.Size(131, 13);
			this.LabelMail.TabIndex = 2;
			this.LabelMail.Text = "http://vk.com/gedkeeper";
			this.LabelMail.Click += new System.EventHandler(this.LabelMail_Click);
			// 
			// LabelCite
			// 
			this.LabelCite.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.LabelCite.Location = new System.Drawing.Point(12, 105);
			this.LabelCite.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.LabelCite.Name = "LabelCite";
			this.LabelCite.Size = new System.Drawing.Size(338, 78);
			this.LabelCite.TabIndex = 3;
			this.LabelCite.Text = "«История рода - это есть история Отечества» «Неуважение к предкам - есть первый п" +
			"ризнак дикости и безнравственности» (Александр Сергеевич Пушкин)";
			this.LabelCite.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// TfmAbout
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(364, 262);
			this.Controls.Add(this.LabelProduct);
			this.Controls.Add(this.LabelVersion);
			this.Controls.Add(this.LabelCopyright);
			this.Controls.Add(this.LabelMail);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.LabelCite);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TfmAbout";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "О программе";
			this.ResumeLayout(false);
			this.PerformLayout();
		}
	}
}