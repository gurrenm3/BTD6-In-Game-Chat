using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.Main;
using Harmony;

namespace In_Game_Chat.Patches
{
    [HarmonyPatch(typeof(Hotkeys), nameof(Hotkeys.InputGetKeyDown))]
    internal class Hotkeys_InputGetKeyDown
    {
        [HarmonyPostfix]
        internal static void Postfix(ref bool __result)
        {
            if (SessionData.Chat.IsFocused)
                __result = false;
        }
    }
}