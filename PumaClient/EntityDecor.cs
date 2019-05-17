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
using CitizenFX.Core.Native;

namespace PumaFramework.Client
{

/// <summary>
/// GTA V Decor, usage: https://forum.fivem.net/t/adding-sync-to-native-functions/112935/6
/// </summary>
public class EntityDecor
{
	
	readonly Entity _entity;

	public EntityDecor(Entity entity)
	{
		_entity = entity;
	}
	
	public enum Type
	{
		Float = 1,
		Bool = 2,
		Int = 3
		// Time = 5
	}
	
	public static bool Has(Entity entity, string propertyName) => API.DecorExistOn(entity.Handle, propertyName);

	public static void Register(string propertyName, Type type)
	{
		try
		{
			API.DecorRegister(propertyName, (int) type);
		}
		catch (Exception e)
		{
			Debug.WriteLine(@"[CRITICAL] A critical bug in one of your scripts was detected. Unable to set or register a decorator's value because another resource has already registered 1.5k or more decorators.");
			Debug.WriteLine($"Error Location: {e.StackTrace}\nError info: {e.Message}");
			throw new EntityDecorRegisterPropertyFailedException();
		}
	}
	
	public static void Register<T>(string propertyName)
	{
		var type = typeof(T);
		if (type == typeof(int)) 		Register(propertyName, Type.Int);
		else if(type == typeof(float)) 	Register(propertyName, Type.Float);
		else if(type == typeof(bool)) 	Register(propertyName, Type.Bool);
		else throw new EntityDecorUndefinedTypeException("Supported types: int, float and bool");
	}

	public static bool Remove(Entity entity, string propertyName) => API.DecorRemove(entity.Handle, propertyName);

	public static bool IsRegisteredAsType(string propertyName, Type type) => API.DecorIsRegisteredAsType(propertyName, (int) type);
	
	public static void Set<T>(Entity entity, string propertyName, T value) where T : struct
	{
		var handle = entity.Handle;
		switch (value)
		{
			case int i:
				API.DecorSetInt(handle, propertyName, i);
				break;
			case float f:
				API.DecorSetFloat(handle, propertyName, f);
				break;
			case bool b:
				API.DecorSetBool(handle, propertyName, b);
				break;
			default: throw new EntityDecorUndefinedTypeException("Supported types: int, float and bool");
		}
	}

	public static T Get<T>(Entity entity, string propertyName) where T : struct
	{
		if (!Has(entity, propertyName)) throw new EntityDecorUnregisteredPropertyException();

		var type = typeof(T);
		var handle = entity.Handle;
		if (type == typeof(int)) 	return (T) (object) API.DecorGetInt(handle, propertyName);
		if (type == typeof(float)) 	return (T) (object) API.DecorGetFloat(handle, propertyName);
		if (type == typeof(bool)) 	return (T) (object) API.DecorGetBool(handle, propertyName);
		throw new EntityDecorUndefinedTypeException("Supported types: int, float and bool");
	}
	
	public bool Remove(string propertyName) => Remove(_entity, propertyName);

	public void Set<T>(string propertyName, T value) where T : struct => Set(_entity, propertyName, value);
	
	public T Get<T>(string propertyName) where T : struct => Get<T>(_entity, propertyName);
	
}

public class EntityDecorUnregisteredPropertyException : Exception { }

public class EntityDecorUndefinedTypeException : Exception
{
	public EntityDecorUndefinedTypeException() {}
	public EntityDecorUndefinedTypeException(string message) : base(message) {}
}

public class EntityDecorRegisterPropertyFailedException : Exception { }

}