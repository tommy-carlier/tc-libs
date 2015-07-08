// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Data
{
	/// <summary>Provides a means to build a <see cref="T:IRecordDescriptor"/> instance.</summary>
	public sealed class RecordDescriptorBuilder
	{
		private readonly ICollection<Field>
			_fields = new List<Field>();

		private readonly IDictionary<string, object>
			_nameSet = new Dictionary<string, object>();

		/// <summary>Removes all the fields.</summary>
		public void Clear()
		{
			_fields.Clear();
			_nameSet.Clear();
		}

		/// <summary>Adds a new field to the builder.</summary>
		/// <param name="name">The name of the field.</param>
		/// <param name="type">The type of the value of the field.</param>
		/// <returns>The builder itself.</returns>
		public RecordDescriptorBuilder AddField(string name, Type type)
		{
			if (name == null) throw new ArgumentNullException("name");
			if (type == null) throw new ArgumentNullException("type");
			if (name.Length == 0)
				throw new ArgumentException("name cannot be an empty string", "name");

			if (_nameSet.ContainsKey(name))
				throw new ArgumentException("This builder already contains a field with the specified name", "name");

			_fields.Add(new Field { Name = name, Type = type });
			_nameSet.Add(name, null);

			return this;
		}

		/// <summary>Builds an <see cref="T:IRecordDescriptor"/> instance.</summary>
		/// <returns>The created instance.</returns>
		public IRecordDescriptor Build()
		{
			return new Descriptor(_fields);
		}

		internal static IRecordDescriptor Copy(IRecordDescriptor metadata)
		{
			return new Descriptor(metadata);
		}

		#region inner type Field

		private struct Field
		{
			public string Name;
			public Type Type;
		}

		#endregion

		#region inner type Descriptor

		private sealed class Descriptor : IRecordDescriptor
		{
			private readonly string[] _names;
			private readonly Type[] _types;
			private readonly IDictionary<string, int> _ordinals;

			private Descriptor(int fieldCount)
			{
				_names = new string[fieldCount];
				_types = new Type[fieldCount];
				_ordinals = new Dictionary<string, int>(fieldCount);
			}

			internal Descriptor(ICollection<Field> fields)
				: this(fields.Count)
			{
				int index = 0;
				foreach (Field field in fields)
				{
					_names[index] = field.Name;
					_types[index] = field.Type;
					_ordinals.Add(field.Name, index);

					index += 1;
				}
			}

			internal Descriptor(IRecordDescriptor metadata)
				: this(metadata.FieldCount)
			{
				for (int i = 0; i < _names.Length; i++)
				{
					string fieldName = metadata.GetFieldName(i);
					_names[i] = fieldName;
					_types[i] = metadata.GetFieldType(i);
					_ordinals.Add(fieldName, i);
				}
			}

			#region IRecordDescriptor Members

			int IRecordDescriptor.FieldCount
			{
				get { return _names.Length; }
			}

			string IRecordDescriptor.GetFieldName(int ordinal)
			{
				VerifyOrdinal(ordinal);
				return _names[ordinal];
			}

			Type IRecordDescriptor.GetFieldType(int ordinal)
			{
				VerifyOrdinal(ordinal);
				return _types[ordinal];
			}

			int IRecordDescriptor.GetFieldOrdinal(string name)
			{
				if (name == null) throw new ArgumentNullException("name");
				if (name.Length == 0) throw new ArgumentException("name cannot be an empty string", "name");

				return _ordinals.GetValue(name, -1);
			}

			#endregion

			private void VerifyOrdinal(int ordinal)
			{
				if (ordinal < 0)
					throw new ArgumentOutOfRangeException(
						"ordinal",
						"ordinal cannot be negative");

				if (ordinal >= _names.Length)
					throw new ArgumentOutOfRangeException(
						"ordinal",
						"ordinal cannot be greater than or equal to the number of fields");
			}
		}

		#endregion
	}
}
