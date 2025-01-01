namespace TimerApp;

public interface ITimerFacade
{
    void SetTime(TimeSpan time);
    Task StartAsync(Action<string> onTick, Action<(double Hours, double Minutes, double Seconds)> onClockUpdate, Action onComplete);
    void Stop();
    void Reset();
    string GetCurrentTime();
    (double Hours, double Minutes, double Seconds) GetClockAngles();
}

public class TimerFacade : ITimerFacade
{
    private readonly TimerModel _timerModel;
    private readonly TimerAdapter _adapter;

    public TimerFacade()
    {
        _timerModel = new TimerModel();
        _adapter = new TimerAdapter();
    }

    public void SetTime(TimeSpan time)
    {
        _timerModel.SetTime(time);
    }

    public async Task StartAsync(Action<string> onTick, Action<(double Hours, double Minutes, double Seconds)> onClockUpdate, Action onComplete)
    {
        await _timerModel.StartAsync(
            time => 
            {
                onTick(_adapter.FormatTime(time));
                onClockUpdate(_adapter.GetHandAngles(time));
            },
            onComplete);
    }

    public void Stop()
    {
        _timerModel.Stop();
    }

    public void Reset()
    {
        _timerModel.Reset();
    }

    public string GetCurrentTime()
    {
        return _adapter.FormatTime(_timerModel.RemainingTime);
    }

    public (double Hours, double Minutes, double Seconds) GetClockAngles()
    {
        return _adapter.GetHandAngles(_timerModel.RemainingTime);
    }
}