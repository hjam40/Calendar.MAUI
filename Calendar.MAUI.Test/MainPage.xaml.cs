using CalendarView.MAUI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace Calendar.MAUI.Test;

public partial class MainPage : ContentPage
{
	public ObservableCollection<CalendarEventInfo> Events { get; set; } = new ObservableCollection<CalendarEventInfo>();
    public ObservableCollection<DateTime> Months { get; set; } = new ObservableCollection<DateTime> { DateTime.Today.AddMonths(-1), DateTime.Today, DateTime.Today.AddMonths(1) };
    public DateTime MinDay { get; set; } = DateTime.Today;
    private DateTime selectedDay = DateTime.Today;
    public DateTime SelectedDay { get => selectedDay; set { selectedDay = value; OnPropertyChanged(nameof(SelectedDay)); } }
    public CultureInfo CalendarCulture { get; set; } = new("en-US");
    public ObservableCollection<CalendarView.MAUI.CalendarView> Calendars { get; set; } = new ObservableCollection<CalendarView.MAUI.CalendarView>();

    public MainPage()
	{
		InitializeComponent();
		Events.Add(new CalendarEventInfo { Date = DateTime.Now.AddDays(2), Title = "Event 1", Type = CalendarEventType.Event1 });
        Events.Add(new CalendarEventInfo { Date = DateTime.Now.AddDays(4), Title = "Event 2", Type = CalendarEventType.Event2 });
        Events.Add(new CalendarEventInfo { Date = DateTime.Now.AddDays(6), Title = "Event 3", Type = CalendarEventType.Event3 });
        Events.Add(new CalendarEventInfo { Date = DateTime.Now.AddDays(8), Title = "Event Mix 1", Type = CalendarEventType.Event3 });
        Events.Add(new CalendarEventInfo { Date = DateTime.Now.AddDays(8), Title = "Event Mix 2", Type = CalendarEventType.Event1 });
        OnPropertyChanged(nameof(Events));
        EN_Calendar.SelectionChanged += EN_Calendar_SelectionChanged;

    }

    private void EN_Calendar_SelectionChanged(object sender, EventArgs e)
    {
        Debug.WriteLine($"Selected day = {EN_Calendar.SelectedDate}");
    }
}

