public class SwimResult
{
    public int ResultId { get; set; }
    public int EventDistance { get; set; }
    public string Stroke { get; set; }
    public TimeSpan Time { get; set; }
    public Swimmer Swimmer { get; set; }
    public DateTime EventDate { get; set; }

    public SwimResult(int resultId, int eventDistance, string stroke, TimeSpan time, Swimmer swimmer, DateTime eventDate) {
        ResultId = resultId;
        EventDistance = eventDistance;
        Stroke = stroke;
        Time = time;
        Swimmer = swimmer;
        EventDate = eventDate;
    }

    public override bool Equals(object? obj)
    {
        return obj is SwimResult result && this.ResultId == result.ResultId;
    }

    public override int GetHashCode()
    {
        return ResultId.GetHashCode();
    }
}
