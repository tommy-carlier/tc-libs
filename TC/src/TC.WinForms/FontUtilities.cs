// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using Microsoft.Win32;

namespace TC.WinForms
{
	/// <summary>Provides utilities that deal with fonts.</summary>
	public static class FontUtilities
	{
		[SuppressMessage(
			"Microsoft.Performance",
			"CA1810:InitializeReferenceTypeStaticFieldsInline",
			Justification = "There is additional logic beyond simple static field initialization.")]
		static FontUtilities()
		{
			InitializeFontFamiliesByFontName();
			InitializeSystemFonts();

			SystemEvents.DisplaySettingsChanged += ResetFontSettings;
			SystemEvents.InstalledFontsChanged += ResetFontSettings;
			SystemEvents.UserPreferenceChanged += ResetFontSettings;
		}

		private static void ResetFontSettings(object sender, EventArgs e)
		{
			InitializeFontFamiliesByFontName();
			InitializeSystemFonts();

			// reset the font of the open forms and their controls
			foreach (Form form in Application.OpenForms)
				foreach (IHasSystemFont control in form.EnumerateDescendants<IHasSystemFont>(true))
					control.Font = control.SystemFont.ToFont();
		}

		#region GetFontFamily functions

		private static readonly Dictionary<string, FontFamily>
			_fontFamiliesByFontName = new Dictionary<string, FontFamily>();

		private static void InitializeFontFamiliesByFontName()
		{
			_fontFamiliesByFontName.Clear();
			foreach (FontFamily fontFamily in FontFamily.Families)
				_fontFamiliesByFontName[fontFamily.Name] = fontFamily;
		}

		/// <summary>Gets the <see cref="T:FontFamily"/> of the font with the specified name.</summary>
		/// <param name="fontName">The name of the font to get the <see cref="T:FontFamily"/> of.</param>
		/// <returns>The <see cref="T:FontFamily"/> of the font with the specified name,
		/// or null if the specified font cannot be found.</returns>
		public static FontFamily GetFontFamily(string fontName)
		{
			FontFamily fontFamily;
			if (!string.IsNullOrEmpty(fontName)
				&& _fontFamiliesByFontName.TryGetValue(fontName, out fontFamily))
			{
				return fontFamily;
			}
			else return null;
		}

		/// <summary>Gets the <see cref="T:FontFamily"/> of the font with one of the specified names.</summary>
		/// <param name="fontName">The name of the font to get the <see cref="T:FontFamily"/> of.</param>
		/// <param name="alternativeFontName">The name of the alternative font to get the <see cref="T:FontFamily"/> of.</param>
		/// <returns>The <see cref="T:FontFamily"/> of the font with the specified name, or the alternative font,
		/// or null if none of the specified fonts can be found.</returns>
		public static FontFamily GetFontFamily(string fontName, string alternativeFontName)
		{
			return GetFontFamily(fontName) ?? GetFontFamily(alternativeFontName);
		}

		/// <summary>Gets the <see cref="T:FontFamily"/> of the font with one of the specified names.</summary>
		/// <param name="fontNames">The names of the possible fonts to get the <see cref="T:FontFamily"/> of.</param>
		/// <returns>The <see cref="T:FontFamily"/> of the first font with one of the specified names,
		/// or null if none of the specified fonts can be found.</returns>
		public static FontFamily GetFontFamily(params string[] fontNames)
		{
			FontFamily fontFamily;
			foreach (string fontName in fontNames)
				if ((fontFamily = GetFontFamily(fontName)) != null)
					return fontFamily;
			return null;
		}

		#endregion

		#region ClearType rendering detection

		/// <summary>Gets a value indicating whether the operating system is using ClearType font rendering.</summary>
		/// <value><c>true</c> if the operating system is using ClearType font rendering; otherwise, <c>false</c>.</value>
		public static bool SystemUsesClearTypeRendering
		{
			get
			{
				try
				{
					return SystemUtilities.IsWindowsXPOrLater
						&& SystemInformation.FontSmoothingType == 2;
				}
				catch (NotSupportedException) { return false; }
			}
		}

		#endregion

		#region system fonts

		private static readonly Dictionary<SystemFont, Font> 
			_systemFonts = new Dictionary<SystemFont, Font>();

		private static void InitializeSystemFonts()
		{
			_systemFonts.Clear();

			Font defaultFont = SystemFonts.IconTitleFont;

			_systemFonts[SystemFont.Default] = defaultFont;
			_systemFonts[SystemFont.Bold] = new Font(defaultFont, FontStyle.Bold);
			_systemFonts[SystemFont.Italic] = new Font(defaultFont, FontStyle.Italic);
			_systemFonts[SystemFont.Header] 
				= new Font(
					defaultFont.FontFamily, 
					defaultFont.SizeInPoints * 1.5F, 
					FontStyle.Bold);

			_systemFonts[SystemFont.Monospace]
				= new Font(
					(SystemUsesClearTypeRendering // Consolas only looks good when ClearType rendering is used
						? GetFontFamily("Envy Code R", "Consolas", "Lucida Console", "Courier New")
						: GetFontFamily("Envy Code R", "Lucida Console", "Courier New"))
						?? FontFamily.GenericMonospace,
					defaultFont.SizeInPoints * 1.2F,
					FontStyle.Regular);
		}

		/// <summary>Converts the specified <see cref="T:SystemFont"/> to a <see cref="T:Font"/>.</summary>
		/// <param name="systemFont">The <see cref="T:SystemFont"/> to convert.</param>
		/// <returns>The <see cref="T:Font"/> that represents the specified <see cref="T:SystemFont"/>.</returns>
		public static Font ToFont(this SystemFont systemFont)
		{
			Font font;
			if (_systemFonts.TryGetValue(systemFont, out font))
				return font;
			else return SystemFonts.DefaultFont;
		}

		#endregion
	}
}
