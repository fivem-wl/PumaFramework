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
using PumaFramework.Core.Container;
using PumaFramework.Core.Event;

namespace PumaFramework.Shared {

public class PumaContainer : Container
{
	EventManager _eventManager;
	
	
	public override void Init()
	{
		_eventManager = Resolver.ResolveReference<EventManager>();
		//Trace.Assert(_eventManager != null);

		base.Init();

		_eventManager.RegisterEventHandlers(this);
		foreach (var component in GetComponents()) _eventManager.RegisterEventHandlers(component);
	}

	public override void Destroy()
	{
		foreach (var component in GetComponents()) _eventManager.UnregisterEventHandlers(component);
		_eventManager.UnregisterEventHandlers(this);
		
		base.Destroy();
	}

	public override IComponent AddComponent(Type type, object key = null, IEnumerable<Type> bindTo = null)
	{
		var component = base.AddComponent(type, key, bindTo);
		_eventManager.RegisterEventHandlers(component);
		return component;
	}

	public override bool RemoveComponent(Type type, object key = null)
	{
		var component = GetComponent(type, key);
		if (component == null) return false;
		
		_eventManager.UnregisterEventHandlers(component);
		return base.RemoveComponent(type, key);
	}
}

}