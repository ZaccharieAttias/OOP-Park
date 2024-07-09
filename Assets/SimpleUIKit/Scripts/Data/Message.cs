using System;
using System.Collections.Generic;

namespace Assets.Scripts.Data
{
    [Serializable]
    public class Message
    {
        public string TextEncrypted;
        public DateTime Time;
        public uint FromId;
        public uint ToId;
        public string FromToIdHash;

        public Message()
        {

        }

        public Message(string fromToHash, uint fromId, uint toId, string message)
        {
            FromToIdHash = fromToHash;
            FromId = fromId;
            ToId = toId;
            TextEncrypted = message;
        }

        public bool Equals(Message other)
        {
            return FromToIdHash == other.FromToIdHash && ToId == other.ToId && FromId == other.FromId && FromToIdHash == other.FromToIdHash && TextEncrypted == other.TextEncrypted;
        }

        public override int GetHashCode()
        {
            return (FromToIdHash + TextEncrypted).GetHashCode();
        }
    }

    public class MessageComparer : IEqualityComparer<Message>
    {
        public bool Equals(Message a, Message b)
        {
            if (ReferenceEquals(a, b)) return true;

            return a != null && b != null && a.Equals(b);
        }

        public int GetHashCode(Message obj)
        {
            return obj.GetHashCode();
        }
    }
}
