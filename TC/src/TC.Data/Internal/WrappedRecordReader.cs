// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Text;

namespace TC.Data.Internal
{
	internal abstract class WrappedRecordReader : IRecordReader
	{
		private readonly IRecordReader _reader;

		protected WrappedRecordReader(IRecordReader reader)
		{
			_reader = reader;
		}

		~WrappedRecordReader()
		{
			Dispose(false);
		}

		protected IRecordReader Reader
		{
			get { return _reader; }
		}

		#region IRecordReader Members

		public virtual bool Read()
		{
			return _reader.Read();
		}

		#endregion

		#region IRecord Members

		public virtual IRecordDescriptor Descriptor
		{
			get { return _reader.Descriptor; }
		}

		public virtual IRecordMetadata Metadata
		{
			get { return _reader.Metadata; }
		}

		public virtual TValue GetValue<TValue>(int ordinal)
		{
			return _reader.GetValue<TValue>(ordinal);
		}

		public virtual bool IsNull(int ordinal)
		{
			return _reader.IsNull(ordinal);
		}

		#endregion

		#region IDisposable Members

		void IDisposable.Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
				_reader.Dispose();
		}

		#endregion
	}
}
