// TC Core Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

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
		public static TAttribute GetFirstAttribute<TAttribute>(
			this ICustomAttributeProvider provider, 
			bool inherit) where TAttribute : Attribute
		{
			if (provider == null) throw new ArgumentNullException("provider");

			object[] attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);
			return
				attributes.HasItems()
					? attributes[0] as TAttribute
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
		public static IEnumerable<TAttribute> GetAttributes<TAttribute>(
			this ICustomAttributeProvider provider, 
			bool inherit) where TAttribute : Attribute
		{
			if (provider == null) throw new ArgumentNullException("provider");

			return GetAttributesCore<TAttribute>(provider, inherit);
		}

		private static IEnumerable<TAttribute> GetAttributesCore<TAttribute>(
			ICustomAttributeProvider provider,
			bool inherit) where TAttribute : Attribute
		{
			TAttribute attribute;
			object[] attributes = provider.GetCustomAttributes(typeof(TAttribute), inherit);
			if (attributes != null)
				for (int i = 0; i < attributes.Length; i++)
					if ((attribute = attributes[i] as TAttribute) != null)
						yield return attribute;
		}
	}
}
