// TC WinForms Library
// Copyright © 2008-2015 Tommy Carlier
// https://github.com/tommy-carlier/tc-libs/
// License: MIT License (MIT): https://github.com/tommy-carlier/tc-libs/blob/master/LICENSE

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace TC.WinForms.Animation
{
	internal static class PropertySetters<TTarget, TValue>
	{
		private static readonly Dictionary<string, PropertySetter<TTarget, TValue>>
			_cachedPropertySetters = new Dictionary<string, PropertySetter<TTarget, TValue>>();

		private static readonly object _lock = new object();

		internal static PropertySetter<TTarget, TValue> GetPropertySetter(string propertyName)
		{
			PropertySetter<TTarget, TValue> propertySetter;

			lock (_lock)
				if (!_cachedPropertySetters.TryGetValue(propertyName, out propertySetter))
				{
					propertySetter = CreatePropertySetter(propertyName);
					if (propertySetter != null)
						_cachedPropertySetters[propertyName] = propertySetter;
				}

			return propertySetter;
		}

		private static PropertySetter<TTarget, TValue> CreatePropertySetter(string propertyName)
		{
			Type type = typeof(TTarget);
			PropertyInfo property = type.GetProperty(propertyName);
			MethodInfo setMethod = property != null ? property.GetSetMethod(true) : null;

			return setMethod != null
				? Delegate.CreateDelegate(
						typeof(PropertySetter<TTarget, TValue>),
						setMethod) as PropertySetter<TTarget, TValue>
				: null;
		}
	}
}
