// Copyright (c) 2021 Augie R. Maddox, Guavaman Enterprises. All rights reserved.

// Prevents Unity from stripping Rewired Bolt integration DLL when building with IL2CPP.

namespace Rewired.Integration.Bolt
{
    [UnityEngine.Scripting.Preserve]
    internal static class PreventDLLStripping
    {
        [UnityEngine.Scripting.Preserve]
        private static void Preserve()
        {
            Rewired.Integration.Bolt.Stripping.Preserve();
        }
    }
}