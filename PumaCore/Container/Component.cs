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

public abstract class Component
{
	enum RegisterType
	{
		Component,
		Reference,
		External
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

	readonly IDictionary<object, RegisterEntry> _refs = new Dictionary<object, RegisterEntry>();
	
	
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
	
	void Add(RegisterType registerType, Type type, object obj, object key) =>
		_refs[CompoundKey(type, key)] = new RegisterEntry(registerType, obj);

	bool Remove(RegisterType registerType, Type type, object key)
	{
		var compoundKey = CompoundKey(type, key);
		if (!_refs.TryGetValue(compoundKey, out var entry) || entry.Type != registerType) return false;
		return _refs.Remove(compoundKey);
	}
	
	public object Get(Type type, object key = null) =>
		_refs.TryGetValue(CompoundKey(type, key), out var obj) ? obj.Object : Parent?.Get(type, key);
	
	#endregion Container

	#region Reference
	
	public void RegisterReferenceRaw(object obj, object key = null, params Type[] types)
	{
		Trace.Assert(types.Length > 0);
		foreach (var type in types)
		{
			Trace.Assert(type.IsInstanceOfType(obj));
			Add(RegisterType.Reference, type, obj, key);
		}
	}

	public bool UnregisterReference(object key = null, params Type[] types) =>
		types.Aggregate(false, (current, type) => current || Remove(RegisterType.Reference, type, key));

	public bool UnregisterReferences(params Type[] types)
	{
		return types.Aggregate(false, (current, type) => current || _refs
			.Where(e => (e.Value.Type == RegisterType.Reference))
			.Where(e => (GetTypeFromKey(e.Key) == type))
			.Aggregate(false, (current_, entry) => (current_ || UnregisterReference(GetObjectFromKey(entry.Key), type)))
		);
	}
	
	//public IList<ValueTuple<object, object>> GetAll(Type type)
	//{
	//	return _refs
	//		.Where(e => IsCompoundKey(e.Key) && GetTypeFromKey(e.Key) == type)
	//		.Select(e => new ValueTuple<object, object>(GetObjectFromKey(e.Key), e.Value.Object))
	//		.ToList();
	//}
//
	//protected ISet<object> GetAll()
	//{
	//	return new HashSet<object>(_refs.Values);
	//}
	
	#endregion Reference

	#region Component
	
	protected IEnumerable<Component> GetComponents() => _refs
		.Where(e => e.Value.Type == RegisterType.Component)
		.Select(e => (Component) e.Value.Object)
		.ToList();
	
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
		
		Add(RegisterType.Component, clazz, component, key);
		if (bindTo != null)
		{
			foreach (var toType in bindTo) Add(RegisterType.Reference, toType, component, key);
		}
		return component;
	}

	public virtual bool RemoveComponent(Type type, object key = null)
	{
		_refs.TryGetValue(CompoundKey(type, key), out var entry2);
		if (!_refs.TryGetValue(CompoundKey(type, key), out var entry) || entry.Type != RegisterType.Component) return false;

		var component = (Component) entry.Object;
		component.Destroy();
		
		_refs
			.Where(e => (e.Value.Object == component))
			.ToList()
			.ForEach(e => _refs.Remove(e.Key));
		
		return true;
	}

	public int RemoveComponents(Type type)
	{
		var components = _refs
			.Where(e => (e.Value.Type == RegisterType.Component))
			.Where(e => (GetTypeFromKey(e.Key) == type))
			.ToList();
		
		foreach (var entry in components) RemoveComponent(type, GetObjectFromKey(entry.Key));
		return components.Count;
	}
	
	#endregion Component
}

}
