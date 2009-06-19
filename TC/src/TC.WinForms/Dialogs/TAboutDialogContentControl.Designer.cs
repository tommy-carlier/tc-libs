namespace TC.WinForms.Dialogs
{
	partial class TAboutDialogContentControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			TC.WinForms.Controls.TSystemIconBox SideImageControl;
			System.Windows.Forms.TableLayoutPanel PanelLayout;
			TC.WinForms.Commands.ApplicationCommand VisitWebsiteCommand;
			TC.WinForms.Dialogs.DialogResultButton dialogResultButton1 = new TC.WinForms.Dialogs.DialogResultButton();
			this.LabelTitle = new TC.WinForms.Controls.TLabel();
			this.LabelCopyright = new TC.WinForms.Controls.TLabel();
			this.LabelVersion = new TC.WinForms.Controls.TLabel();
			this.Hyperlink = new TC.WinForms.Controls.TCommandHyperlink();
			SideImageControl = new TC.WinForms.Controls.TSystemIconBox();
			PanelLayout = new System.Windows.Forms.TableLayoutPanel();
			VisitWebsiteCommand = new TC.WinForms.Commands.ApplicationCommand();
			PanelLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// SideImageControl
			// 
			SideImageControl.SystemIcon = TC.WinForms.SystemIcon.FormIcon;
			SideImageControl.Location = new System.Drawing.Point(8, 8);
			SideImageControl.Name = "SideImageControl";
			SideImageControl.Size = new System.Drawing.Size(42, 98);
			SideImageControl.Dock = System.Windows.Forms.DockStyle.Left;
			SideImageControl.TabIndex = 0;
			SideImageControl.TabStop = false;
			// 
			// PanelLayout
			// 
			PanelLayout.AutoSize = true;
			PanelLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			PanelLayout.ColumnCount = 2;
			PanelLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
			PanelLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			PanelLayout.Controls.Add(this.LabelTitle, 0, 0);
			PanelLayout.Controls.Add(this.LabelCopyright, 1, 2);
			PanelLayout.Controls.Add(this.LabelVersion, 1, 1);
			PanelLayout.Controls.Add(this.Hyperlink, 1, 3);
			PanelLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			PanelLayout.Location = new System.Drawing.Point(50, 8);
			PanelLayout.Name = "PanelLayout";
			PanelLayout.Padding = new System.Windows.Forms.Padding(5);
			PanelLayout.RowCount = 4;
			PanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			PanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			PanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			PanelLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			PanelLayout.Size = new System.Drawing.Size(122, 98);
			PanelLayout.TabIndex = 4;
			// 
			// LabelTitle
			// 
			this.LabelTitle.AutoSize = true;
			PanelLayout.SetColumnSpan(this.LabelTitle, 2);
			this.LabelTitle.Location = new System.Drawing.Point(8, 8);
			this.LabelTitle.Margin = new System.Windows.Forms.Padding(3);
			this.LabelTitle.Name = "LabelTitle";
			this.LabelTitle.Size = new System.Drawing.Size(64, 25);
			this.LabelTitle.SystemFont = TC.WinForms.SystemFont.Header;
			this.LabelTitle.TabIndex = 1;
			this.LabelTitle.Text = "{Title}";
			// 
			// LabelCopyright
			// 
			this.LabelCopyright.AutoSize = true;
			this.LabelCopyright.Location = new System.Drawing.Point(23, 58);
			this.LabelCopyright.Margin = new System.Windows.Forms.Padding(3);
			this.LabelCopyright.Name = "LabelCopyright";
			this.LabelCopyright.Size = new System.Drawing.Size(59, 13);
			this.LabelCopyright.TabIndex = 3;
			this.LabelCopyright.Text = "{Copyright}";
			// 
			// LabelVersion
			// 
			this.LabelVersion.AutoSize = true;
			this.LabelVersion.Location = new System.Drawing.Point(23, 39);
			this.LabelVersion.Margin = new System.Windows.Forms.Padding(3);
			this.LabelVersion.Name = "LabelVersion";
			this.LabelVersion.Size = new System.Drawing.Size(91, 13);
			this.LabelVersion.TabIndex = 2;
			this.LabelVersion.Text = "Version: {Version}";
			// 
			// Hyperlink
			// 
			this.Hyperlink.AutoSize = true;
			this.Hyperlink.Command = VisitWebsiteCommand;
			this.Hyperlink.Location = new System.Drawing.Point(23, 77);
			this.Hyperlink.Margin = new System.Windows.Forms.Padding(3);
			this.Hyperlink.Name = "Hyperlink";
			this.Hyperlink.Size = new System.Drawing.Size(37, 13);
			this.Hyperlink.TabIndex = 4;
			this.Hyperlink.TabStop = true;
			this.Hyperlink.Text = "{URL}";
			// 
			// VisitWebsiteCommand
			// 
			VisitWebsiteCommand.Executed += new System.EventHandler(this.HandlerVisitWebsiteCommandExecuted);
			// 
			// TAboutDialogContentControl
			// 
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Commands.Add(VisitWebsiteCommand);
			this.Controls.Add(PanelLayout);
			this.Controls.Add(SideImageControl);
			dialogResultButton1.DialogResult = System.Windows.Forms.DialogResult.OK;
			dialogResultButton1.Text = "OK";
			this.DialogResultButtons.Add(dialogResultButton1);
			this.Name = "TAboutDialogContentControl";
			this.Size = new System.Drawing.Size(180, 114);
			PanelLayout.ResumeLayout(false);
			PanelLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TC.WinForms.Controls.TLabel LabelTitle;
		private TC.WinForms.Controls.TLabel LabelVersion;
		private TC.WinForms.Controls.TLabel LabelCopyright;
		private TC.WinForms.Controls.TCommandHyperlink Hyperlink;
	}
}
