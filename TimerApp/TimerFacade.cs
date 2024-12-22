public class TimerFacade
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

    public async Task StartAsync(Action<string> onTick, Action onComplete)
    {
        await _timerModel.StartAsync(
            time => onTick(_adapter.FormatTime(time)),
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
}