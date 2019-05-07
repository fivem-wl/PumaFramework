using System;

namespace PumaFramework.Core {

public class AssertException : Exception
{
	public readonly string Description;
	
	
	public AssertException(string desc)
	{
		Description = desc;
	}
}

}