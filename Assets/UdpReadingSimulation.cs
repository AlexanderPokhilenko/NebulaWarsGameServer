using System.Net;
using Server.Udp.Connection;

/// <summary>
/// Отвечает за эмуляцию задержки доставки сообщения от клиента к серверу
/// </summary>
public class UdpReadingSimulation : JitterSimulation, IByteArrayHandler
{
    private readonly IByteArrayHandler byteArrayHandler;

    public UdpReadingSimulation(IByteArrayHandler byteArrayHandler, int minJitterDelayMs, int maxJitterDelayMs)
        : base(minJitterDelayMs, maxJitterDelayMs)
    {
        this.byteArrayHandler = byteArrayHandler;
    }

    public void HandleBytes(byte[] data, IPEndPoint endPoint)
    {
        Delay((() => byteArrayHandler.HandleBytes(data, endPoint)));
    }
}