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
using System.Collections.Generic;
using CitizenFX.Core;
using PumaFramework.Core.Container;
using static CitizenFX.Core.Native.API;

namespace PumaFramework.Shared {

public abstract class PumaScript : BaseScript
{
	public new PlayerList Players => base.Players;
	
	public readonly Resolver RootResolver = new Resolver();


	protected PumaScript()
	{
		RootResolver.RegisterReference<PumaScript>(this);
		RootResolver.RegisterReference<EventHandlerDictionary>(EventHandlers);

		#if SERVER
			EventHandlers["onResourceStart"] += new Action<string>(OnResourceStart);
			EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop);
		#elif CLIENT
			EventHandlers["onClientResourceStart"] += new Action<string>(OnResourceStart);
			EventHandlers["onClientResourceStop"] += new Action<string>(OnResourceStop);
		#endif
	}
	
	void OnResourceStart(string resourceName)
	{
		if (GetCurrentResourceName() != resourceName) return;
		OnStart();
	}
	
	void OnResourceStop(string resourceName)
	{
		if (GetCurrentResourceName() != resourceName) return;
		OnStop();
	}

	protected abstract void OnStart();
	
	protected abstract void OnStop();
}

}