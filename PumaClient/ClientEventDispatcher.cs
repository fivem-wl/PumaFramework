using CitizenFX.Core;
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
}

}