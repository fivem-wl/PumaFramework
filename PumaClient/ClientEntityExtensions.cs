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
using CitizenFX.Core.Native;

namespace PumaFramework.Client {

public static class ClientEntityExtensions
{
	/// <summary>
	/// Convert <seealso cref="Entity"/> to <seealso cref="Player"/>.
	/// </summary>
	/// <param name="entity"></param>
	/// <returns><seealso cref="Player"/> if converted successfully; otherwise, null.</returns>
	public static Player ToPlayer(this Entity entity)
		=> (entity is Ped) ? new Player(API.NetworkGetPlayerIndexFromPed(entity.Handle)) : null;

	/// <summary>
	///	Try convert <seealso cref="Entity"/> to <seealso cref="Player"/>.
	/// </summary>
	/// <param name="entity"></param>
	/// <param name="player"></param>
	/// <returns>true if converted successfully; otherwise, false.</returns>
	public static bool TryToPlayer(this Entity entity, out Player player)
		=> (player = entity.ToPlayer()) != null;
}

}