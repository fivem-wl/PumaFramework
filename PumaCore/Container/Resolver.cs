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
using System.Reflection;
using static PumaFramework.Core.Container.CompoundKeyUtils;

namespace PumaFramework.Core.Container {

using ResolveBinding = ValueTuple<IEnumerable<Type>, Func<object, IComponent>>;

public class Resolver : IResolver
{
	public IResolver Parent { get; }

	public bool Fenced { get; }

	readonly IDictionary<object, object> _refs = new Dictionary<object, object>();

	
	public Resolver(IResolver parent = null) : this(parent, false) { }
	
	public Resolver(IResolver parent, bool fenced)
	{
		Parent = parent;
		Fenced = fenced;
	}
	
	public Container ConstructContainer(Type clazz)
	{
		bool fenced = clazz.GetCustomAttribute<FenceAttribute>() != null;
		
		var container = Activator.CreateInstance(clazz) as Container;
		container.Resolver = new Resolver(this, fenced);
		
		foreach (BindAttribute attr in clazz.GetCustomAttributes(typeof(BindAttribute), true))
		{
			container.NewComponent(attr.Implementation);
			if (attr.To != null) container.BindComponents(attr.Implementation, attr.To);
		}

		IDictionary<Component, bool> initializingComponents = new Dictionary<Component, bool>();
		bool InitDeps(Component component)
		{
			if (!initializingComponents.TryGetValue(component, out var inited))
			{
				initializingComponents[component] = false;
				if (!component.GetType().GetCustomAttributes(typeof(RequireAttribute))
					.Select(attr => attr as RequireAttribute)
					.All(dep =>
					{
						var depComponent = container.GetComponent(dep.GetType()) as Component;
						return InitDeps(depComponent);
					})
				) return false;
			}
			else if (inited == false) return false;
			
			component.Init();
			initializingComponents[component] = true;
			return true;
		}

		if (!container._components
			.Where(e => ((e.Key as Type) == e.Value.GetType()))
			.All(e => InitDeps(e.Value as Component))
		) return null;
		
		container.Init();
		return container;
	}

	public void RegisterReferenceRaw(object obj, object key = null, params Type[] types)
	{
		//Trace.Assert(types.Length > 0);
		foreach (var type in types)
		{
			//Trace.Assert(type.IsInstanceOfType(obj));
			_refs[CompoundKey(type, key)] = obj;
		}
	}

	public bool UnregisterReference(object key = null, params Type[] types)
	{
		return types.Aggregate(false, (current, type) => current || _refs.Remove(CompoundKey(type, key)));
	}

	public bool UnregisterReferences(params Type[] types)
	{
		var result = false;
		foreach (var type in types)
		{
			var components = _refs
				.Where(e => IsCompoundKey(e.Key) && GetTypeFromKey(e.Key) == type)
				.ToList();
		
			foreach (var entry in components) result |= UnregisterReference(GetObjectFromKey(entry.Key), type);
		}
		return result;
	}

	public object ResolveReference(Type type, object key = null)
	{
		return _refs.TryGetValue(type, out var obj) ? obj : Parent?.ResolveReference(type);
	}
}

}
