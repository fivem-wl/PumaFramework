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
using PumaFramework.Core.Event;
using PumaFramework.Server.Event;
using PumaFramework.Shared.Event;

namespace PumaFramework.Server {

class ServerEventDispatcher
{
	readonly EventManager _eventManager;
	
	
	public ServerEventDispatcher(EventManager eventManager)
	{
		_eventManager = eventManager;
	}
	
	[EventHandler("onResourceStart")]
	void OnResourceStart(string resourceName)
	{
		_eventManager.DispatchEvent(new ResourceStartEvent(resourceName));
	}

	[EventHandler("onResourceStop")]
	void OnResourceStop(string resourceName)
	{
		_eventManager.DispatchEvent(new ResourceStopEvent(resourceName));
	}

	[EventHandler("playerConnecting")]
	void OnPlayerConnecting([FromSource] Player player, string playerName, dynamic setKickReason, dynamic deferrals)
	{
		_eventManager.DispatchEvent(new PlayerConnectingEvent(player, playerName, setKickReason, deferrals));
	}
	
	[EventHandler("playerJoining")]
	void OnPlayerJoining([FromSource] Player player)
	{
		_eventManager.DispatchEvent(new PlayerJoiningEvent(player));
	}

	[EventHandler("playerDropped")]
	void OnPlayerDropped([FromSource] Player player, string reason)
	{
		_eventManager.DispatchEvent(new PlayerDroppedEvent(player, reason));
	}
}

}