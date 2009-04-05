// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace TC.WinForms
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SetWindowTheme(IntPtr handle, string applicationName, string idList);

		internal const int
			TV_FIRST = 0x1100,
			TVM_SETEXTENDEDSTYLE = TV_FIRST + 44,
			TVM_GETEXTENDEDSTYLE = TV_FIRST + 45,
			TVM_SETAUTOSCROLLINFO = TV_FIRST + 59,
			TVS_NOHSCROLL = 0x8000,
			TVS_EX_DOUBLEBUFFER = 0x0004,
			TVS_EX_AUTOHSCROLL = 0x0020,
			TVS_EX_FADEINOUTEXPANDOS = 0x0040;
	}
}