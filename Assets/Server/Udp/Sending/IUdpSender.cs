﻿using System.Net;

namespace Server.Udp.Sending
{
    public interface IUdpSender
    {
        void Send(byte[] serializedContainer, IPEndPoint endPoint);
    }
}