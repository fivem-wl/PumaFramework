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
using System.Linq.Expressions;
using System.Reflection;
using CitizenFX.Core;

namespace PumaFramework.Shared {

public static class EventHandlerUtils
{
	public static IEnumerable<Delegate> RegisterEventHandlers(EventHandlerDictionary handlerDictionary, object obj)
	{
		return obj.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
			.Where(method => method.GetCustomAttribute<EventHandlerAttribute>() != null)
			.Select(method =>
			{
				var eventName = method.GetCustomAttribute<EventHandlerAttribute>().Name;
				var parameters = method.GetParameters().Select(p => p.ParameterType).ToArray();
				var actionType = Expression.GetDelegateType(parameters.Concat(new[] { typeof(void) }).ToArray());
				
				var @delegate = Delegate.CreateDelegate(actionType, obj, method.Name);
				handlerDictionary[eventName] += @delegate;
				return @delegate;
			})
			.ToArray();
	}
}

}