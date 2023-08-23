using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace GenOne.Blazor.QrScanner
{
    public class QrScannerNotInitializedException : Exception
    {
        private static readonly QrScannerNotInitializedException Instance = new();

        private QrScannerNotInitializedException() : base("Qr scanner is not initialized.")
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
