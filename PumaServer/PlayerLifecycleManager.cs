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
using PumaFramework.Core.Event;
using PumaFramework.Server.Event;
using PumaFramework.Shared;

namespace PumaFramework.Server {

public class PlayerLifecycleManager : PumaComponent
{
	readonly ISet<Type> _componentTypes = new HashSet<Type>();


	protected override void Start()
	{
		foreach (var attr in Parent.GetType().GetCustomAttributes<PlayerLifecycleComponentAttribute>())
		{
			_componentTypes.Add(attr.Type);
		}
	}

	protected override void Destroy()
	{
		foreach (var type in _componentTypes)
		{
			Parent.RemoveComponents(type);
		}
	}
	
	[PumaEventHandler(HandlerPriority.Monitor)]
	void OnPlayerJoining(PlayerJoiningEvent @event)
	{
		foreach (var type in _componentTypes)
		{
			Parent.AddComponent(type, @event.Player);
		}
	}
	
	[PumaEventHandler(HandlerPriority.Bottom)]
	void OnPlayerDropped(PlayerDroppedEvent @event)
	{
		foreach (var type in _componentTypes)
		{
			Parent.RemoveComponent(type, @event.Player);
		}
	}
}

}