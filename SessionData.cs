using In_Game_Chat.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace In_Game_Chat
{
    class SessionData
    {
        public static ChatBox Chat { get; set; } = new ChatBox();
        public static List<ModPlayer> playersWithMod = new List<ModPlayer>();
        public static MessageManager messageManager = new MessageManager();

        public static void RemovePlayer(ModPlayer player)
        {
            playersWithMod.Remove(player);
        }

        public static void AddPlayer(ModPlayer player)
        {
            playersWithMod.Add(player);
        }
    }
}
