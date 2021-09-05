﻿using HarmonyLib;
using NebulaModel.Packets.Factory.Laboratory;
using NebulaWorld;

namespace NebulaPatcher.Patches.Dynamic
{
    [HarmonyPatch(typeof(UILabWindow))]
    internal class UILabWindow_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(UILabWindow.OnItemButtonClick))]
        public static void OnItemButtonClick_Prefix(UILabWindow __instance, int index)
        {
            if (!Multiplayer.IsActive)
            {
                return;
            }

            LabComponent labComponent = GameMain.localPlanet.factory.factorySystem.labPool[__instance.labId];
            if (labComponent.researchMode)
            {
                if (GameMain.mainPlayer.inhandItemId > 0 && GameMain.mainPlayer.inhandItemCount > 0)
                {
                    //Notify about depositing source cubes
                    ItemProto[] matrixProtos = __instance.matrixProtos;
                    int id = matrixProtos[index].ID;
                    if (GameMain.mainPlayer.inhandItemId == id)
                    {
                        int num = labComponent.matrixServed[index] / 3600;
                        int num2 = 100 - num;
                        if (num2 < 0)
                        {
                            num2 = 0;
                        }
                        int num3 = (GameMain.mainPlayer.inhandItemCount >= num2) ? num2 : GameMain.mainPlayer.inhandItemCount;
                        if (num3 > 0)
                        {
                            Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateCubesPacket(labComponent.matrixServed[index] + num3 * 3600, index, __instance.labId, GameMain.localPlanet?.id ?? -1));
                        }
                    }
                }
                else
                {
                    //Notify about widthrawing source cubes
                    if (labComponent.matrixServed[index] / 3600 > 0)
                    {
                        Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateCubesPacket(0, index, __instance.labId, GameMain.localPlanet?.id ?? -1));
                    }
                }
            }
            else if (labComponent.matrixMode)
            {
                if (GameMain.mainPlayer.inhandItemId > 0 && GameMain.mainPlayer.inhandItemCount > 0)
                {
                    //Notify about depositing source items to the center
                    int num7 = labComponent.served[index];
                    int num8 = 100 - num7;
                    if (num8 < 0)
                    {
                        num8 = 0;
                    }
                    int num9 = (GameMain.mainPlayer.inhandItemCount >= num8) ? num8 : GameMain.mainPlayer.inhandItemCount;
                    if (num9 > 0)
                    {
                        Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateStoragePacket(labComponent.served[index] + num9, index, __instance.labId, GameMain.localPlanet?.id ?? -1));
                    }
                }
                else
                {
                    //Notify about withdrawing source items from the center
                    if (labComponent.served[index] > 0)
                    {
                        Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateStoragePacket(0, index, __instance.labId, GameMain.localPlanet?.id ?? -1));
                    }
                }
            }
            else
            {
                //Notify about changing matrix selection
                Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateEventPacket(index, __instance.labId, GameMain.localPlanet?.id ?? -1));
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(UILabWindow.OnProductButtonClick))]
        public static void OnItemButtonClick_Prefix(UILabWindow __instance)
        {
            if (!Multiplayer.IsActive)
            {
                return;
            }

            LabComponent labComponent = GameMain.localPlanet.factory.factorySystem.labPool[__instance.labId];
            if (labComponent.matrixMode)
            {
                //Notify about withdrawing produced cubes
                Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateEventPacket(-3, __instance.labId, GameMain.localPlanet?.id ?? -1));
            }
            else if (!labComponent.researchMode)
            {
                //Notify about selection of research mode
                Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateEventPacket(-1, __instance.labId, GameMain.localPlanet?.id ?? -1));
            }

        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(UILabWindow.OnBackButtonClick))]
        public static void OnBackButtonClick_Prefix(UILabWindow __instance)
        {
            //Notify about recipe reset
            if (Multiplayer.IsActive)
            {
                Multiplayer.Session.Network.SendPacketToLocalStar(new LaboratoryUpdateEventPacket(-2, __instance.labId, GameMain.localPlanet?.id ?? -1));
            }
        }
    }
}
