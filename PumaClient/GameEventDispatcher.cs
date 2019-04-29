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

using System.Collections.Generic;
using CitizenFX.Core;
using PumaFramework.Client.Event;
using PumaFramework.Core.Event;

namespace PumaFramework.Client {

public class GameEventDispatcher
{
	readonly EventManager _eventManager;
	
	
	public GameEventDispatcher(EventManager eventManager)
	{
		_eventManager = eventManager;
	}
	
	[EventHandler("gameEventTriggered")]
	void OnGameEventTriggered(string name, IList<dynamic> args)
	{
		GameEventDispatchers[name]?.Invoke(_eventManager, args);
	}
	
	
	static readonly IDictionary<string, System.Action<EventManager, IList<dynamic>>> GameEventDispatchers = new Dictionary<string, System.Action<EventManager, IList<dynamic>>>()
	{
		{
			"CEventNetworkEntityDamage",
			(m, args) => m.DispatchEvent(new NetworkEntityDamageEvent
			(
				Entity.FromHandle((int) args[0]), 
				Entity.FromHandle((int) args[1]), 
				(int) args[3] == 1,
				(uint) args[4],
				(int) args[9] != 0,
				(int) args[10]
			))
		},

		{
			"NetworkHostSessionEvent",
			(m, args) => m.DispatchEvent(new NetworkHostSessionEvent())
		},
		{
			"NetworkStartSessionEvent",
			(m, args) => m.DispatchEvent(new NetworkStartSessionEvent())
		},

		{
			"NetworkStartMatchEvent",
			(m, args) => m.DispatchEvent(new NetworkStartMatchEvent())
		},

		{
			"NetworkPlayerJoinScriptEvent",
			(m, args) => m.DispatchEvent(new NetworkPlayerJoinScriptEvent())
		},
		{
			"NetworkPlayerLeftScriptEvent",
			(m, args) => m.DispatchEvent(new NetworkPlayerLeftScriptEvent())
		},

		{
			"NetworkPlayerSpawnEvent",
			(m, args) => m.DispatchEvent(new NetworkPlayerSpawnEvent())
		},

		{
			"NetworkAttemptHostMigrationEvent",
			(m, args) => m.DispatchEvent(new NetworkAttemptHostMigrationEvent())
		},
		{
			"NetworkHostMigrationEvent",
			(m, args) => m.DispatchEvent(new NetworkHostMigrationEvent())
		},

		{
			"NetworkVehicleUndrivableEvent",
			(m, args) => m.DispatchEvent(new NetworkVehicleUndrivableEvent
			(
				new Vehicle((int) args[0]),
				Entity.FromHandle((int) args[1]),
				(uint) args[2]
			))
		},
	};
}

}