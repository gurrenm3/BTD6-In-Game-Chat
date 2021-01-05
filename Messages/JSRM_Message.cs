using Assets.Scripts.Unity;
using Newtonsoft.Json;
using BloonsTD6_Mod_Helper.Extensions;
using System.Reflection;

namespace In_Game_Chat.Messages
{
    /// <summary>
    /// This message happens when player joins a session
    /// </summary>
    class JSRM_Message
    {
        public const string broadcasterCoopCode = "BTD6_Chat_Broadcaster";
        public string Payload { get; set; } = $"I am using {Assembly.GetExecutingAssembly().GetName().Name} too!";
        public byte? PeerID { get; set; }
        public string PlayerName { get; set; }


        public JSRM_Message() {  }

        public JSRM_Message(int peerID, string playerName)
        {
            PeerID = (byte)peerID;
            PlayerName = playerName;
        }

        public void Send()
        {
            if (Game.instance?.nkGI is null)
                return;

            string json = Serialize();
            Game.instance.nkGI.SendMessage(json, code: broadcasterCoopCode);

            // Sending to peers as well. Likely not necessary as above sends to all
            /*Game.instance.nkGI.SendMessage(json, 1, broadcasterCoopCode);
            Game.instance.nkGI.SendMessage(json, 2, broadcasterCoopCode);
            Game.instance.nkGI.SendMessage(json, 3, broadcasterCoopCode);
            Game.instance.nkGI.SendMessage(json, 4, broadcasterCoopCode);*/
        }

        public JSRM_Message Read(string json)
        {
            return JsonConvert.DeserializeObject<JSRM_Message>(json);
        }

        private string Serialize()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}