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
using PumaFramework.Core.Container;

namespace PumaFramework.Core.Event {

public class EventManager
{
	static object GetEventSource(object eventHandlerObject)
	{
		var field = eventHandlerObject.GetType().GetFieldsEx(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(f => !f.IsStatic)
			.Where(f => f.IsInitOnly)
			.SingleOrDefault(f => f.GetCustomAttribute<EventSourceAttribute>() != null);
		return (field == null) ? null : field.GetValue(eventHandlerObject);
	}
	
	
	readonly IDictionary<object, SortedSet<EventHandler>> _eventHandlers = new Dictionary<object, SortedSet<EventHandler>>();
	readonly IDictionary<object, IEnumerable<EventHandler>> _objectEventHandlers = new Dictionary<object, IEnumerable<EventHandler>>();
	
	
	public EventManager()
	{
		
	}

	EventHandler RegisterEventHandler(Type eventType, object source, HandlerPriority priority, Action<Event> callback)
	{
		if (!_eventHandlers.TryGetValue(eventType, out var handlers))
		{
			handlers = new SortedSet<EventHandler>();
			_eventHandlers[eventType] = handlers;
		}

		var handler = new EventHandler(eventType, source, priority, callback);
		handlers.Add(handler);
		return handler;
	}

	public bool RegisterEventHandlers(object obj)
	{
		if (_objectEventHandlers.ContainsKey(obj)) return false;

		var source = GetEventSource(obj);
		var handlers = obj.GetType().GetMethodsEx(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(method => method.GetCustomAttribute<PumaEventHandlerAttribute>() != null)
			.Where(method => !method.GetCustomAttribute<PumaEventHandlerAttribute>().BindSource || source != null)
			.Where(method => method.GetParameters().Length == 1)
			.Where(method => method.GetParameters()[0].ParameterType.IsSubclassOf(typeof(Event)))
			.Select(method => RegisterEventHandler(
				method.GetParameters()[0].ParameterType,
				source,
				method.GetCustomAttribute<PumaEventHandlerAttribute>().Priority,
				@event => method.Invoke(obj, new[] {@event})
			))
			.ToList();
		
		_objectEventHandlers[obj] = handlers;
		return true;
	}

	bool UnregisterEventHandler(EventHandler handler)
	{
		return _eventHandlers.TryGetValue(handler.EventType, out var set) && set.Remove(handler);
	}
	
	public bool UnregisterEventHandlers(object obj)
	{
		if (!_objectEventHandlers.TryGetValue(obj, out var handlers)) return false;

		var success = handlers.Aggregate(true, (current, handler) => current && UnregisterEventHandler(handler));
		success &= _objectEventHandlers.Remove(obj);
		return success;
	}
	
	void DispatchEvent(Type eventType, Event @event)
	{
		if (!_eventHandlers.TryGetValue(eventType, out var handlerSet)) return;

		var isInterruptable = @event is IInterruptable;
		var source = @event.GetSource();

		var handlers = (source == null) ? handlerSet.ToList() : handlerSet.Where(h => (h.Source == null) || h.Source == source).ToList();
		foreach (var handler in handlers)
		{
			if (isInterruptable && (@event as IInterruptable).IsInterrupted()) return;
			handler.Handle(@event);
		}
	}
	
	public void DispatchEvent(Event @event)
	{
		DispatchEvent(@event.GetType(), @event);
	}
	
	public void DispatchEvent(Event @event, params Type[] types)
	{
		foreach (var type in types) DispatchEvent(type, @event);
	}
}

}