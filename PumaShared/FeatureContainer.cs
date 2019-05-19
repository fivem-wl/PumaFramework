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
using System.Linq;
using System.Reflection;
using CitizenFX.Core;
using PumaFramework.Core.Container;
using PumaFramework.Core.Event;
using PumaFramework.Shared.Event;

namespace PumaFramework.Shared {

#if SERVER
[ChildComponent(typeof(PumaPlayerLifecycleManager))]
#elif CLIENT
#endif
public sealed class FeatureContainer : PumaComponent
{
	public readonly PumaScript PumaScript;
	
	
	internal FeatureContainer(PumaScript script)
	{
		PumaScript = script;
		this.RegisterReference<PumaScript>(PumaScript);
		
		script.EventManager.RegisterEventHandlers(this);
		
		var featureClasses = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(a => a.GetTypes())
			.Where(t => t.IsSubclassOf(typeof(Feature)));
		
		foreach (var clazz in featureClasses)
		{
			NewComponent(clazz);
			Debug.WriteLine($"[Puma] Feature {clazz} loaded.");
		}
	}
	
	[PumaEventHandler(HandlerPriority.Monitor)]
	void OnResourceStart(ResourceStartEvent @event)
	{
		if (!@event.IsCurrentResource) return;
		
		Awake();
		Start();
	}
	
	[PumaEventHandler(HandlerPriority.Bottom)]
	void OnResourceStop(ResourceStopEvent @event)
	{
		if (!@event.IsCurrentResource) return;
		Destroy();
	}
}

}