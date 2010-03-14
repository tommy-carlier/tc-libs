// TC Data Library
// Copyright © 2009-2010 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Data
{
	/// <summary>Represents a <see cref="T:IRecord"/> that can be written to.</summary>
	public interface IWritableRecord : IRecord
	{
		/// <summary>Sets the value of the field at the specified index.</summary>
		/// <typeparam name="TValue">The type of the value to set.</typeparam>
		/// <param name="ordinal">The zero-based index of the field.</param>
		/// <param name="value">The value to assign to the specified field.</param>
		void SetValue<TValue>(int ordinal, TValue value);

		/// <summary>Sets the value of the field at the specified index to null.</summary>
		/// <param name="ordinal">The zero-based index of the field.</param>
		void SetNull(int ordinal);
	}
}
