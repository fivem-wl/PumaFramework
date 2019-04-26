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
using static PumaFramework.Core.Container.CompoundKeyUtils;

namespace PumaFramework.Core.Container {

public abstract class Container : IContainer
{
	public IResolver Resolver { get; internal set; }

	internal readonly IDictionary<object, IComponent> _components = new Dictionary<object, IComponent>();
	
	
	public IContainer Owner => this;

	public IComponent GetComponent(Type type, object key = null) => _components[CompoundKey(type, key)];

	public IList<ValueTuple<object, IComponent>> GetComponents(Type type)
	{
		return _components
			.Where(e => IsCompoundKey(e.Key) && GetTypeFromKey(e.Key) == type)
			.Select(e => new ValueTuple<object, IComponent>(GetObjectFromKey(e.Key), e.Value))
			.ToList();
	}

	public IComponent AddComponent(Type type, object key = null, IEnumerable<Type> bindTo = null)
	{
		var component = NewComponent(type, key);
		if (bindTo != null) BindComponents(type, bindTo);
		
		component.Init();
		return component;
	}
		
	internal Component NewComponent(Type clazz, object key = null)
	{
		var constructor = (key != null) ? clazz.GetConstructor(new []{key.GetType()}) : null;
		var component = ((constructor != null) ? constructor.Invoke(new []{key}) : Activator.CreateInstance(clazz)) as Component;
		component.Owner = this;
		_components.Add(CompoundKey(clazz, key), component);
		return component;
	}

	internal void BindComponents(Type implType, IEnumerable<Type> toTypes)
	{
		var impl = _components[implType];
		foreach (var toType in toTypes) _components.Add(CompoundKey(toType), impl);
	}

	public bool RemoveComponent(Type type, object key = null)
	{
		var compoundKey = CompoundKey(type, key);
		if (!_components.TryGetValue(compoundKey, out var component)) return false;
		
		component.Destroy();
		return _components.Remove(compoundKey);
	}

	public int RemoveComponents(Type type)
	{
		var components = _components
			.Where(e => IsCompoundKey(e.Key) && GetTypeFromKey(e.Key) == type)
			.ToList();
		
		foreach (var entry in components) RemoveComponent(type, GetObjectFromKey(entry.Key));
		return components.Count;
	}

	public void Init() {}

	public void Destroy() {}
}

}
