// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

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
	Target = "~N:TC.Settings",
	Justification = "TC.Settings has few types but is considered a separate category.")]

[assembly: SuppressMessage(
	"Microsoft.Performance",
	"CA1805:DoNotInitializeUnnecessarily",
	Scope = "member",
	Target = "~M:TC.SystemUtilities.#cctor",
	Justification = "IsWindows7OrLater, IsWindowsVistaOrLater and IsWindowsXPOrLater are NOT automatically initialized by the runtime (bug in Code Analysis?)")]
