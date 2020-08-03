using JetBrains.Annotations;

namespace Server.GameEngine.Experimental.Systems
{
    public interface IRudpMessagesStorage
    {
        [CanBeNull]
        ReliableMessagesPack[] GetMessagesForActivePlayers();
    }
}