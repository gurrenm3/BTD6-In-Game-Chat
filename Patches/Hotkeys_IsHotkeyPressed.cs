using Assets.Scripts.Unity.UI_New.InGame;
using Assets.Scripts.Unity.UI_New.Main;
using Harmony;
using System;
using UnhollowerRuntimeLib;
using UnityEngine;

namespace In_Game_Chat.Patches
{
    [HarmonyPatch(typeof(Hotkeys), nameof(Hotkeys.IsHotkeyPressed), new Type[] { typeof(KeyCode), typeof(KeyCode) })]
    internal class Hotkeys_IsHotkeyPressed
    {
        [HarmonyPostfix]
        internal static void Postfix(ref bool __result)
        {
            if (SessionData.Chat.IsFocused)
                __result = false;
        }
    }
}