using System;
using System.Threading;

/// <summary>
/// Нужен для симуляции сетевой задержки
/// </summary>
public abstract class JitterSimulation
{
    private readonly Random random;
    private readonly int minJitterDelayMs;
    private readonly int maxJitterDelayMs;
    
    protected JitterSimulation(int minJitterDelayMs, int maxJitterDelayMs)
    {
        this.minJitterDelayMs = minJitterDelayMs;
        this.maxJitterDelayMs = maxJitterDelayMs;
        random = new Random();
    }

    protected void Delay(Action action)
    {
        int delayMs = GetRandomDelay();
        new Timer(callback: o => action.Invoke(), null, delayMs, Timeout.Infinite);
    }
    
    private int GetRandomDelay()
    {
        return random.Next(minJitterDelayMs, maxJitterDelayMs);
    }
}