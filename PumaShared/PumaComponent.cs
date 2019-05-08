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
using PumaFramework.Core;
using PumaFramework.Core.Container;
using PumaFramework.Core.Event;

namespace PumaFramework.Shared {

public class PumaComponent : Component
{
	EventManager _eventManager;
	
	
	protected override void Start()
	{
		_eventManager = this.Get<EventManager>();
		Trace.Assert(_eventManager != null);

		base.Start();

		_eventManager.RegisterEventHandlers(this);
		foreach (var component in GetComponents()) _eventManager.RegisterEventHandlers(component);
	}

	protected override void Destroy()
	{
		foreach (var component in GetComponents()) _eventManager.UnregisterEventHandlers(component);
		_eventManager.UnregisterEventHandlers(this);
		
		base.Destroy();
	}

	public override Component AddComponent(Type type, object key = null, IEnumerable<Type> bindTo = null)
	{
		var component = base.AddComponent(type, key, bindTo);
		_eventManager.RegisterEventHandlers(component);
		return component;
	}

	public override bool RemoveComponent(Type type, object key = null)
	{
		var component = Get(type, key);
		if (component == null) return false;
		
		_eventManager.UnregisterEventHandlers(component);
		return base.RemoveComponent(type, key);
	}
}

}