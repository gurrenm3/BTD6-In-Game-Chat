using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnhollowerBaseLib;
using NinjaKiwi.NKMulti;
using Assets.Scripts.Unity;
using Newtonsoft.Json;

namespace In_Game_Chat.Messages
{
    public class Chat_Message : BloonsTD6_Mod_Helper.Api.Coop.Chat_Message
    {
        public Chat_Message(string messageToSend, bool sendMessageNow = true)
        {
            var id = Game.instance?.nkGI?.PeerID;
            if (id.HasValue)
                PeerID = id.Value;

            Sender = Game.instance?.playerService?.Player?.LiNKAccount?.DisplayName;
            Message = messageToSend;

            if (sendMessageNow)
            {
                SessionData.Chat.SendMessage(this);
            }
        }
    }
}