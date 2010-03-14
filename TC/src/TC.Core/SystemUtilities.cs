// TC Core Library
// Copyright © 2008-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace TC
{
	/// <summary>Provides system utilities.</summary>
	public static class SystemUtilities
	{
		/// <summary>Determines whether the specified exception is a critical exception.</summary>
		/// <param name="exception">The exception to check.</param>
		/// <returns><c>true</c> if the specified exception is critical; otherwise, <c>false</c>.</returns>
		/// <remarks>This function is used to determine whether to re-throw an exception 
		/// in a generic try { } catch(Exception) { } construct.</remarks>
		public static bool IsCritical(this Exception exception)
		{
			if (exception == null) throw new ArgumentNullException("exception");
			return
				exception is OutOfMemoryException
				|| exception is AppDomainUnloadedException
				|| exception is BadImageFormatException
				|| exception is CannotUnloadAppDomainException
				|| exception is ExecutionEngineException
				|| exception is InvalidProgramException
				|| exception is ThreadAbortException;
		}

		/// <summary>Determines whether the application is currently in design-mode.</summary>
		/// <remarks>This is a hack that only works in Visual Studio.
		/// See http://www.dotnetjunkies.com/WebLog/mjordan/archive/2003/12/01/4117.aspx</remarks>
		public static readonly bool DesignMode = Process.GetCurrentProcess().ProcessName == "devenv";

		#region version numbers

		/// <summary>Gets the Windows XP version number.</summary>
		public static readonly Version WindowsXPVersion = new Version(5, 1);

		/// <summary>Gets the Windows Vista version number.</summary>
		public static readonly Version WindowsVistaVersion = new Version(6, 0);

		/// <summary>Gets the Windows 7 version number.</summary>
		public static readonly Version Windows7Version = new Version(6, 1);

		/// <summary>Indicates whether the current OS is Windows NT-based.</summary>
		public static readonly bool IsRunningOnWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;

		/// <summary>Determines whether the current OS version is at least the specified version.</summary>
		/// <param name="version">The version to compare with.</param>
		/// <returns><c>true</c> if the current OS version is at least the specified version; otherwise, <c>false</c>.</returns>
		public static bool IsOSVersionAtLeast(Version version)
		{
			if (version == null) throw new ArgumentNullException("version");
			return Environment.OSVersion.Version >= version;
		}

		/// <summary>Indicates whether the current OS is Windows XP or later.</summary>
		public static readonly bool IsWindowsXPOrLater 
			= IsRunningOnWindows && Environment.OSVersion.Version >= WindowsXPVersion;

		/// <summary>Indicates whether the current OS is Windows Vista or later.</summary>
		public static readonly bool IsWindowsVistaOrLater
			= IsRunningOnWindows && Environment.OSVersion.Version >= WindowsVistaVersion;

		/// <summary>Indicates whether the current OS is Windows 7 or later.</summary>
		public static readonly bool IsWindows7OrLater 
			= IsRunningOnWindows && Environment.OSVersion.Version >= Windows7Version;

		#endregion
	}
}
