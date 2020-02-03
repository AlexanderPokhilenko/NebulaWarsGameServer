﻿﻿﻿﻿﻿﻿﻿﻿using NetworkLibrary.NetworkLibrary.Udp.ServerToPlayer.PositionMessages;
      using ZeroFormatter;

      namespace NetworkLibrary.NetworkLibrary.Udp.PlayerToServer.UserInputMessage
{
    [ZeroFormattable]
    public struct PlayerJoystickInputMessage
    {
        [Index(0)] public int PlayerTemporaryIdentifierForTheMatch;
        [Index(1)] public int GameRoomNumber;
        [Index(2)] public float X;
        [Index(3)] public float Y;

        public PlayerJoystickInputMessage(int playerTemporaryIdentifierForTheMatch, int gameRoomNumber, float x, float y)
        {
            PlayerTemporaryIdentifierForTheMatch = playerTemporaryIdentifierForTheMatch;
            GameRoomNumber = gameRoomNumber;
            X = x;
            Y = y;
        }

        public Vector2 GetVector2() => new Vector2(X, Y);
    }
}