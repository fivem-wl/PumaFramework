using System;
using System.Collections.Generic;
using System.Reflection;

namespace PumaFramework.Core {

public static class TypeExtensions
{
	public static MethodInfo[] GetMethodsEx(this Type type, BindingFlags bindingFlags)
	{
		if (!bindingFlags.HasFlag(BindingFlags.NonPublic)) return type.GetMethods(bindingFlags);
		
		var methods = new List<MethodInfo>();
		for (; type.BaseType != null; type = type.BaseType)
		{
			methods.AddRange(type.GetMethods(bindingFlags | BindingFlags.DeclaredOnly));
		}
		return methods.ToArray();
	}
}

}