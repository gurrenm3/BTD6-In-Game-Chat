using Assets.Scripts.Simulation;
using Assets.Scripts.Unity.UI_New.InGame;
using Harmony;
using MelonLoader;

namespace In_Game_Chat.Patches
{
    [HarmonyPatch(typeof(Simulation), nameof(Simulation.InitialiseMap))]
    internal class Simulation_Initialise
    {
        [HarmonyPostfix]
        internal static void Postfix(Simulation __instance)
        {
            SessionData.Chat.IsVisible = false;
            /*MelonLogger.Log("Initialise");
            bool isCoop = InGame.instance.IsCoop;

            MelonLogger.Log(isCoop);*/
        }
    }
}