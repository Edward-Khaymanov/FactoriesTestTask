using Cysharp.Threading.Tasks;
using System;
using System.Threading;

public class UniTaskTimer : ITimer
{
    private CancellationTokenSource _cancellationTokenSource;

    public UniTaskTimer()
    {
        _cancellationTokenSource = new();
    }

    public event Action Tick;
    public bool IsRunning { get; private set; }

    public void Start(float intervalInSeconds)
    {
        if (IsRunning)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        }
        _cancellationTokenSource = new();
        StartAsync(intervalInSeconds, _cancellationTokenSource.Token).Forget();
    }

    public void Stop()
    {
        _cancellationTokenSource.Cancel();
        IsRunning = false;
    }

    private async UniTaskVoid StartAsync(float intervalInSeconds, CancellationToken cancellationToken)
    {
        IsRunning = true;

        while (cancellationToken.IsCancellationRequested == false)
        {
            await UniTask.WaitForSeconds(intervalInSeconds, cancellationToken: _cancellationTokenSource.Token);
            Tick?.Invoke();
        }

        IsRunning = false;
    }
}