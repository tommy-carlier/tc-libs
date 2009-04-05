// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

// This file is used by Code Analysis to maintain SuppressMessage 
// attributes that are applied to this project. 
// Project-level suppressions either have no target or are given 
// a specific target and scoped to a namespace, type, member, etc. 
//
// To add a suppression to this file, right-click the message in the 
// Error List, point to "Suppress Message(s)", and click 
// "In Project Suppression File". 
// You do not need to add suppressions to this file manually. 

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage(
	"Microsoft.Design",
	"CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace",
	Target = "System.Runtime.CompilerServices",
	Justification = "System.Runtime.CompilerServices.ExtensionAttribute is used to enable extension methods in .NET 2.0.")]

[assembly: SuppressMessage(
	"Microsoft.Design",
	"CA1020:AvoidNamespacesWithFewTypes",
	Scope = "namespace",
	Target = "TC.Settings",
	Justification = "TC.Settings has few types but is considered a separate category.")]

[assembly: SuppressMessage(
	"Microsoft.Performance",
	"CA1805:DoNotInitializeUnnecessarily",
	Scope = "member",
	Target = "TC.SystemUtilities.#.cctor()",
	Justification = "IsWindows7OrLater, IsWindowsVistaOrLater and IsWindowsXPOrLater are NOT automatically initialized by the runtime (bug in Code Analysis?)")]
