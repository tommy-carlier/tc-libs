// TC Core Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://www.codeplex.com/tccore
// License: Microsoft Public License (Ms-PL): http://www.codeplex.com/tccore/license

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace TC
{
	/// <summary>Provides utilities that deal with reflection.</summary>
	public static class ReflectionUtilities
	{
		/// <summary>Gets the first attribute of the specified type defined on the specified provider.</summary>
		/// <typeparam name="TAttribute">The type of the attribute to get.</typeparam>
		/// <param name="provider">The <see cref="T:ICustomAttributeProvider"/> to investigate.</param>
		/// <param name="inherit">if set to <c>true</c>, look up the hierarchy chain for the inherited attribute.</param>
		/// <returns>The first attribute of the specified type, or null if not found.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TAttribute is an important parameter and knowledge of generics is essential for using this function.")]
		public static TAttribute GetFirstAttribute<TAttribute>(this ICustomAttributeProvider provider, bool inherit) where TAttribute : Attribute
		{
			if (provider == null) throw new ArgumentNullException("provider");

			object[] lAttributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);
			return
				(lAttributes != null && lAttributes.Length > 0)
					? lAttributes[0] as TAttribute
					: null;
		}

		/// <summary>Gets the attributes of the specified type defined on the specified provider.</summary>
		/// <typeparam name="TAttribute">The type of the attributes to get.</typeparam>
		/// <param name="provider">The <see cref="T:ICustomAttributeProvider"/> to investigate.</param>
		/// <param name="inherit">if set to <c>true</c>, look up the hierarchy chain for the inherited attribute.</param>
		/// <returns>The collection of attributes of the specified type.</returns>
		[SuppressMessage(
			"Microsoft.Design",
			"CA1004:GenericMethodsShouldProvideTypeParameter",
			Justification = "The type TAttribute is an important parameter and knowledge of generics is essential for using this function.")]
		public static IEnumerable<TAttribute> GetAttributes<TAttribute>(this ICustomAttributeProvider provider, bool inherit) where TAttribute : Attribute
		{
			if (provider == null) throw new ArgumentNullException("provider");

			TAttribute lAttribute;
			object[] lAttributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);
			if (lAttributes != null)
				for (int i = 0; i < lAttributes.Length; i++)
					if ((lAttribute = lAttributes[i] as TAttribute) != null)
						yield return lAttribute;
		}
	}
}
