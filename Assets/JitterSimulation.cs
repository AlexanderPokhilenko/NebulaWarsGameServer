using System;
using System.Net;
using System.Threading;
using Server.Udp.Connection;
using Server.Udp.Sending;

/// <summary>
/// Нужен для симуляции сетевой задержки
/// </summary>
public class JitterSimulation : IByteArrayHandler, IUdpSender
{
    private readonly Random random;
    private readonly int minJitterDelayMs;
    private readonly int maxJitterDelayMs;
    private readonly IUdpSender udpSender;
    private readonly IByteArrayHandler realByteArrayHandler;

    public JitterSimulation(IByteArrayHandler realByteArrayHandler, IUdpSender udpSender, int minJitterDelayMs,
        int maxJitterDelayMs)
    {
        this.realByteArrayHandler = realByteArrayHandler;
        this.udpSender = udpSender;
        this.minJitterDelayMs = minJitterDelayMs;
        this.maxJitterDelayMs = maxJitterDelayMs;
        random = new Random();
    }

    public void HandleBytes(byte[] data, IPEndPoint endPoint)
    {
        int delayMs = GetRandomDelay();
        new Timer(callback: o => realByteArrayHandler.HandleBytes(data, endPoint), null, delayMs, Timeout.Infinite);
    }

    public void Send(byte[] data, IPEndPoint endPoint)
    {
        int delayMs = GetRandomDelay();
        new Timer(o => udpSender.Send(data, endPoint), null, delayMs, Timeout.Infinite);
    }

    private int GetRandomDelay()
    {
        return random.Next(minJitterDelayMs, maxJitterDelayMs);
    }
    
}