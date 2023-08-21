using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace GenOne.Blazor.Map
{
    public class MapInitializationException : Exception
    {
        private static MapInitializationException Instance = new();

        private MapInitializationException() : base("Map hasn't been initialized.")
        { }

        [StackTraceHidden]
        internal static void ThrowIf([DoesNotReturnIf(true)] bool condition)
        {
            if (condition)
            {
                throw Instance;
            }
        }
    }
}