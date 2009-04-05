// TC WinForms Library
// Copyright © 2008 - 2009 Tommy Carlier
// http://tc.codeplex.com
// License: Microsoft Public License (Ms-PL): http://tc.codeplex.com/license

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TC.WinForms.Animation
{
	internal static class PropertySetters<TTarget, TValue>
	{
		private static readonly Dictionary<string, PropertySetter<TTarget, TValue>>
			fCachedPropertySetters = new Dictionary<string, PropertySetter<TTarget, TValue>>();

		private static readonly object fLock = new object();

		internal static PropertySetter<TTarget, TValue> GetPropertySetter(string propertyName)
		{
			PropertySetter<TTarget, TValue> lPropertySetter;

			lock (fLock)
				if (!fCachedPropertySetters.TryGetValue(propertyName, out lPropertySetter))
				{
					lPropertySetter = CreatePropertySetter(propertyName);
					if (lPropertySetter != null)
						fCachedPropertySetters[propertyName] = lPropertySetter;
				}

			return lPropertySetter;
		}

		private static PropertySetter<TTarget, TValue> CreatePropertySetter(string propertyName)
		{
			Type lType = typeof(TTarget);
			PropertyInfo lProperty = lType.GetProperty(propertyName);
			MethodInfo lSetMethod = lProperty != null ? lProperty.GetSetMethod(true) : null;

			return lSetMethod != null
				? Delegate.CreateDelegate(
					typeof(PropertySetter<TTarget, TValue>),
					lSetMethod) as PropertySetter<TTarget, TValue>
				: null;
		}
	}
}
