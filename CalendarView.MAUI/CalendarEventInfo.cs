namespace CalendarView.MAUI;

public enum CalendarEventType
{
    Event1 = 1, Event2 = 2, Event3 = 3
}
public class CalendarEventInfo
{
    public string Title { get; set; }
    public DateTime Date { get; set; }
    public CalendarEventType Type { get; set; }
}
