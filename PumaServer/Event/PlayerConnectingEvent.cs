using CitizenFX.Core;

namespace PumaFramework.Server.Event {

public class PlayerConnectingEvent : PlayerEvent
{
	public readonly string PlayerName;
	public readonly dynamic SetKickReason;
	public readonly dynamic Deferrals;
	
	
	public PlayerConnectingEvent(Player player, string playerName, dynamic setKickReason, dynamic deferrals) : base(player)
	{
		PlayerName = playerName;
		SetKickReason = setKickReason;
		Deferrals = deferrals;
	}
}

}