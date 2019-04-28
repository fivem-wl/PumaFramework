using CitizenFX.Core;

namespace PumaFramework.Client.Event {

public class NetworkEntityDamageEvent : GameEvent
{
	public readonly Entity Victim;
	public readonly Entity Attacker;
	public readonly bool IsFatal;
	public readonly uint WeaponInfoHash;
	public readonly bool IsMelee;
	public readonly int DamageType;

	
	public NetworkEntityDamageEvent(Entity victim, Entity attacker, bool isFatal, uint weaponInfoHash, bool isMelee, int damageType)
	{
		Victim = victim;
		Attacker = attacker;
		IsFatal = isFatal;
		WeaponInfoHash = weaponInfoHash;
		IsMelee = isMelee;
		DamageType = damageType;
	}
}

}