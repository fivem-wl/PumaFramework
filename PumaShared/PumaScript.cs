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
using PumaFramework.Core.Event;
using PumaFramework.Shared.Event;

#if SERVER
using PumaFramework.Server;
#elif CLIENT
using PumaFramework.Client;
#endif

namespace PumaFramework.Shared {

public abstract class PumaScript : BaseScript
{
	public readonly FeatureContainer FeatureContainer;
	
	public readonly EventManager EventManager = new EventManager();


	protected PumaScript()
	{
		FeatureContainer = new FeatureContainer(this);
		
		FeatureContainer.RegisterReference<EventHandlerDictionary>(EventHandlers);
		FeatureContainer.RegisterReference<ExportDictionary>(Exports);
		FeatureContainer.RegisterReference<PlayerList>(Players);
		
		FeatureContainer.RegisterReference<PumaScript>(this);
		FeatureContainer.RegisterReference<FeatureContainer>(FeatureContainer);
		FeatureContainer.RegisterReference<EventManager>(EventManager);

		#if SERVER
			FxEventHandlerUtils.RegisterEventHandlers(EventHandlers, new ServerEventDispatcher(EventManager));
		#elif CLIENT
			FxEventHandlerUtils.RegisterEventHandlers(EventHandlers, new ClientEventDispatcher(EventManager));
			FxEventHandlerUtils.RegisterEventHandlers(EventHandlers, new GameEventDispatcher(EventManager));
		#endif
		
		EventManager.RegisterEventHandlers(this);
	}
	
	[PumaEventHandler(HandlerPriority.Monitor)]
	void OnResourceStart(ResourceStartEvent @event)
	{
		if (@event.IsCurrentResource) OnStart();
	}
	
	[PumaEventHandler(HandlerPriority.Bottom)]
	void OnResourceStop(ResourceStopEvent @event)
	{
		if (@event.IsCurrentResource) OnStop();
	}

	protected abstract void OnStart();
	
	protected abstract void OnStop();
}

}