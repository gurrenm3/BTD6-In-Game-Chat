using Harmony;
using NinjaKiwi.NKMulti;
using MelonLoader;
using System;
using BloonsTD6_Mod_Helper.Api.Coop;
using System.Linq;
using System.Collections.Generic;
using Assets.Scripts.Unity;
using In_Game_Chat.Messages;

namespace In_Game_Chat.Patches
{
    [HarmonyPatch(typeof(NKMultiConnection), nameof(NKMultiConnection.Receive))]
    internal class NKMultiConnection_Receive
    {
        static List<string> playerConnectedCodes = new List<string>() { "JSRM", "PCM" };
        static List<string> playerDisconnectedCodes = new List<string>() { "DC" };

        [HarmonyPostfix]
        internal static void Postfix(NKMultiConnection __instance)
        {
            var messageQueue = __instance.ReceiveQueue;
            if (messageQueue == null || messageQueue.Count == 0)
                return;

            foreach (var message in messageQueue)
            {
                ActOnCode(message);
            }
        }

        private static void ActOnCode(Message message)
        {
            //MelonLogger.Log(message.Code);
            switch (message.Code)
            {
                case "JSRM":
                    SessionData.messageManager.JSRM_Msg();
                    break;
                case "DC":
                    SessionData.messageManager.DC_Msg(message.bytes);
                    break;
                case JSRM_Message.broadcasterCoopCode:
                    SessionData.messageManager.Broadcaster_Msg(message.bytes);
                    break;
                case Chat_Message.chatCoopCode:
                    SessionData.messageManager.Chat_Msg(message.bytes);
                    break;
                default:
                    break;
            }

            //return;
            try
            {
               /* var deserialize = SerialisationUtil.Deserialise<string>(message.bytes);
                MelonLogger.Log(deserialize);*/
            }
            catch (Exception ex)
            {
                //MelonLogger.Log(ex.Message);
            }
        }
    }
}
