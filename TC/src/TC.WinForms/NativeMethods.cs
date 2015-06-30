// TC WinForms Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Win32.SafeHandles;

namespace TC.WinForms
{
	internal static class NativeMethods
	{
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		internal static extern IntPtr SendMessage(IntPtr handle, uint message, IntPtr wParam, IntPtr lParam);

		[DllImport("uxtheme.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int SetWindowTheme(IntPtr handle, string applicationName, string idList);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetWindowDC(IntPtr handle);

		[DllImport("user32.dll")]
		internal static extern int ReleaseDC(IntPtr handle, IntPtr dc);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowRect(IntPtr handle, out RECT rect);

		[DllImport("gdi32.dll")]
		internal static extern int ExcludeClipRect(IntPtr dc, int left, int top, int right, int bottom);

		internal const int
			WM_NCCALCSIZE = 0x83,
			WM_NCPAINT = 0x85,
			WM_THEMECHANGED = 0x031A,

			WS_EX_CLIENTEDGE = 0x0200,

			WVR_HREDRAW = 0x0100,
			WVR_VREDRAW = 0x0200,
			WVR_REDRAW = WVR_HREDRAW | WVR_VREDRAW,

			TV_FIRST = 0x1100,
			TVM_SETEXTENDEDSTYLE = TV_FIRST + 44,
			TVM_GETEXTENDEDSTYLE = TV_FIRST + 45,
			TVM_SETAUTOSCROLLINFO = TV_FIRST + 59,

			TVS_NOHSCROLL = 0x8000,
			TVS_EX_DOUBLEBUFFER = 0x0004,
			TVS_EX_AUTOHSCROLL = 0x0020,
			TVS_EX_FADEINOUTEXPANDOS = 0x0040,

			ERROR = 0,
			NULLREGION = 1,
			SIMPLEREGION = 2,
			COMPLEXREGION = 3;

		[StructLayout(LayoutKind.Sequential)]
		internal struct RECT
		{
			public int Left, Top, Right, Bottom;

			public RECT(int left, int top, int right, int bottom)
			{
				Left = left;
				Top = top;
				Right = right;
				Bottom = bottom;
			}

			public RECT(Rectangle rectangle)
			{
				Left = rectangle.Left;
				Top = rectangle.Top;
				Right = rectangle.Right;
				Bottom = rectangle.Bottom;
			}

			public Rectangle ToRectangle()
			{
				return Rectangle.FromLTRB(Left, Top, Right, Bottom);
			}

			public void Inflate(int width, int height)
			{
				Left -= width;
				Top -= height;
				Right += width;
				Bottom += height;
			}
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct NCCALCSIZE_PARAMS
		{
			public RECT rgrc0, rgrc1, rgrc2;
			public IntPtr lpPos;
		}

		internal static T PtrToStruct<T>(IntPtr ptr) where T : struct
		{
			return (T)Marshal.PtrToStructure(ptr, typeof(T));
		}

		internal sealed class DeviceContext : IDisposable, IDeviceContext
		{
			private readonly IntPtr _hWnd;
			private Hdc _dc;
			private bool _disposed, _shouldCallDangerousRelease;

			public DeviceContext(IntPtr hWnd)
			{
				_hWnd = hWnd;
			}

			~DeviceContext()
			{
				DisposeCore();
			}

			public void Dispose()
			{
				DisposeCore();
				GC.SuppressFinalize(this);
			}

			private void DisposeCore()
			{
				if (!_disposed)
				{
					ReleaseHdc();
					_disposed = true;
				}
			}

			#region IDeviceContext Members

			[SuppressMessage(
				"Microsoft.Reliability", 
				"CA2001:AvoidCallingProblematicMethods", 
				MessageId = "System.Runtime.InteropServices.SafeHandle.DangerousGetHandle",
				Justification = "The IDeviceContext requires the HDC to be returned.")]
			public IntPtr GetHdc()
			{
				if (_dc == null)
					_dc = new Hdc(_hWnd);

				_shouldCallDangerousRelease = false;
				_dc.DangerousAddRef(ref _shouldCallDangerousRelease);

				return _dc.DangerousGetHandle();
			}

			public void ReleaseHdc()
			{
				if (_dc != null)
				{
					if (_shouldCallDangerousRelease)
						_dc.DangerousRelease();

					_dc.Close();
					_dc = null;
				}
			}

			#endregion

			#region inner class Hdc

			private sealed class Hdc : SafeHandleZeroOrMinusOneIsInvalid
			{
				internal Hdc(IntPtr hWnd)
					: base(true)
				{
					_hWnd = hWnd;
					SetHandle(GetWindowDC(hWnd));
				}

				private readonly IntPtr _hWnd;

				protected override bool ReleaseHandle()
				{
					return ReleaseDC(_hWnd, handle) != 0;
				}
			}

			#endregion
		}
	}
}