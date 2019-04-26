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

public static class ContainerExtensions
{
	// IEnumerable
	public static void ForEach<T>(this IEnumerable<T> @this, Action<T> action)
	{
		foreach (var e in @this) action(e);
	}
	
	// IComponent
	public static T GetComponent<T>(this IComponent component, object key = null) where T : class, IComponent =>
		component.Owner?.GetComponent<T>(key);
	
	public static T ResolveReference<T>(this IComponent component) where T : class =>
		component.Owner.Resolver.ResolveReference<T>();
	
	// IContainer
	public static T GetComponent<T>(this IContainer container, object key = null) where T : class, IComponent =>
		container.GetComponent(typeof(T)) as T;
	
	public static T AddComponent<T>(this IContainer container, object key = null, IEnumerable<Type> bindTo = null) where T : Component, new() =>
		container.AddComponent(typeof(T), key, bindTo) as T;

	public static bool RemoveComponent<T>(this IContainer container, object key = null) where T : class, IComponent =>
		container.RemoveComponent(typeof(T), key);

	public static T ResolveReference<T>(this IContainer container) where T : class =>
		container.Resolver.ResolveReference<T>();
	
	// IResolver
	public static T ConstructContainer<T>(this IResolver resolver) where T : Container, new() =>
		resolver.ConstructContainer(typeof(T)) as T;
	
	public static void RegisterReference<T>(this IResolver resolver, T obj, object key = null, params Type[] types) where T : class =>
		resolver.RegisterReferenceRaw(obj, key, types.Concat(new []{ typeof(T)}).ToArray());
	
	public static void UnregisterReference<T>(this IResolver resolver) where T : class =>
		resolver.UnregisterReference(typeof(T));
	
	public static T ResolveReference<T>(this IResolver resolver) where T : class =>
		resolver.ResolveReference(typeof(T)) as T;
}

}