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

using System;
using CitizenFX.Core;

namespace PumaFramework.Server
{

public static class PlayerExtensions
{
	/// <summary>
	/// Get Player ServerId
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentException"></exception>
	public static int GetServerId(this Player player)
	{
		var succeed = int.TryParse(player.Handle, out var serverId);
		if (!succeed) throw new ArgumentException();
		return serverId;
	}

	/// <summary>
	/// Get Player RGSC License
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public static string GetLicense(this Player player)
	{
		return player.Identifiers["license"];
	}
}

}