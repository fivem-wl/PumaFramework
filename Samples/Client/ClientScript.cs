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

using CitizenFX.Core;
using PumaFramework.Client.Event;
using PumaFramework.Client.Event.Game;
using PumaFramework.Client.Event.Resource;
using PumaFramework.Core.Event;
using PumaFramework.Shared;
using PumaFramework.Shared.Event;

namespace Client {

public class ClientScript : PumaScript
{
	public ClientScript()
	{
		
	}

	[PumaEventHandler]
	void OnResourceStart(ResourceStartEvent @event)
	{
		Debug.WriteLine($"[ResourceStartEvent]{@event.ResourceName}");
	}
	
	[PumaEventHandler]
	void OnClientMapStrat(ClientMapStartEvent @event)
	{
		Debug.WriteLine($"[ClientMapStartEvent]{@event.ResourceName}");
		Exports["spawnmanager"].setAutoSpawn(true);
		Exports["spawnmanager"].forceRespawn();	
	}
	
	[PumaEventHandler]
	void OnThisPlayerSpawn(ThisPlayerSpawnedEvent @event)
	{
		Debug.WriteLine($"[OnThisPlayerSpawn]{Game.Player.Name}, " +
		                $"{@event.Model}, {@event.Position}, {@event.Heading}");
	}

	[PumaEventHandler]
	void OnPlayerDead(PlayerDeadEvent @event)
	{
		Debug.WriteLine($"[OnPlayerDead]{@event.Victim.Handle}, {@event.Attacker.Handle}, " +
		                $"{@event.WeaponInfoHash}, {@event.IsMelee}, {@event.DamageType}");
	}

	[PumaEventHandler]
	void OnPlayerKillNpc(PlayerKillNpcEvent @event)
	{
		Debug.WriteLine($"[OnPlayerKillNpc]{@event.Attacker.Handle}, {@event.Victim.Handle}, " +
		                $"{@event.WeaponInfoHash}, {@event.IsMelee}, {@event.DamageType}");
	}

	[PumaEventHandler]
	void OnNetworkPlayerLeftScript(NetworkPlayerLeftScriptEvent @event)
	{
		Debug.WriteLine($"OnNetworkPlayerLeftScript NetworkPlayerLeftScriptEvent");
	}

	[PumaEventHandler]
	void OnNetworkHostSession(NetworkHostSessionEvent @event)
	{
		Debug.WriteLine($"NetworkHostSessionEvent");
	}

	[PumaEventHandler]
	void OnNetworkPlayerSpawn(NetworkPlayerSpawnEvent @event)
	{
		Debug.WriteLine($"NetworkPlayerSpawnEvent");
	}

	protected override void OnStart()
	{
		
	}
	
	protected override void OnStop()
	{
		
	}
}

}