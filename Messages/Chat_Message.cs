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
    public class Chat_Message
    {
        public const string chatCoopCode = "BTD6_Chat_Msg";
        public int PeerID { get; set; }
        public string Sender { get; set; }
        public string Message { get; set; }
        //public Il2CppStructArray<byte> MessageBytes { get; set; }


        public Chat_Message() {  }

        public Chat_Message(string messageToSend, bool sendMessageNow = true)
        {
            PeerID = Game.instance.nkGI.PeerID;
            Sender = Game.instance.playerService.Player.LiNKAccount.DisplayName;
            Message = messageToSend;

            if (sendMessageNow)
                SessionData.Chat.SendMessage(this);
        }

        public Chat_Message(Il2CppStructArray<byte> messageBytes)
        {
            var json = SerialisationUtil.Deserialise<string>(messageBytes);
            var message = Read(json);
            PeerID = message.PeerID;
            Sender = message.Sender;
            Message = message.Message;
        }

        public Chat_Message Read(string json)
        {
            return JsonConvert.DeserializeObject<Chat_Message>(json);
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
