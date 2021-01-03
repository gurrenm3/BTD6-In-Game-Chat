using Assets.Scripts.Unity;
using Assets.Scripts.Unity.UI_New.InGame;
using MelonLoader;
using System;
using System.Reflection;
using UnityEngine;
using BloonsTD6_Mod_Helper.Extensions;
using NinjaKiwi.NKMulti;

namespace In_Game_Chat
{
    public class MelonMain : MelonMod
    {
        internal static string modDir = $"{Environment.CurrentDirectory}\\Mods\\{Assembly.GetExecutingAssembly().GetName().Name}";
        public static string coopCode = "BTD6_Chat";

        public override void OnApplicationStart()
        {
            MelonLogger.Log("Mod has finished loading");
        }

        public override void OnUpdate()
        {
            if (InGame.instance is null && Game.instance?.nkGI is null)
                return;

            if (!SessionData.Chat.instantiated)
                SessionData.Chat.InitializeChatBox();

            if (Input.GetKeyDown(KeyCode.UpArrow))
                SessionData.Chat.ShowHideChatBox();

            SessionData.Chat.Update();       
        }
    }
}