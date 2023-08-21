using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace GenOne.Blazor.BottomSheet.Exceptions
{
    public class BottomSheetNotInitializedException : Exception
    {
        private static BottomSheetNotInitializedException Instance = new();

        private BottomSheetNotInitializedException() : base("Bottom sheet is not initialized.")
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
