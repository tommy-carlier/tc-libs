// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System.Diagnostics.CodeAnalysis;

namespace TC.Data
{
	/// <summary>Contains the values of a record.</summary>
	public interface IRecord
	{
		/// <summary>Gets the <see cref="T:IRecordDescriptor"/> that describes the fields.</summary>
		/// <value>The <see cref="T:IRecordDescriptor"/> that describes the fields.</value>
		IRecordDescriptor Descriptor { get; }

		/// <summary>Gets the <see cref="T:IRecordMetadata"/> that contains extra information.</summary>
		/// <value>The <see cref="T:IRecordMetadata"/> that contains extra information.</value>
		IRecordMetadata Metadata { get; }

        /// <summary>Gets the value of the field at the specified index.</summary>
        /// <typeparam name="TValue">The type of the value to get.</typeparam>
        /// <param name="ordinal">The zero-based index of the field.</param>
        /// <returns>The value of the field at the specified index.</returns>
        TValue GetValue<TValue>(int ordinal);

		/// <summary>Determines whether the value of the field at the specified index is null.</summary>
		/// <param name="ordinal">The zero-based index of the field.</param>
		/// <returns>If the value of the specified field is null, <c>true</c>; otherwise, <c>false</c>.</returns>
		bool IsNull(int ordinal);
	}
}
