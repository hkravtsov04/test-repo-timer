public class TimerAdapter
{
    public string FormatTime(TimeSpan time)
    {
        return time.ToString(@"hh\:mm\:ss\.f");
    }
}