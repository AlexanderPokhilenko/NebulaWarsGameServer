using System;
using System.Net;

namespace Server.Udp.Sending
{
    public class MockUdpSender: IUdpSender
    {
        private int numberOfSentDatagrams;
        
        public void Send(byte[] data, IPEndPoint endPoint)
        {
            numberOfSentDatagrams++;
        }

        public int GetNumbersOfPackages()
        {
            return numberOfSentDatagrams;
        }
    }
}