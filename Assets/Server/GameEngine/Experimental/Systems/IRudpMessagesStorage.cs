using JetBrains.Annotations;
using Server.GameEngine.Experimental;

namespace Server.GameEngine.Systems
{
    public interface IRudpMessagesStorage
    {
        [CanBeNull]
        ReliableMessagesPack[] GetMessagesForActivePlayers();
    }
}