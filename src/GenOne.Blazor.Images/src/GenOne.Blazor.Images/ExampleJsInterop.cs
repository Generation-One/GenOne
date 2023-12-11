using Blurhash;
using Blurhash.ImageSharp;
using Microsoft.JSInterop;

namespace GenOne.Blazor.Images;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.
