// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tcwinforms
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tcwinforms/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Windows.Forms;

namespace TC.WinForms.Dialogs
{
	/// <summary>Defines a button that sets the <see cref="P:Form.DialogResult"/> of a <see cref="T:TDialog{TContentControl}"/>.</summary>
	public sealed class DialogResultButton
	{
		/// <summary>Initializes a new instance of the <see cref="DialogResultButton"/> class.</summary>
		public DialogResultButton() { }

		/// <summary>Initializes a new instance of the <see cref="DialogResultButton"/> class.</summary>
		/// <param name="dialogResult">The <see cref="T:DialogResult"/> that is set when the button is clicked.</param>
		/// <param name="text">The caption of the button.</param>
		public DialogResultButton(DialogResult dialogResult, string text)
		{
			DialogResult = dialogResult;
			Text = text;
		}

		/// <summary>Gets or sets the <see cref="T:DialogResult"/> that is set when the button is clicked.</summary>
		/// <value>The <see cref="T:DialogResult"/> that is set when the button is clicked.</value>
		[DefaultValue(typeof(DialogResult), "None"), Category("Behavior")]
		[Description("The DialogResult that is set when the button is clicked.")]
		public DialogResult DialogResult { get; set; }

		/// <summary>Gets or sets the caption of the button.</summary>
		/// <value>The caption of the button.</value>
		[DefaultValue(""), Category("Appearance"), Localizable(true), Description("The caption of the button.")]
		public string Text { get; set; }

		/// <summary>Returns a <see cref="T:String"/> that represents the current <see cref="T:DialogResultButton"/>.</summary>
		/// <returns>A <see cref="T:String"/> that represents the current <see cref="T:DialogResultButton"/>.</returns>
		public override string ToString()
		{
			return (Text ?? String.Empty) + " (" + DialogResult.ToString() + ")";
		}

		private static readonly Padding fDefaultPadding = new Padding(12, 0, 12, 0);
		internal Button CreateButton()
		{
			Button lButton = new Button();
			lButton.Text = Text ?? DialogResult.ToString();
			lButton.Click += HandlerButtonClick;
			lButton.Padding = fDefaultPadding;
			lButton.Tag = this;
			return lButton;
		}

		private void HandlerButtonClick(object sender, EventArgs e)
		{
			Button lButton = sender as Button;
			Form lForm = lButton.FindForm();
			if (lForm != null)
			{
				lForm.DialogResult = DialogResult;
				if (!lForm.Modal) lForm.Close();
			}
		}

		/// <summary>Gets a <see cref="T:DialogResultButton"/> that represents <see cref="F:DialogResult.OK"/>.</summary>
		/// <value>The created <see cref="T:DialogResultButton"/>.</value>
		[SuppressMessage(
			"Microsoft.Naming",
			"CA1709:IdentifiersShouldBeCasedCorrectly",
			MessageId = "OK",
			Justification = "OK is not an acceptable term and should not be changed to 'Ok'.")]
		public static DialogResultButton OK
		{
			get { return new DialogResultButton(DialogResult.OK, "OK"); }
		}

		/// <summary>Gets a <see cref="T:DialogResultButton"/> that represents <see cref="F:DialogResult.Cancel"/>.</summary>
		/// <value>The created <see cref="T:DialogResultButton"/>.</value>
		public static DialogResultButton Cancel
		{
			get { return new DialogResultButton(DialogResult.Cancel, "Cancel"); }
		}

		/// <summary>Gets a <see cref="T:DialogResultButton"/> that represents <see cref="F:DialogResult.Yes"/>.</summary>
		/// <value>The created <see cref="T:DialogResultButton"/>.</value>
		public static DialogResultButton Yes
		{
			get { return new DialogResultButton(DialogResult.Yes, "Yes"); }
		}

		/// <summary>Gets a <see cref="T:DialogResultButton"/> that represents <see cref="F:DialogResult.No"/>.</summary>
		/// <value>The created <see cref="T:DialogResultButton"/>.</value>
		public static DialogResultButton No
		{
			get { return new DialogResultButton(DialogResult.No, "No"); }
		}
	}
}
