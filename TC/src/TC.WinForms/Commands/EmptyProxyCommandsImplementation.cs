// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Diagnostics.CodeAnalysis;

namespace TC.WinForms.Commands
{
	/// <summary>The empty implementation of <see cref="T:IProxyCommandsImplementation{TCommandIdentifier}"/>.</summary>
	/// <typeparam name="TCommandIdentifier">The type of the command identifiers to implement.</typeparam>
	public sealed class EmptyProxyCommandsImplementation<TCommandIdentifier> : IProxyCommandsImplementation<TCommandIdentifier>
	{
		private EmptyProxyCommandsImplementation() { }

        /// <summary>Gets the instance of the <see cref="T:EmptyProxyCommandsImplementation{TCommandIdentifier}"/>.</summary>
        public static readonly EmptyProxyCommandsImplementation<TCommandIdentifier>
			Instance = new EmptyProxyCommandsImplementation<TCommandIdentifier>();

		#region IProxyCommandsImplementation<TCommandIdentifier> Members

		/// <summary>Gets the <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</summary>
		/// <param name="identifier">The <see cref="T:TCommandIdentifier"/> to get the <see cref="T:ICommand"/> of.</param>
		/// <returns>The <see cref="T:ICommand"/> that implements the specified <see cref="T:TCommandIdentifier"/>.</returns>
		public ICommand GetCommand(TCommandIdentifier identifier) { return null; }

		#endregion
	}
}
