/*
 * This file is part of PumaFramework.
 *
 * PumaFramework is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * PumaFramework is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public License
 * along with PumaFramework.  If not, see <https://www.gnu.org/licenses/>.
 */

using CitizenFX.Core;

namespace PumaFramework.Client.Event.Game {

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
		Attacker = attacker ?? victim;		// Treat any unknown source as self source, such as set health to 0
		IsFatal = isFatal;
		WeaponInfoHash = weaponInfoHash;
		IsMelee = isMelee;
		DamageType = damageType;
	}
}

}