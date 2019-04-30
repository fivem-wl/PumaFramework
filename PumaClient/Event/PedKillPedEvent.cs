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

namespace PumaFramework.Client.Event
{
	public class PedKillPedEvent : GameEvent
	{
		public readonly Ped Attacker;
		public readonly Ped Victim;
		public readonly uint WeaponInfoHash;
		public readonly bool IsMelee;
		public readonly int DamageType;

		public PedKillPedEvent(Ped attacker, Ped victim, uint weaponInfoHash, bool isMelee, int damageType)
		{
			Attacker = attacker;
			Victim = victim;
			WeaponInfoHash = weaponInfoHash;
			IsMelee = isMelee;
			DamageType = damageType;
		}
	}
}
