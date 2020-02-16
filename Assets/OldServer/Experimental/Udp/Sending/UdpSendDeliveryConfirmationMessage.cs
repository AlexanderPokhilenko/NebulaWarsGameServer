using System;
using System.Net;
using Libraries.NetworkLibrary.Udp.ServerToPlayer;
using NetworkLibrary.NetworkLibrary.Udp;

namespace OldServer.Experimental.Udp.Sending
{
    public static partial class UdpSendUtils
    {
        public static void SendDeliveryConfirmationMessage(DeliveryConfirmationFromServerMessage mes,
            IPEndPoint address)
        {
            if (address != null)
            {
                var data = MessageFactory.GetSerializedMessage(mes);
                NetworkMediator.udpBattleConnection.Send(data, address);
            }
            else
            {
                throw new Exception("SendDeliveryConfirmationMessage address == null");
            }
        }
    }
}