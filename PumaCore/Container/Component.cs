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
using Microsoft.Collections.Extensions;
using static PumaFramework.Core.Container.CompoundKeyUtils;

namespace PumaFramework.Core.Container {

public abstract class Component
{
	[Flags]
	enum RegisterType
	{
		Component	= 1,
		Reference	= 2,
		External	= 4,
	}

	class RegisterEntry
	{
		internal readonly RegisterType Type;
		internal readonly object Object;
		
		public RegisterEntry(RegisterType type, object obj)
		{
			Type = type;
			Object = obj;
		}
	}
	
	
	public Component Parent { get; private set; }

	public bool Fenced { get; }

	readonly MultiValueDictionary<object, RegisterEntry> _refs = new MultiValueDictionary<object, RegisterEntry>();
	
	
	protected Component()
	{
		Fenced = GetType().GetCustomAttribute<FenceAttribute>() != null;
		foreach (var attr in GetType().GetCustomAttributes<ChildComponentAttribute>())
		{
			var component = NewComponent(attr.Implementation, null, attr.BindTo);
		}
	}

	#region Lifecycle

	protected virtual void Awake()
	{
		foreach (var component in GetComponents())
		{
			component.Awake();
		}
	}

	protected virtual void Start()
	{
		foreach (var component in GetComponents())
		{
			component.Start();
		}

		foreach (var attr in GetType().GetCustomAttributes<RequiredAttribute>())
		{
			this.Require(attr.ObjectType);
		}
	}

	protected virtual void Destroy()
	{
		foreach (var attr in GetType().GetCustomAttributes<ChildComponentAttribute>().Reverse())
		{
			var component = (Component) Get(attr.Implementation);
			component.Destroy();
		}
	}
	
	#endregion Lifecycle
	
	#region Container

	bool Add(RegisterType registerType, Type type, object key, object obj)
	{
		var compoundKey = CompoundKey(type, key);
		if (_refs.TryGetValue(compoundKey, out var entries) &&
			(entries.Any(e => e.Type == registerType) || entries.Any(e => e.Object == obj))
		) return false;

		_refs.Add(compoundKey, new RegisterEntry(registerType, obj));
		return true;
	}

	bool Remove(RegisterType registerType, Type type, object key)
	{
		var compoundKey = CompoundKey(type, key);
		if (!_refs.TryGetValue(compoundKey, out var entries)) return false;

		var entry = entries.LastOrDefault(e => (e.Type & registerType) != 0);
		return (entry != null && _refs.Remove(compoundKey, entry));
	}
	
	bool Remove(RegisterType registerType, Type type, object key, object obj)
	{
		var compoundKey = CompoundKey(type, key);
		if (!_refs.TryGetValue(compoundKey, out var entries)) return false;

		var entry = entries.SingleOrDefault(e => ((e.Type & registerType) != 0 && e.Object == obj));
		return (entry != null && _refs.Remove(compoundKey, entry));
	}

	bool RemoveAll(RegisterType registerType, Type type, object key)
	{
		var compoundKey = CompoundKey(type, key);
		if (!_refs.TryGetValue(compoundKey, out var entries)) return false;
		
		return entries
			.Where(e => (e.Type & registerType) != 0)
			.ToList()
			.Aggregate(false, (current, entry) => current || _refs.Remove(compoundKey, entry));
	}
	
	bool RemoveAll(RegisterType registerType, object obj)
	{
		return _refs
			.SelectMany(e => e.Value
				.Where(entry => entry.Object == obj)
				.Select(entry => new ValueTuple<object, RegisterEntry>(e.Key, entry))
			)
			.ToList()
			.Aggregate(false, (current, tuple) => current || _refs.Remove(tuple.Item1, tuple.Item2));
	}

	object Get(RegisterType registerType, Type type, object key)
	{
		var compoundKey = CompoundKey(type, key);
		if (!_refs.TryGetValue(compoundKey, out var entries)) return null;
		return entries.SingleOrDefault(e => ((e.Type & registerType) != 0))?.Object;
	}
	
	IList<object> GetAll(RegisterType registerType, Type type)
	{
		return _refs
			.Where(e => GetTypeFromKey(e.Key) == type)
			.SelectMany(e => e.Value)
			.Where(e => (e.Type & registerType) != 0)
			.Select(e => e.Object)
			.ToList();
	}

	public object Get(Type type, object key = null) =>
		_refs.TryGetValue(CompoundKey(type, key), out var entries) ? entries.Last().Object : Parent?.Get(type, key);
	
	public IList<object> GetAll(Type type, object key = null)
	{
		var objs = (Parent != null ? Parent.GetAll(type, key) : new List<object>());
		if (!_refs.TryGetValue(CompoundKey(type, key), out var entries)) return objs;
		return objs.Concat(entries.Select(e => e.Object)).ToList();
	}

	#endregion Container

	#region Reference
	
	public void RegisterReferenceRaw(object obj, object key = null, params Type[] types)
	{
		Trace.Assert(types.Length > 0);
		foreach (var type in types)
		{
			Trace.Assert(type.IsInstanceOfType(obj));
			Add(RegisterType.Reference, type, key, obj);
		}
	}

	public bool UnregisterReference(object key = null, params Type[] types) =>
		types.Aggregate(false, (current, type) => current || Remove(RegisterType.Reference, type, key));
	
	public bool UnregisterReferences(params Type[] types)
	{
		return types.Aggregate(false, (current, type) => current || _refs
			.Where(e => GetTypeFromKey(e.Key) == type)
			.SelectMany(e => e.Value
				.Where(entry => entry.Type == RegisterType.Reference)
				.Select(entry => new ValueTuple<object, RegisterEntry>(e.Key, entry))
			)
			.Aggregate(false, (curr, tuple) => (curr || _refs.Remove(tuple.Item1, tuple.Item2)))
		);
	}

	#endregion Reference

	#region Component
	
	protected IEnumerable<Component> GetComponents()
	{
		return _refs
			.SelectMany(e => e.Value.Where(entry => entry.Type == RegisterType.Component))
			.Select(e => (Component) e.Object)
			.ToList();
	}

	public virtual Component AddComponent(Type type, object key = null, IEnumerable<Type> bindTo = null)
	{
		var component = NewComponent(type, key, bindTo);
		component.Awake();
		component.Start();
		return component;
	}
	
	protected Component NewComponent(Type clazz, object key = null, IEnumerable<Type> bindTo = null)
	{
		var constructor = (key != null) ? clazz.GetConstructor(new []{key.GetType()}) : null;
		var component = (Component) ((constructor != null) ? constructor.Invoke(new []{key}) : Activator.CreateInstance(clazz));
		component.Parent = this;
		
		Add(RegisterType.Component, clazz, key, component);
		if (bindTo != null)
		{
			foreach (var toType in bindTo) Add(RegisterType.Reference, toType, key, component);
		}
		return component;
	}

	public virtual bool RemoveComponent(Type type, object key = null)
	{
		var component = (Component) Get(RegisterType.Component, type, key);
		if (component == null) return false;
		
		component.Destroy();
		return RemoveAll(RegisterType.Component | RegisterType.Reference | RegisterType.External, component);
	}

	public bool RemoveComponents(Type type)
	{
		return _refs
			.Where(e => GetTypeFromKey(e.Key) == type)
			.SelectMany(e => e.Value
				.Where(entry => entry.Type == RegisterType.Component)
				.Select(entry => GetObjectFromKey(e.Key))
			)
			.ToList()
			.Aggregate(false, (current, key) => current || RemoveComponent(type, key));
	}
	
	#endregion Component
}

}
