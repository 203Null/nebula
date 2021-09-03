﻿using HarmonyLib;
using NebulaWorld;

namespace NebulaPatcher.Patches.Dynamic
{
    // This is part of the weird planet movement fix
    // as we delay setting localPlanet and planetId we need to be able to load FactoryData without them beeing set
    // LoadingPlanetFactoryMain() -> factoryModel.gpuiManager.AddModel -> VegeRenderer::AddInst() -> base.manager.activeFactory -> GPUInstancingManager::get_activeFactory() -> get_activePlanet()
    // which accesses GameMain.localPlanet which is null in that case which causes a NullReferenceException
    // we need to return the right PlanetData in that case, luckily we make use of PlanetData.factoryLoading while loading the factory
    [HarmonyPatch(typeof(GPUInstancingManager))]
    internal class GPUInstancingManager_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(GPUInstancingManager.activePlanet), MethodType.Getter)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles", Justification = "Original Function Name")]
        public static bool get_activePlanet_Prefix(GPUInstancingManager __instance, ref PlanetData __result)
        {
            if (!Multiplayer.IsActive)
            {
                return true;
            }

            __result = __instance.specifyPlanet ?? GameMain.localPlanet;
            if (__result == null && GameMain.localStar != null)
            {
                foreach (PlanetData p in GameMain.galaxy.StarById(GameMain.localStar.id).planets)
                {
                    if (p.factoryLoading)
                    {
                        __result = p;
                        break;
                    }
                }
            }
            return false;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GPUInstancingManager.AddModel))]
        public static bool AddModel_Prefix(GPUInstancingManager __instance, ref int __result)
        {
            //Do not add model to the GPU queue if player is not on the same planet as building that was build
            if (Multiplayer.IsActive && Multiplayer.Session.Factories.EventFactory != null && Multiplayer.Session.Factories.EventFactory.planet != __instance.activePlanet)
            {
                __result = 0;
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(GPUInstancingManager.AddPrebuildModel))]
        public static bool AddPrebuildModel_Prefix(GPUInstancingManager __instance, ref int __result)
        {
            //Do not add model to the GPU queue if player is not on the same planet as building that was build
            if (Multiplayer.IsActive && Multiplayer.Session.Factories.EventFactory != null && Multiplayer.Session.Factories.EventFactory.planet != __instance.activePlanet)
            {
                __result = 0;
                return false;
            }
            return true;
        }
    }
}
