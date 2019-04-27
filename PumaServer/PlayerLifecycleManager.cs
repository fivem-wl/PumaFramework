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
using CitizenFX.Core;
using PumaFramework.Core.Container;

namespace PumaFramework.Server {

public class PlayerLifecycleManager : Component
{
	readonly ISet<Type> _componentTypes = new HashSet<Type>();
	readonly IList<(string, Delegate)> _eventHandlers = new List<(string, Delegate)>();
	
	
	public override void Init()
	{
		var eventHandlers = this.ResolveReference<EventHandlerDictionary>();

		void AddEventHandler(string @event, Delegate handler)
		{
			eventHandlers[@event] += handler;
			_eventHandlers.Add(new ValueTuple<string, Delegate>(@event, handler));
		}
		
		AddEventHandler("playerJoining", new Action<Player>(OnPlayerJoining));
		AddEventHandler("playerDropped", new Action<Player, string>(OnPlayerDropped));

		foreach (PlayerLifecycleComponentAttribute attr in Owner.GetType().GetCustomAttributes(typeof(PlayerLifecycleComponentAttribute)))
		{
			_componentTypes.Add(attr.Type);
		}
	}

	public override void Destroy()
	{
		var eventHandlers = this.ResolveReference<EventHandlerDictionary>();
		foreach (var (@event, @delegate) in _eventHandlers) eventHandlers[@event] -= @delegate;
		
		foreach (var type in _componentTypes)
		{
			Owner.RemoveComponents(type);
		}
	}

	void OnPlayerJoining([FromSource] Player player)
	{
		foreach (var type in _componentTypes)
		{
			Owner.AddComponent(type, player);
		}
	}
	
	void OnPlayerDropped([FromSource] Player player, string reason)
	{
		foreach (var type in _componentTypes)
		{
			Owner.RemoveComponent(type, player);
		}
	}
}

}