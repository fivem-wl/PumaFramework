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

public class EventHandler : IComparable<EventHandler>
{
	public enum Priority
	{
		Bottom,
		Lowest,
		Low,
		Normal,
		High,
		Highest,
		Monitor,
	}


	static long _count = 0;


	readonly long _id;
	internal readonly Priority _priority;
	internal readonly Type _eventType;
	readonly Action<Event> _handler;


	public EventHandler(Type eventType, Priority priority, Action<Event> handler)
	{
		_id = Interlocked.Increment(ref _count);
		_priority = priority;
		_eventType = eventType;
		_handler = handler;
	}

	public int CompareTo(EventHandler other)
	{
		if (_priority == other._priority) return (_id < other._id) ? -1 : 1;
		return (_priority > other._priority) ? -1 : 1;
	}

	public void Handle(Event @event)
	{
		_handler(@event);
	}
}

}