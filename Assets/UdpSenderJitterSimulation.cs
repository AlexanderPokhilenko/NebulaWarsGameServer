using System.Net;
using Server.Udp.Sending;

/// <summary>
/// Отвечает за эмуляцию задержки доставки сообщения от сервера до клиента
/// </summary>
public class UdpSenderJitterSimulation:JitterSimulation, IUdpSender
{
    private readonly IUdpSender realUdpSender;

    public UdpSenderJitterSimulation(IUdpSender realUdpSender, int minJitterDelayMs, int maxJitterDelayMs) 
        : base(minJitterDelayMs, maxJitterDelayMs)
    {
        this.realUdpSender = realUdpSender;
    }

    public void Send(byte[] data, IPEndPoint endPoint)
    {
        Delay(()=>realUdpSender.Send(data, endPoint));
    }
}