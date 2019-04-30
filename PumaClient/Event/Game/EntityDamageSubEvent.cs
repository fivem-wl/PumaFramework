namespace PumaFramework.Client.Event.Game {

public abstract class EntityDamageSubEvent : GameEvent
{
	public readonly NetworkEntityDamageEvent SourceEvent;
	
	public uint WeaponInfoHash	=> SourceEvent.WeaponInfoHash;
	public bool IsMelee			=> SourceEvent.IsMelee;
	public int DamageType		=> SourceEvent.DamageType;

	
	protected EntityDamageSubEvent(NetworkEntityDamageEvent source)
	{
		SourceEvent = source;
	}
}

}