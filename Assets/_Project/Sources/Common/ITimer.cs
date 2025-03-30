using System;

public interface ITimer
{
    public event Action Tick;
    public bool IsRunning { get; }
    public void Start(float intervalInSeconds);
    public void Stop();
}