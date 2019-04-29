/*
 * This file is part of PumaFramework.
 *
 * PumaFramework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * PumaFramework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with PumaFramework.  If not, see <https://www.gnu.org/licenses/>.
 */

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