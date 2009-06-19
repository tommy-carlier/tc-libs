namespace TC.WinForms.Dialogs
{
	partial class TMessageDialogContentControl
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
			System.Windows.Forms.TableLayoutPanel TableLayout;
			this.SideImageControl = new TC.WinForms.Controls.TSystemIconBox();
			this.MessageLabel = new TC.WinForms.Controls.TLabel();
			TableLayout = new System.Windows.Forms.TableLayoutPanel();
			TableLayout.SuspendLayout();
			this.SuspendLayout();
			// 
			// TableLayout
			// 
			TableLayout.AutoSize = true;
			TableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			TableLayout.ColumnCount = 2;
			TableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			TableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			TableLayout.Controls.Add(this.SideImageControl, 0, 0);
			TableLayout.Controls.Add(this.MessageLabel, 1, 0);
			TableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			TableLayout.Location = new System.Drawing.Point(8, 8);
			TableLayout.Name = "TableLayout";
			TableLayout.RowCount = 1;
			TableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
			TableLayout.Size = new System.Drawing.Size(84, 38);
			TableLayout.TabIndex = 0;
			// 
			// SideImageControl
			// 
			this.SideImageControl.Location = new System.Drawing.Point(3, 3);
			this.SideImageControl.Name = "SideImageControl";
			this.SideImageControl.Size = new System.Drawing.Size(42, 42);
			this.SideImageControl.TabIndex = 0;
			this.SideImageControl.TabStop = false;
			// 
			// MessageLabel
			// 
			this.MessageLabel.AutoSize = true;
			this.MessageLabel.Location = new System.Drawing.Point(41, 0);
			this.MessageLabel.Name = "MessageLabel";
			this.MessageLabel.Padding = new System.Windows.Forms.Padding(10);
			this.MessageLabel.Size = new System.Drawing.Size(20, 33);
			this.MessageLabel.TabIndex = 0;
			// 
			// TMessageDialogContentControl
			// 
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(TableLayout);
			this.Name = "TMessageDialogContentControl";
			this.Size = new System.Drawing.Size(100, 54);
			TableLayout.ResumeLayout(false);
			TableLayout.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private TC.WinForms.Controls.TSystemIconBox SideImageControl;
		private TC.WinForms.Controls.TLabel MessageLabel;
	}
}
