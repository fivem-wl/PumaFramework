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
using PumaFramework.Core.Container;
using PumaFramework.Shared;

namespace Server {

public class ServerScript : PumaScript
{
	public ServerService Service { get; private set; }


	public ServerScript()
	{
		
	}

	protected override void OnStart()
	{
		Service = RootResolver.ConstructContainer<ServerService>();
		Debug.WriteLine("ServerScript loaded.");
	}
	
	protected override void OnStop()
	{
	}
}

}