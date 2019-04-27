using CitizenFX.Core;

namespace PumaFramework.Server.Event {

public abstract class PlayerEvent : Core.Event.Event
{
	public readonly Player Player;
	
	
	protected PlayerEvent(Player player)
	{
		Player = player;
	}
}

}