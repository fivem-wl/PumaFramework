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
using CitizenFX.Core;
using PumaFramework.Client.Event;
using PumaFramework.Core.Event;
using PumaFramework.Shared.Event;

namespace PumaFramework.Client {

class ClientEventDispatcher
{
	readonly EventManager _eventManager;
	
	
	public ClientEventDispatcher(EventManager eventManager)
	{
		_eventManager = eventManager;
	}
	
	[EventHandler("onClientResourceStart")]
	void OnResourceStart(string resourceName)
	{
		_eventManager.DispatchEvent(new ResourceStartEvent(resourceName));
	}

	[EventHandler("onClientResourceStop")]
	void OnResourceStop(string resourceName)
	{
		_eventManager.DispatchEvent(new ResourceStopEvent(resourceName));
	}
	
	static readonly IDictionary<string, Func<IList<dynamic>, GameEvent>> GameEventCreators = new Dictionary<string, Func<IList<dynamic>, GameEvent>>()
	{
		{
			"CEventNetworkEntityDamage",
			args => new NetworkEntityDamageEvent
			(
				Entity.FromHandle((int) args[0]), 
				Entity.FromHandle((int) args[1]), 
				(int) args[3] == 1,
				(uint) args[4],
				(int) args[9] != 0,
				(int) args[10]
			)
		},

		{
			"NetworkHostSessionEvent",
			args => new NetworkHostSessionEvent()
		},
		{
			"NetworkStartSessionEvent",
			args => new NetworkStartSessionEvent()
		},

		{
			"NetworkStartMatchEvent",
			args => new NetworkStartMatchEvent()
		},

		{
			"NetworkPlayerJoinScriptEvent",
			args => new NetworkPlayerJoinScriptEvent()
		},
		{
			"NetworkPlayerLeftScriptEvent",
			args => new NetworkPlayerLeftScriptEvent()
		},

		{
			"NetworkPlayerSpawnEvent",
			args => new NetworkPlayerSpawnEvent()
		},

		{
			"NetworkAttemptHostMigrationEvent",
			args => new NetworkAttemptHostMigrationEvent()
		},
		{
			"NetworkHostMigrationEvent",
			args => new NetworkHostMigrationEvent()
		},

		{
			"NetworkVehicleUndrivableEvent",
			args => new NetworkVehicleUndrivableEvent
			(
				new Vehicle((int) args[0]),
				Entity.FromHandle((int) args[1]),
				(uint) args[2]
			)
		},
	};
	
	[EventHandler("gameEventTriggered")]
	void OnGameEventTriggered(string name, IList<dynamic> args)
	{
		var @event = GameEventCreators[name]?.Invoke(args);
		if (@event != null) _eventManager.DispatchEvent(@event);
	}
}

}