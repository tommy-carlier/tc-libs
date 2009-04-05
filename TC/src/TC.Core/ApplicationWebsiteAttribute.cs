// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tccore
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tccore/license

using System;
using System.Collections.Generic;
using System.Text;

namespace TC
{
	/// <summary>Specifies the URL of the official website of this application.</summary>
	/// <remarks>A hyperlink for this website is displayed in the About-dialog.</remarks>
	[AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = false)]
	public sealed class ApplicationWebsiteAttribute : Attribute
	{
		/// <summary>Initializes a new instance of the <see cref="T:ApplicationWebsiteAttribute"/> class.</summary>
		/// <param name="uri">The URI of the official website of the running application.</param>
		/// <param name="displayString">The title or address of the official website of the running application.</param>
		public ApplicationWebsiteAttribute(string uri, string displayString)
		{
			if (uri == null) throw new ArgumentNullException("uri");

			if (!Uri.TryCreate(uri, UriKind.Absolute, out fUri))
				throw new ArgumentException("uri is not a valid absolute URI", "uri");

			fDisplayString 
				= string.IsNullOrEmpty(displayString) 
					? uri.ToString() 
					: displayString;
		}

		private readonly Uri fUri;

		/// <summary>Gets the <see cref="T:Uri"/> of the official website of the running application.</summary>
		/// <value>The <see cref="T:Uri"/> of the official website of the running application.</value>
		public Uri Uri { get { return fUri; } }

		private readonly string fDisplayString;

		/// <summary>Gets the title or address of the official website of the running application.</summary>
		/// <value>The title or address of the official website of the running application.</value>
		public string DisplayString { get { return fDisplayString; } }
	}
}
