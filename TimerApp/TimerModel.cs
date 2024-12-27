namespace TimerApp;

public interface ITimerModel
{
    TimeSpan RemainingTime { get; }
    TimeSpan InitialTime { get; }
    void SetTime(TimeSpan time);
    Task StartAsync(Action<TimeSpan> onTick, Action onComplete);
    void Stop();
    void Reset();
}

public class TimerModel : ITimerModel
{
    private TimeSpan _remainingTime;
    private TimeSpan _initialTime;
    private bool _isRunning;
    private CancellationTokenSource _cancellationTokenSource;

    public TimeSpan RemainingTime => _remainingTime;
    public TimeSpan InitialTime => _initialTime;

    public TimerModel()
    {
        _remainingTime = TimeSpan.Zero;
        _initialTime = TimeSpan.Zero;
        _isRunning = false;
    }

    public void SetTime(TimeSpan time)
    {
        _initialTime = time;
        _remainingTime = time;
    }

    public async Task StartAsync(Action<TimeSpan> onTick, Action onComplete)
    {
        if (_isRunning || _remainingTime <= TimeSpan.Zero) return;

        _isRunning = true;
        _cancellationTokenSource = new CancellationTokenSource();

        try
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested && _remainingTime > TimeSpan.Zero)
            {
                _remainingTime = _remainingTime.Subtract(TimeSpan.FromMilliseconds(100));
                await Task.Delay(100);
                if (_remainingTime <= TimeSpan.Zero)
                {
                    _remainingTime = TimeSpan.Zero;
                    onTick(_remainingTime);
                    onComplete();
                    break;
                }
                onTick(_remainingTime);
            }
        }
        catch (TaskCanceledException) { }
        finally
        {
            _isRunning = false;
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _cancellationTokenSource?.Cancel();
    }

    public void Reset()
    {
        Stop();
        _remainingTime = _initialTime;
    }
}
