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
using System.Linq;

namespace PumaFramework.Core.Container {

public static class ComponentExtensions
{
	// IEnumerable
	public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
	{
		foreach (var e in @this) action(e);
	}
	
	#region Reference
	
	public static void RegisterReference<T>(this Component component, T obj, object key = null, params Type[] types) where T : class =>
		component.RegisterReferenceRaw(obj, key, types.Concat(new []{ typeof(T)}).ToArray());
	
	public static void UnregisterReference<T>(this Component component) where T : class =>
		component.UnregisterReference(typeof(T));
	
	public static T Get<T>(this Component component, object key = null) where T : class =>
		(T) component.Get(typeof(T), key);
	
	public static T Get<T>(this Component component, Type type, object key = null) where T : class =>
		(T) component.Get(type, key);

	public static object Require(this Component component, Type type, object key = null)
	{
		var obj = component.Get(type, key);
		if (obj == null) throw new UnsatisfiedDependencyException(type);
		return obj;
	}

	public static T Require<T>(this Component component, object key = null) where T : class =>
		(T) component.Require(typeof(T), key);
	
	#endregion Reference
	
	#region Component
	
	public static T AddComponent<T>(this Component component, object key = null, IEnumerable<Type> bindTo = null) where T : Component =>
		component.AddComponent(typeof(T), key, bindTo) as T;

	public static bool RemoveComponent<T>(this Component component, object key = null) where T : Component =>
		component.RemoveComponent(typeof(T), key);
	
	#endregion Component
	
}

}