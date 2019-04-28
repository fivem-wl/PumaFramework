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
	};
	
	[EventHandler("gameEventTriggered")]
	void OnGameEventTriggered(string name, IList<dynamic> args)
	{
		var @event = GameEventCreators[name]?.Invoke(args);
		if (@event != null) _eventManager.DispatchEvent(@event);
	}
}

}