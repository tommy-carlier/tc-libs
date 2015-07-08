// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

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
