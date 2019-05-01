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
using CitizenFX.Core.Native;

using PumaFramework.Client.Event.Game;
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
		if (!GameEventDispatchers.TryGetValue(name, out var dispatcher)) return;
		dispatcher.Invoke(_eventManager, args);
	}


	private static readonly IDictionary<string, Action<EventManager, IList<dynamic>>> GameEventDispatchers = new Dictionary<string, Action<EventManager, IList<dynamic>>>()
	{
		{
			"CEventNetworkEntityDamage",
			(m, args) =>
			{
				// Base event to dispatch
				var damageEvent = new NetworkEntityDamageEvent(
					Entity.FromHandle((int) args[0]),
					Entity.FromHandle((int) args[1]),
					(int) args[3] == 1,
					(uint) args[4],
					(int) args[9] != 0,
					(int) args[10]
				);
				m.DispatchEvent(damageEvent);

				// More specific fatal events to dispatch
				if (damageEvent.IsFatal)
				{
					var isAttackerPed = damageEvent.Attacker is Ped;
					var isAttackerPlayer = isAttackerPed && ((Ped) damageEvent.Attacker).IsPlayer;
					var isVictimPed = damageEvent.Victim is Ped;
					var isVictimPlayer = isVictimPed && ((Ped) damageEvent.Victim).IsPlayer;
					var isVictimThisPlayer = isVictimPlayer && (damageEvent.Victim.ToPlayer() == Game.Player);
					
					if (isAttackerPlayer && isVictimPlayer)	m.DispatchEvent(new PlayerKillPlayerEvent(damageEvent));
					else if (isAttackerPlayer && isVictimPed)	m.DispatchEvent(new PlayerKillPedEvent(damageEvent));
					else if (isAttackerPed && isVictimPlayer)	m.DispatchEvent(new PedKillPlayerEvent(damageEvent));
					else if (isVictimPed && isAttackerPed)		m.DispatchEvent(new PedKillPedEvent(damageEvent));
					else										m.DispatchEvent(new EntityKillEntityEvent(damageEvent));

					if (isVictimPlayer) m.DispatchEvent(new PlayerDeadEvent(damageEvent));
					if (isVictimThisPlayer) m.DispatchEvent(new ThisPlayerDeadEvent(damageEvent));
				}
				// More specific damage events to dispatch (todo)
				else
				{

				}

				
			}
		},

		{
			"CEventNetworkSignInStateChanged",
			(m, args) => m.DispatchEvent(new NetworkSignInStateChangedEvent())
		},
		{
			"CEventNetworkHostSession",
			(m, args) => m.DispatchEvent(new NetworkHostSessionEvent())
		},
		{
			"CEventNetworkStartSession",
			(m, args) => m.DispatchEvent(new NetworkStartSessionEvent())
		},

		{
			"CEventNetworkStartMatch",
			(m, args) => m.DispatchEvent(new NetworkStartMatchEvent())
		},

		{
			"CEventNetworkPlayerJoinScript",
			(m, args) => m.DispatchEvent(new NetworkPlayerJoinScriptEvent())
		},
		{
			"CEventNetworkPlayerLeftScript",
			(m, args) => m.DispatchEvent(new NetworkPlayerLeftScriptEvent())
		},

		{
			"CEventNetworkPlayerSpawn",
			(m, args) => m.DispatchEvent(new NetworkPlayerSpawnEvent())
		},

		{
			"CEventNetworkAttemptHostMigration",
			(m, args) => m.DispatchEvent(new NetworkAttemptHostMigrationEvent())
		},
		{
			"CEventNetworkHostMigration",
			(m, args) => m.DispatchEvent(new NetworkHostMigrationEvent())
		},

		{
			"CEventNetworkVehicleUndrivable",
			(m, args) =>
			{
				var vehicle = new Vehicle((int) args[0]);
				// Treat any unknown source as self source, such as set health to 0
				var causer = Entity.FromHandle((int) args[1]) ?? vehicle;
				var weaponInfoHash = (uint) (uint) args[2];
				m.DispatchEvent(new NetworkVehicleUndrivableEvent(vehicle, causer, weaponInfoHash));
			}
		},
	};
}

}