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

namespace PumaFramework.Core.Container {

public static class CompoundKeyUtils
{
	public static object CompoundKey(Type type, object key = null) =>
		(key == null) ? type : (object) new ValueTuple<Type, object>(type, key);
	
	public static object CompoundKey<T>(object key = null) where T : class, IComponent =>
		CompoundKey(typeof(T), key);

	public static bool IsCompoundKey(object key) =>
		key is ValueTuple<Type, object>;

	public static Type GetTypeFromKey(object key) =>
		(key is ValueTuple<Type, object> tuple) ? tuple.Item1 : key as Type;
	
	public static object GetObjectFromKey(object key) =>
		(key is ValueTuple<Type, object> tuple) ? tuple.Item2 : null;
}

}