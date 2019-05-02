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

/// <summary>
///     NetworkVehicleUndrivableEvent
/// </summary>
class NetworkVehicleUndrivableEvent : GameEvent
{
	public readonly Entity Attacker;
	public readonly Vehicle Vehicle;
	public readonly uint WeaponInfohash;


	/// <summary>
	///     ctor.
	/// </summary>
	/// <param name="vehicle"></param>
	/// <param name="attacker"></param>
	/// <param name="weaponInfoHash"></param>
	public NetworkVehicleUndrivableEvent(Vehicle vehicle, Entity attacker, uint weaponInfoHash)
	{
		Vehicle = vehicle;
		Attacker = attacker;
		WeaponInfohash = weaponInfoHash;
	}
}

}