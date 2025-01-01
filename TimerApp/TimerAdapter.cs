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

    public (double Hours, double Minutes, double Seconds) GetHandAngles(TimeSpan time)
{
    double hourAngle = time.TotalHours % 12 * -30; 
    
    double minuteAngle = time.TotalMinutes % 60 * -6; 
    
    double secondAngle = time.TotalSeconds % 60 * -6;
    
    return (hourAngle, minuteAngle, secondAngle);
}

}