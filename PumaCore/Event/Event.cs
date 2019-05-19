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
using System.Linq;
using System.Reflection;

namespace PumaFramework.Core.Event {

public abstract class Event
{
	static readonly IDictionary<Type, FieldInfo> CachedSourceFields = new Dictionary<Type, FieldInfo>();


	public object GetSource()
	{
		var thisType = GetType();
		if (!CachedSourceFields.TryGetValue(thisType, out var field))
		{
			field = thisType.GetRuntimeFields()
				.Where(f => !f.IsStatic && f.IsInitOnly)
				.SingleOrDefault(f => f.GetCustomAttribute<EventSourceAttribute>() != null);
			
			CachedSourceFields[thisType] = field;
		}

		return (field == null) ? null : field.GetValue(this);
	}
}

}