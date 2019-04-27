using CitizenFX.Core;

namespace PumaFramework.Server.Event {

public class PlayerDroppedEvent : PlayerEvent
{
	public readonly string Reason;


	public PlayerDroppedEvent(Player player, string reason) : base(player)
	{
		Reason = reason;
	}
}

}