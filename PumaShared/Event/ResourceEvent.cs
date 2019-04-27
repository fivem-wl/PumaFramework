using static CitizenFX.Core.Native.API;

namespace PumaFramework.Shared.Event {

public class ResourceEvent : Core.Event.Event
{
	public readonly string ResourceName;

	public bool IsCurrentResource => (ResourceName == GetCurrentResourceName());
	
	
	public ResourceEvent(string resourceName)
	{
		ResourceName = resourceName;
	}
}

}