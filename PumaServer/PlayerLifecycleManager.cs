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
using PumaFramework.Core.Container;
using PumaFramework.Core.Event;
using PumaFramework.Server.Event;

namespace PumaFramework.Server {

public class PlayerLifecycleManager : Component
{
	readonly ISet<Type> _componentTypes = new HashSet<Type>();
	
	
	public override void Init()
	{
		foreach (PlayerLifecycleComponentAttribute attr in Owner.GetType().GetCustomAttributes(typeof(PlayerLifecycleComponentAttribute)))
		{
			_componentTypes.Add(attr.Type);
		}
	}

	public override void Destroy()
	{
		foreach (var type in _componentTypes)
		{
			Owner.RemoveComponents(type);
		}
	}
	
	[PumaEventHandler(HandlerPriority.Monitor)]
	void OnPlayerJoining(PlayerJoiningEvent @event)
	{
		foreach (var type in _componentTypes)
		{
			Owner.AddComponent(type, @event.Player);
		}
	}
	
	[PumaEventHandler(HandlerPriority.Bottom)]
	void OnPlayerDropped(PlayerDroppedEvent @event)
	{
		foreach (var type in _componentTypes)
		{
			Owner.RemoveComponent(type, @event.Player);
		}
	}
}

}