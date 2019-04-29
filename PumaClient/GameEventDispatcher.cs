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