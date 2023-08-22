namespace GenOne.Blazor.Map
{
    internal static class PinsRaw
    {
        public static string UserPinHtml { get; } =
            """
                <svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" version="1.1" viewBox="-12 -12 24 24">
                	<circle r="9" style="stroke:#fff;stroke-width:3;fill:#2A93EE;fill-opacity:1;opacity:1;"></circle>
                </svg>
             """;
        
        public static string MapPinHtml { get; } =
            """
                <svg xmlns="http://www.w3.org/2000/svg" width="36" height="36" fill="#653CEE" viewBox="0 0 256 256"><path d="M128,16a88.1,88.1,0,0,0-88,88c0,75.3,80,132.17,83.41,134.55a8,8,0,0,0,9.18,0C136,236.17,216,179.3,216,104A88.1,88.1,0,0,0,128,16Zm0,56a32,32,0,1,1-32,32A32,32,0,0,1,128,72Z"></path></svg>
             """;
    }
}