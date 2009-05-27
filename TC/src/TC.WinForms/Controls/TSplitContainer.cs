// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using TC.WinForms.Forms;
using TC.WinForms.Settings;

namespace TC.WinForms.Controls
{
	/// <summary>Represents a control that contains 2 resizable panels with a splitter inbetween.</summary>
	[ToolboxBitmap(typeof(SplitContainer))]
	public class TSplitContainer : SplitContainer
	{
		/// <summary>Initializes a new instance of the <see cref="T:TSplitContainer"/> class.</summary>
		public TSplitContainer()
		{
			SplitterMoved += HandlerSplitterMoved;
		}

		private void HandlerSplitterMoved(object sender, SplitterEventArgs e)
		{
			TForm form = FindForm() as TForm;
			if (form != null) form.SaveSettings();
		}

		/// <summary>Loads the settings of this control.</summary>
		/// <param name="settings">The <see cref="T:SplitContainerSettings"/> to load from.</param>
		public void LoadSettings(SplitContainerSettings settings)
		{
			if (settings == null) throw new ArgumentNullException("settings");

			if (settings.SplitterDistance > 0)
				SplitterDistance = settings.SplitterDistance;

			switch (settings.CollapsedPanel)
			{
				case FixedPanel.Panel1: Panel1Collapsed = true; break;
				case FixedPanel.Panel2: Panel2Collapsed = true; break;
			}
		}

		/// <summary>Saves the settings of this control.</summary>
		/// <param name="settings">The <see cref="T:SplitContainerSettings"/> to save to.</param>
		public void SaveSettings(SplitContainerSettings settings)
		{
			if (settings == null) throw new ArgumentNullException("settings");

			settings.CollapsedPanel
				= Panel1Collapsed ? FixedPanel.Panel1
				: Panel2Collapsed ? FixedPanel.Panel2
				: FixedPanel.None;

			if (!(Panel1Collapsed || Panel2Collapsed))
				settings.SplitterDistance = SplitterDistance;
		}
	}
}
