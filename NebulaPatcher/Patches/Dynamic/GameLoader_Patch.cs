﻿using HarmonyLib;
using NebulaWorld;

namespace NebulaPatcher.Patches.Dynamic
{
    [HarmonyPatch(typeof(GameLoader))]
    internal class GameLoader_Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(GameLoader.FixedUpdate))]
        public static void FixedUpdate_Postfix(int ___frame)
        {
            if (Multiplayer.IsActive && ___frame >= 11)
            {
                Multiplayer.Session.OnGameLoadCompleted();
            }
        }
    }
}
