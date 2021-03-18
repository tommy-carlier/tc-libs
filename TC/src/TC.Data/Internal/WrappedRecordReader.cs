// TC Data Library
// Copyright © 2009-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;

namespace TC.Data.Internal
{
	internal abstract class WrappedRecordReader : IRecordReader
	{
        protected WrappedRecordReader(IRecordReader reader) => Reader = reader;

        ~WrappedRecordReader()
		{
			Dispose(false);
		}

        protected IRecordReader Reader { get; }

        #region IRecordReader Members

        public virtual bool Read() => Reader.Read();

        #endregion

        #region IRecord Members

        public virtual IRecordDescriptor Descriptor => Reader.Descriptor;

        public virtual IRecordMetadata Metadata => Reader.Metadata;

        public virtual TValue GetValue<TValue>(int ordinal) => Reader.GetValue<TValue>(ordinal);

        public virtual bool IsNull(int ordinal) => Reader.IsNull(ordinal);

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
				Reader.Dispose();
		}

		#endregion
	}
}
