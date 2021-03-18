// TC Core Library
// Copyright © 2008-2021 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Diagnostics.CodeAnalysis;

namespace TC
{
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class ApplicationWebsiteAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:ApplicationWebsiteAttribute"/> class.</summary>
		/// <param name="uri">The URI of the official website of the running application.</param>
		/// <param name="title">The title or address of the official website of the running application.</param>
		public ApplicationWebsiteAttribute(string uri, string title)
		{
			if (uri == null) throw new ArgumentNullException("uri");

			if (!Uri.TryCreate(uri, UriKind.Absolute, out _uri))
				throw new ArgumentException("uri is not a valid absolute URI", "uri");

			Title = title.IsNullOrEmpty() 
					? uri.ToString() 
					: title;
		}

		private readonly Uri _uri;

        /// <summary>Gets the <see cref="T:Uri"/> of the official website of the running application.</summary>
        /// <value>The <see cref="T:Uri"/> of the official website of the running application.</value>
        public Uri Uri => _uri;

        /// <summary>Gets the title or address of the official website of the running application.</summary>
        /// <value>The title or address of the official website of the running application.</value>
        public string Title { get; }
    }
}
