public interface ITimerAdapter
{
    string FormatTime(TimeSpan time);
}


public class TimerAdapter : ITimerAdapter
{
    public string FormatTime(TimeSpan time)
    {
        return time.ToString(@"hh\:mm\:ss\.f");
    }
}