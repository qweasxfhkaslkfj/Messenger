using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOP_Messanger
{
    public class Message
    {
        public string sender;
        public string text;
        public DateTime time;
        public string senderColor;

        public Message(string sender, string text, string senderColor = null)
        {
            this.sender = sender;
            this.text = text;
            this.time = DateTime.Now;
            if (senderColor != null)
                this.senderColor = senderColor;
            else
                this.senderColor = GetDefaultSenderColor(sender);
        }

        // Цвета сообщений
        private static string GetDefaultSenderColor(string sender)
        {
            int hash = Math.Abs(sender.GetHashCode());

            string[] colors = 
            {
                "#E3F2FD",
                "#F3E5F5",
                "#E8F5E9",
                "#FFF3E0",
                "#FCE4EC",
                "#E0F2F1",
                "#FFF8E1",
                "#E8EAF6",
                "#F1F8E9",
                "#F9FBE7",
                "#EDE7F6", 
                "#E0F7FA"
            };

            return colors[hash % colors.Length];
        }

        // Сообщение
        public override string ToString()
        {
            return $"[{Time:HH:mm}] {Sender}: {Text}";
        }

    }
}
