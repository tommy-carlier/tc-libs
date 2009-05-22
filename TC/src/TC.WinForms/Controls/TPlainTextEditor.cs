// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a control for editing multi-line text.</summary>
	[SuppressMessage(
		"Microsoft.Naming",
		"CA1702:CompoundWordsShouldBeCasedCorrectly",
		MessageId = "PlainText",
		Justification = "In this case, I mean 'plain text' and not 'plaintext' (which is a cryptographic term)")]
	public class TPlainTextEditor : TTextEditor, IHasSystemFont
	{
		/// <summary>Initializes a new instance of the <see cref="TPlainTextEditor"/> class.</summary>
		public TPlainTextEditor()
		{
			Font = SystemFont.ToFont();
		}

		/// <summary>Handles the specified keyboard key.</summary>
		/// <param name="keyData">The data of the pressed key.</param>
		/// <param name="inReadOnlyMode">Indicates whether the control is currently in read-only mode.</param>
		/// <returns>If the key was handled, true; otherwise, false.</returns>
		protected override bool HandleKey(Keys keyData, bool inReadOnlyMode)
		{
			if (!inReadOnlyMode)
				switch (keyData)
				{
					case Keys.Enter:
						// Enter => handle the enter key
						HandleEnterKey();
						return true;
				}

			return base.HandleKey(keyData, inReadOnlyMode);
		}

		/// <summary>Increases the line indent.</summary>
		public override void IncreaseLineIndent()
		{
			string lText = SelectedText;
			ReplaceSelectedText(IncreaseLineIndent(lText), lText.Length > 0);
		}

		private static string IncreaseLineIndent(string text)
		{
			if (text.Length == 0)
				return "\t";

			return text.Length > 1 && text.EndsWith('\n')
				? ("\t" + text.Substring(0, text.Length - 1).Replace("\n", "\n\t") + "\n")
				: ("\t" + text.Replace("\n", "\n\t"));
		}

		/// <summary>Decreases the line indent.</summary>
		public override void DecreaseLineIndent()
		{
			if (SelectionLength == 0)
			{
				int lStart = SelectionStart;
				if (lStart > 0 && Text[lStart - 1] == '\t')
					SendKeys.Send("{BS}");
			}
			else
			{
				string lText = SelectedText;
				if (lText.StartsWith('\t'))
					lText = lText.Length > 1
						? lText.Substring(1)
						: String.Empty;

				ReplaceSelectedText(lText.Replace("\n\t", "\n"), true);
			}
		}

		private void HandleEnterKey()
		{
			int lFirstCharIndex = GetFirstCharIndexOfCurrentLine();
			int lCurrentCharIndex = SelectionStart;

			string lNewText = "\n";
			if (lCurrentCharIndex > lFirstCharIndex)
				lNewText += GetIndentForNewLine(Text.Substring(lFirstCharIndex, lCurrentCharIndex - lFirstCharIndex));

			ReplaceSelectedText(lNewText, false);
		}

		private static readonly Regex fRegexPreviousLineIndent
			= new Regex(@"^([\t ]*).*$", RegexOptions.Compiled);

		/// <summary>Gets the indent to add to the new line after the user presses the Enter-key.</summary>
		/// <param name="previousLine">The previous line.</param>
		/// <returns>The indent to add to the new line.</returns>
		protected virtual string GetIndentForNewLine(string previousLine)
		{
			if (previousLine == null) throw new ArgumentNullException("previousLine");

			Match lMatch = fRegexPreviousLineIndent.Match(previousLine);
			return lMatch.Success ? lMatch.Groups[1].Value : String.Empty;
		}

		#region IHasSystemFont Members

		/// <summary>Gets the <see cref="T:SystemFont"/> of the control.</summary>
		/// <value>The <see cref="T:SystemFont"/> of the control.</value>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual SystemFont SystemFont { get { return SystemFont.Default; } }

		/// <summary>Gets or sets the font used when displaying text in the control.</summary>
		/// <returns>The <see cref="T:Font"/> to apply to the text displayed by the control.</returns>
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}

		#endregion
	}
}
