using Assets.Scripts.Unity;
using MelonLoader;
using Newtonsoft.Json;
using NinjaKiwi.NKMulti;
using System.Linq;
using UnhollowerBaseLib;
using BTD_Mod_Helper.Extensions;

namespace In_Game_Chat.Messages
{
    class MessageManager
    {
        public void JSRM_Msg()
        {
            int id = Game.instance.nkGI.PeerID;
            string playerName = Game.instance.playerService.Player.LiNKAccount.DisplayName;
            JSRM_Message jsrmMessage = new JSRM_Message(id, playerName);
            jsrmMessage.Send();
        }

        public void DC_Msg(Il2CppStructArray<byte> messageBytes)
        {
            var msg = new PeerDisconnectedMessage(messageBytes);

            var player = SessionData.playersWithMod.FirstOrDefault(p => p.PeerID == msg.PeerID);
            if (player != null)
            {
                SessionData.Chat.UpdateChatLog($"{player.PlayerName} has left.", "Notification");
                SessionData.playersWithMod.Remove(player);
                return;
            }

            SessionData.Chat.UpdateChatLog($"Player {msg.PeerID} has left.", "Notification");
        }

        public void Chat_Msg(Il2CppStructArray<byte> messageBytes)
        {
            var msg = new BloonsTD6_Mod_Helper.Api.Coop.Chat_Message(messageBytes);
            SessionData.Chat.UpdateChatLog(msg.Message, msg.Sender);
        }

        public void Broadcaster_Msg(Il2CppStructArray<byte> messageBytes)
        {
            var msg = Deserialize<JSRM_Message>(messageBytes);
            var player = SessionData.playersWithMod.FirstOrDefault(p => p.PeerID == msg.PeerID);
            if (player == null)
            {
                var newPlayer = new ModPlayer()
                {
                    PeerID = msg.PeerID.Value,
                    PlayerName = msg.PlayerName
                };

                SessionData.playersWithMod.Add(newPlayer);
                
                if (!string.IsNullOrEmpty(newPlayer.PlayerName))
                {
                    SessionData.Chat.UpdateChatLog($"{newPlayer.PlayerName} has joined as Player {newPlayer.PeerID}!", "Notification");
                }
                else
                    SessionData.Chat.UpdateChatLog($"Player {newPlayer.PeerID} has joined!", "Notification");

                JSRM_Msg();
                return;
            }

            
            //SessionData.Chat.UpdateChatLog($"{player.PlayerName} has joined as Player {player.PeerID}!", "Notification");
        }

        public T Deserialize<T> (Il2CppStructArray<byte> messageBytes)
        {
            string modMessage = SerialisationUtil.Deserialise<string>(messageBytes);
            return JsonConvert.DeserializeObject<T>(modMessage);
        }
    }
}
