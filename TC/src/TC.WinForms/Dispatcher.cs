// TC WinForms Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Windows.Forms;

namespace TC.WinForms
{
	/// <summary>Provides functions to invoke methods on the UI-thread.</summary>
	public static class Dispatcher
	{
		/// <summary>Invokes the specified method on the UI-thread.</summary>
		/// <param name="control">The control that was created on the UI-thread.</param>
		/// <param name="method">The method to invoke.</param>
		public static void Invoke(this Control control, Action method)
		{
			if (control == null) throw new ArgumentNullException("control");
			if (method == null) throw new ArgumentNullException("method");

			control.Invoke(method);
		}
		
		/// <summary>Invokes the specified method on the UI-thread, asynchronously.</summary>
		/// <param name="control">The control that was created on the UI-thread.</param>
		/// <param name="method">The method to invoke.</param>
		public static void InvokeAsync(this Control control, Action method)
		{
			if (control == null) throw new ArgumentNullException("control");
			if (method == null) throw new ArgumentNullException("method");

			control.BeginInvoke(method);
		}

		/// <summary>Invokes the specified method on the UI-thread.</summary>
		/// <typeparam name="TArg">The type of the argument to pass to the method.</typeparam>
		/// <param name="control">The control that was created on the UI-thread.</param>
		/// <param name="method">The method to invoke.</param>
		/// <param name="argument">The argument to pass to the method.</param>
		public static void Invoke<TArg>(this Control control, Action<TArg> method, TArg argument)
		{
			if (control == null) throw new ArgumentNullException("control");
			if (method == null) throw new ArgumentNullException("method");

			control.Invoke(method, argument);
		}

		/// <summary>Invokes the specified method on the UI-thread, asynchronously.</summary>
		/// <typeparam name="TArg">The type of the argument to pass to the method.</typeparam>
		/// <param name="control">The control that was created on the UI-thread.</param>
		/// <param name="method">The method to invoke.</param>
		/// <param name="argument">The argument to pass to the method.</param>
		public static void InvokeAsync<TArg>(this Control control, Action<TArg> method, TArg argument)
		{
			if (control == null) throw new ArgumentNullException("control");
			if (method == null) throw new ArgumentNullException("method");

			control.BeginInvoke(method, argument);
		}

		/// <summary>Invokes the specified method on the UI-thread.</summary>
		/// <typeparam name="TArg1">The type of the first argument to pass to the method.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument to pass to the method.</typeparam>
		/// <param name="control">The control that was created on the UI-thread.</param>
		/// <param name="method">The method to invoke.</param>
		/// <param name="argument1">The first argument to pass to the method.</param>
		/// <param name="argument2">The second argument to pass to the method.</param>
		public static void Invoke<TArg1, TArg2>(
			this Control control, 
			Action<TArg1, TArg2> method, 
			TArg1 argument1, 
			TArg2 argument2)
		{
			if (control == null) throw new ArgumentNullException("control");
			if (method == null) throw new ArgumentNullException("method");

			control.Invoke(method, argument1, argument2);
		}

		/// <summary>Invokes the specified method on the UI-thread, asynchronously.</summary>
		/// <typeparam name="TArg1">The type of the first argument to pass to the method.</typeparam>
		/// <typeparam name="TArg2">The type of the second argument to pass to the method.</typeparam>
		/// <param name="control">The control that was created on the UI-thread.</param>
		/// <param name="method">The method to invoke.</param>
		/// <param name="argument1">The first argument to pass to the method.</param>
		/// <param name="argument2">The second argument to pass to the method.</param>
		public static void InvokeAsync<TArg1, TArg2>(
			this Control control,
			Action<TArg1, TArg2> method,
			TArg1 argument1, 
			TArg2 argument2)
		{
			if (control == null) throw new ArgumentNullException("control");
			if (method == null) throw new ArgumentNullException("method");

			control.BeginInvoke(method, argument1, argument2);
		}
	}
}
