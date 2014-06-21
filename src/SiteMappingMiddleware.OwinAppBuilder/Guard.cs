// ReSharper disable once CheckNamespace
namespace System
{
    internal static class Guard
    {
        internal static void MustNotBeNull(this object argument, string name)
        {
            if (argument == null)
            {
                throw new ArgumentNullException("name");
            }
        }

        internal static void MustNotBeNullOrWhitespace(this string argument, string name)
        {
            if (string.IsNullOrWhiteSpace(argument))
            {
                throw new ArgumentException("name");
            }
        }
    }
}