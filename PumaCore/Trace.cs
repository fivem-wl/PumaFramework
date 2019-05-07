namespace PumaFramework.Core {

public static class Trace
{
	public static void Assert(bool condition, string description = null)
	{
		if (!condition) throw new AssertException(description);
	}
}

}