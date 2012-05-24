namespace Echo
{
	public static class StringExtensions
	{
		public static string Expand(this string format, params object[] args)
		{
			return (args.Length == 0) ? format : string.Format(format, args);
		}
	}
}