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