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
using System.Threading;

namespace PumaFramework.Core.Event {

class EventHandler : IComparable<EventHandler>
{
	static long _count = 0;


	internal readonly long Id;
	internal readonly HandlerPriority Priority;
	internal readonly Type EventType;
	internal readonly Action<Event> Handler;


	public EventHandler(Type eventType, HandlerPriority priority, Action<Event> handler)
	{
		Id = Interlocked.Increment(ref _count);
		Priority = priority;
		EventType = eventType;
		Handler = handler;
	}

	public int CompareTo(EventHandler other)
	{
		if (Priority == other.Priority) return (Id < other.Id) ? -1 : 1;
		return (Priority > other.Priority) ? -1 : 1;
	}

	public void Handle(Event @event)
	{
		Handler(@event);
	}
}

}