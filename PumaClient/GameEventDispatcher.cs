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
	
	
	static readonly IDictionary<string, Action<EventManager, IList<dynamic>>> GameEventDispatchers = new Dictionary<string, Action<EventManager, IList<dynamic>>>()
	{
		{
			"CEventNetworkEntityDamage",
			(m, args) =>
			{
				var victim = Entity.FromHandle((int) args[0]);
				var attacker = Entity.FromHandle((int) args[1]);
				var isFatal = (int) args[3] == 1;
				var weaponInfoHash = (uint) args[4];
				var isMelee = (int) args[9] != 0;
				var damageType = (int) args[10];

				// Base event to dispatch
				m.DispatchEvent(new NetworkEntityDamageEvent(victim, attacker, isFatal, weaponInfoHash, isMelee, damageType));

				// More specific fatal events to dispatch
				if (isFatal)
				{
					#region Conditions
					var isAttackerPed = false;
					var isAttackerPlayer = false;
					var isVictimPed = false;
					var isVictimPlayer = false;
					var isVictimThisPlayer = false;
					// 攻击者是Ped
					if (attacker is Ped pedAttacker)
					{
						isAttackerPed = true;
						// 攻击者是Player
						if (pedAttacker.IsPlayer)
						{
							isAttackerPlayer = true;
						}
					}
					// 受害者是Ped
					if (victim is Ped pedVictim)
					{
						isVictimPed = true;
						// 受害者是Player
						if (pedVictim.IsPlayer)
						{
							isVictimPlayer = true;
							if (victim.ToPlayer() == Game.Player)
							{
								isVictimThisPlayer = true;
							}
						}
					}
					#endregion
					#region Dispatch
					if (isAttackerPlayer && isVictimPlayer)
					{
						m.DispatchEvent(new PlayerKillPlayerEvent(attacker.ToPlayer(), victim.ToPlayer(), weaponInfoHash, isMelee, damageType));
					}
					else if (isAttackerPlayer && isVictimPed)
					{
						m.DispatchEvent(new PlayerKillPedEvent(attacker.ToPlayer(), (Ped)victim, weaponInfoHash, isMelee, damageType));
					}
					else if (isAttackerPed && isVictimPlayer)
					{
						m.DispatchEvent(new PedKillPlayerEvent((Ped)attacker, victim.ToPlayer(), weaponInfoHash, isMelee, damageType));
					}
					else if (isVictimPed && isAttackerPed)
					{
						m.DispatchEvent(new PedKillPedEvent((Ped)attacker, (Ped)victim, weaponInfoHash, isMelee, damageType));
					}
					else
					{
						m.DispatchEvent(new EntityKillEntityEvent(attacker, victim, weaponInfoHash, isMelee, damageType));
					}

					if (isVictimThisPlayer)
					{
						m.DispatchEvent(new PlayerDeadEvent(victim.ToPlayer(), attacker, weaponInfoHash, isMelee, damageType));
					}
					#endregion
				}
				// More specific damage events to dispatch (todo)
				else
				{

				}

				
			}
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