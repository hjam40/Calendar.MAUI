using Microsoft.Maui.Controls.Shapes;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;

using Path = Microsoft.Maui.Controls.Shapes.Path;

namespace CalendarView.MAUI;

public partial class CalendarView : ContentView
{
    //Default buttons data
    private const string NEXTBUTTONDATA = "M12.2929 16.7071C11.9024 16.3166 11.9024 15.6834 12.2929 15.2929L14.5858 13H7C6.44772 13 6 12.5523 6 12C6 11.4477 6.44772 11 7 11H14.5858L12.2929 8.70711C11.9024 8.31658 11.9024 7.68342 12.2929 7.29289C12.6834 6.90237 13.3166 6.90237 13.7071 7.29289L17.7071 11.2929C18.0976 11.6834 18.0976 12.3166 17.7071 12.7071L13.7071 16.7071C13.3166 17.0976 12.6834 17.0976 12.2929 16.7071Z M12 23C5.92487 23 1 18.0751 1 12C1 5.92487 5.92487 1 12 1C18.0751 1 23 5.92487 23 12C23 18.0751 18.0751 23 12 23ZM3 12C3 16.9706 7.02944 21 12 21C16.9706 21 21 16.9706 21 12C21 7.02944 16.9706 3 12 3C7.02944 3 3 7.02944 3 12Z";
    private const string PREVBUTTONDATA = "M11.7071 16.7071C12.0976 16.3166 12.0976 15.6834 11.7071 15.2929L9.41421 13H17C17.5523 13 18 12.5523 18 12C18 11.4477 17.5523 11 17 11H9.41421L11.7071 8.70711C12.0976 8.31658 12.0976 7.68342 11.7071 7.29289C11.3166 6.90237 10.6834 6.90237 10.2929 7.29289L6.29289 11.2929C5.90237 11.6834 5.90237 12.3166 6.29289 12.7071L10.2929 16.7071C10.6834 17.0976 11.3166 17.0976 11.7071 16.7071Z M12 23C18.0751 23 23 18.0751 23 12C23 5.92487 18.0751 1 12 1C5.92487 1 1 5.92487 1 12C1 18.0751 5.92487 23 12 23ZM21 12C21 16.9706 16.9706 21 12 21C7.02944 21 3 16.9706 3 12C3 7.02944 7.02944 3 12 3C16.9706 3 21 7.02944 21 12Z";
    //Default styles
    private static bool STYLESINITIATED = false;
    private static readonly Style BORDERSTYLE = new(typeof(Border));
    private static readonly Style MONTHSTYLE = new(typeof(Label));
    private static readonly Style OTHERMONTHDAYSTYLE = new(typeof(CalendarDayView));
    private static readonly Style SELECTEDDAYSTYLE = new(typeof(CalendarDayView));
    private static readonly Style TODAYSTYLE = new(typeof(CalendarDayView));
    private static readonly Style EVENT1DAYSTYLE = new(typeof(CalendarDayView));
    private static readonly Style EVENT2DAYSTYLE = new(typeof(CalendarDayView));
    private static readonly Style EVENT3DAYSTYLE = new(typeof(CalendarDayView));

    //General Calendar Properties
    public static readonly BindableProperty SelectedDateProperty = BindableProperty.Create(nameof(SelectedDate), typeof(DateTime?), typeof(CalendarView), null, BindingMode.TwoWay, propertyChanged: SelectedDateChanged);
    public static readonly BindableProperty MinSelectedDateProperty = BindableProperty.Create(nameof(MinSelectedDate), typeof(DateTime), typeof(CalendarView), DateTime.MinValue, propertyChanged: (b, o, n) => { if (o != n) ((CalendarView)b).FillMonthDays(); });
    public static readonly BindableProperty MaxSelectedDateProperty = BindableProperty.Create(nameof(MaxSelectedDate), typeof(DateTime), typeof(CalendarView), DateTime.MaxValue, propertyChanged: (b, o, n) => { if (o != n) ((CalendarView)b).FillMonthDays(); });
    public static readonly BindableProperty FirstDayOfWeekProperty = BindableProperty.Create(nameof(FirstDayOfWeek), typeof(DayOfWeek), typeof(CalendarView), Thread.CurrentThread.CurrentCulture.DateTimeFormat.FirstDayOfWeek, propertyChanged: (b, o, n) => { if (o != n) ((CalendarView)b).FillMonthDays(); });
    public static readonly BindableProperty CalendarCultureProperty = BindableProperty.Create(nameof(CalendarCulture), typeof(CultureInfo), typeof(CalendarView), Thread.CurrentThread.CurrentCulture, propertyChanged: (b, o, n) => { if (o != n && n != null && n is CultureInfo ci) ((CalendarView)b).FirstDayOfWeek = ci.DateTimeFormat.FirstDayOfWeek; });
    public static readonly BindableProperty CalendarEventsProperty = BindableProperty.Create(nameof(CalendarEvents), typeof(IEnumerable<CalendarEventInfo>), typeof(CalendarView), null, propertyChanged: CalendarEventsChanged);
    public static readonly BindableProperty HeaderBackgroundProperty = BindableProperty.Create(nameof(HeaderBackground), typeof(Brush), typeof(CalendarView), null);
    public static readonly BindableProperty ShowChangeMonthArrowsProperty = BindableProperty.Create(nameof(ShowChangeMonthArrows), typeof(bool), typeof(CalendarView), true);
    public static readonly BindableProperty AllowPanGestureProperty = BindableProperty.Create(nameof(AllowPanGesture), typeof(bool), typeof(CalendarView), true);
    public static readonly BindableProperty BorderStyleProperty = BindableProperty.Create(nameof(BorderStyle), typeof(Style), typeof(CalendarView), BORDERSTYLE);
    //First and Last day of week Styles
    public static readonly BindableProperty FirstDayOfWeekStyleProperty = BindableProperty.Create(nameof(FirstDayOfWeekStyle), typeof(Style), typeof(CalendarView), null);
    public static readonly BindableProperty LastDayOfWeekStyleProperty = BindableProperty.Create(nameof(LastDayOfWeekStyle), typeof(Style), typeof(CalendarView), null);
    //Buttons Properties
    public static readonly BindableProperty NextButtonDataProperty = BindableProperty.Create(nameof(NextButtonData), typeof(string), typeof(CalendarView), NEXTBUTTONDATA);
    public static readonly BindableProperty PrevButtonDataProperty = BindableProperty.Create(nameof(PrevButtonData), typeof(string), typeof(CalendarView), PREVBUTTONDATA);
    public static readonly BindableProperty ButtonsColorProperty = BindableProperty.Create(nameof(ButtonsColor), typeof(Color), typeof(CalendarView), Colors.Black);
    public static readonly BindableProperty ButtonsScaleProperty = BindableProperty.Create(nameof(ButtonsScale), typeof(double), typeof(CalendarView), 1d);
    //Month Properties
    public static readonly BindableProperty MonthFormatProperty = BindableProperty.Create(nameof(MonthFormat), typeof(string), typeof(CalendarView), "MMMM yy", propertyChanged: (b, o, n) => { if (o != n) ((CalendarView)b).FillMonthDays(); });
    public static readonly BindableProperty MonthStyleProperty = BindableProperty.Create(nameof(MonthStyle), typeof(Style), typeof(CalendarView), MONTHSTYLE);
    //Day Names Styles
    public static readonly BindableProperty DayNameStyleProperty = BindableProperty.Create(nameof(DayNameStyle), typeof(Style), typeof(CalendarView), null);
    public static readonly BindableProperty DayNameFirstDayOfWeekStyleProperty = BindableProperty.Create(nameof(DayNameFirstDayOfWeekStyle), typeof(Style), typeof(CalendarView), null);
    public static readonly BindableProperty DayNameLastDayOfWeekStyleProperty = BindableProperty.Create(nameof(DayNameLastDayOfWeekStyle), typeof(Style), typeof(CalendarView), null);
    //Day Style
    public static readonly BindableProperty DayStyleProperty = BindableProperty.Create(nameof(DayStyle), typeof(Style), typeof(CalendarView), null);
    //Other Month and out of selection range day Style
    public static readonly BindableProperty OtherMonthDayStyleProperty = BindableProperty.Create(nameof(OtherMonthDayStyle), typeof(Style), typeof(CalendarView), OTHERMONTHDAYSTYLE);
    //Today Style
    public static readonly BindableProperty TodayStyleProperty = BindableProperty.Create(nameof(TodayStyle), typeof(Style), typeof(CalendarView), TODAYSTYLE);
    //Selected Day Style
    public static readonly BindableProperty SelectedDayStyleProperty = BindableProperty.Create(nameof(SelectedDayStyle), typeof(Style), typeof(CalendarView), SELECTEDDAYSTYLE);
    //Events Days Styles
    public static readonly BindableProperty Event1DayStyleProperty = BindableProperty.Create(nameof(Event1DayStyle), typeof(Style), typeof(CalendarView), EVENT1DAYSTYLE);
    public static readonly BindableProperty Event2DayStyleProperty = BindableProperty.Create(nameof(Event2DayStyle), typeof(Style), typeof(CalendarView), EVENT2DAYSTYLE);
    public static readonly BindableProperty Event3DayStyleProperty = BindableProperty.Create(nameof(Event3DayStyle), typeof(Style), typeof(CalendarView), EVENT3DAYSTYLE);
    /// <summary>
    /// Trigger when user change the selected day. The event does not trigger if SelectedDay property is set.
    /// </summary>
    public event EventHandler SelectionChanged;

    public DateTime? SelectedDate { get => (DateTime?)GetValue(SelectedDateProperty); set => SetValue(SelectedDateProperty, value); }
    public DateTime MinSelectedDate { get => (DateTime)GetValue(MinSelectedDateProperty); set => SetValue(MinSelectedDateProperty, value); }
    public DateTime MaxSelectedDate { get => (DateTime)GetValue(MaxSelectedDateProperty); set => SetValue(MaxSelectedDateProperty, value); }
    public DayOfWeek FirstDayOfWeek { get => (DayOfWeek)GetValue(FirstDayOfWeekProperty); set => SetValue(FirstDayOfWeekProperty, value); }
    /// <summary>
    /// Style Target Type Border. Define the aspect for the control border.
    /// </summary>
    public Style BorderStyle { get => (Style)GetValue(BorderStyleProperty); set => SetValue(BorderStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for First day of week cells.
    /// </summary>
    public Style FirstDayOfWeekStyle { get => (Style)GetValue(FirstDayOfWeekStyleProperty); set => SetValue(FirstDayOfWeekStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for Last day of week cells.
    /// </summary>
    public Style LastDayOfWeekStyle { get => (Style)GetValue(LastDayOfWeekStyleProperty); set => SetValue(LastDayOfWeekStyleProperty, value); }
    public CultureInfo CalendarCulture { get => (CultureInfo)GetValue(CalendarCultureProperty); set => SetValue(CalendarCultureProperty, value); }
    public IEnumerable<CalendarEventInfo> CalendarEvents { get => (IEnumerable<CalendarEventInfo>)GetValue(CalendarEventsProperty); set => SetValue(CalendarEventsProperty, value); }
    public Brush HeaderBackground { get => (Brush)GetValue(HeaderBackgroundProperty); set => SetValue(HeaderBackgroundProperty, value); }
    /// <summary>
    /// Show (true) or hide (false) buttons for change month
    /// </summary>
    public bool ShowChangeMonthArrows { get => (bool)GetValue(ShowChangeMonthArrowsProperty); set => SetValue(ShowChangeMonthArrowsProperty, value); }
    /// <summary>
    /// Allow Pan gesture for change month
    /// </summary>
    public bool AllowPanGesture { get => (bool)GetValue(AllowPanGestureProperty); set => SetValue(AllowPanGestureProperty, value); }
    /// <summary>
    /// String markup data for next button icon
    /// </summary>
    public string NextButtonData
    {
        get => (string)GetValue(NextButtonDataProperty);
        set
        {
            SetValue(NextButtonDataProperty, value);
            try
            {
                NextPath.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(value);
            }
            catch
            {
                NextPath.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(NEXTBUTTONDATA);
            }
        }
    }
    /// <summary>
    /// String markup data for previous button icon
    /// </summary>
    public string PrevButtonData
    {
        get => (string)GetValue(PrevButtonDataProperty);
        set
        {
            SetValue(PrevButtonDataProperty, value);
            try
            {
                PrevPath.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(value);
            }
            catch
            {
                PrevPath.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(PREVBUTTONDATA);
            }
        }
    }
    /// <summary>
    /// Color for change month buttons
    /// </summary>
    public Color ButtonsColor { get => (Color)GetValue(ButtonsColorProperty); set => SetValue(ButtonsColorProperty, value); }
    /// <summary>
    /// Scale for change month buttons
    /// </summary>
    public double ButtonsScale { get => (double)GetValue(ButtonsScaleProperty); set => SetValue(ButtonsScaleProperty, value); }
    /// <summary>
    /// Datetime format for Month label. Default "MMMM yy"
    /// </summary>
    public string MonthFormat { get => (string)GetValue(MonthFormatProperty); set => SetValue(MonthFormatProperty, value); }
    /// <summary>
    /// Style Target Type Label. Define the aspect for Month Name Label.
    /// </summary>
    public Style MonthStyle { get => (Style)GetValue(MonthStyleProperty); set => SetValue(MonthStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayNameView. Define the aspect for the days name cells.
    /// </summary>
    public Style DayNameStyle { get => (Style)GetValue(DayNameStyleProperty); set => SetValue(DayNameStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayNameView. Define the aspect for the first day of week name cell.
    /// </summary>
    public Style DayNameFirstDayOfWeekStyle { get => (Style)GetValue(DayNameFirstDayOfWeekStyleProperty); set => SetValue(DayNameFirstDayOfWeekStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayNameView. Define the aspect for the last day of week name cell.
    /// </summary>
    public Style DayNameLastDayOfWeekStyle { get => (Style)GetValue(DayNameLastDayOfWeekStyleProperty); set => SetValue(DayNameLastDayOfWeekStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for the normal days cells.
    /// </summary>
    public Style DayStyle { get => (Style)GetValue(DayStyleProperty); set => SetValue(DayStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for days cells out of selection range or actual month.
    /// </summary>
    public Style OtherMonthDayStyle { get => (Style)GetValue(OtherMonthDayStyleProperty); set => SetValue(OtherMonthDayStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect Today cell.
    /// </summary>
    public Style TodayStyle { get => (Style)GetValue(TodayStyleProperty); set => SetValue(TodayStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for the selected day cell.
    /// </summary>
    public Style SelectedDayStyle { get => (Style)GetValue(SelectedDayStyleProperty); set => SetValue(SelectedDayStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for days cells with events type Event1.
    /// </summary>
    public Style Event1DayStyle { get => (Style)GetValue(Event1DayStyleProperty); set => SetValue(Event1DayStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for days cells with events type Event2.
    /// </summary>
    public Style Event2DayStyle { get => (Style)GetValue(Event2DayStyleProperty); set => SetValue(Event2DayStyleProperty, value); }
    /// <summary>
    /// Style Target Type CalendarDayView. Define the aspect for days cells with events type Event3.
    /// </summary>
    public Style Event3DayStyle { get => (Style)GetValue(Event3DayStyleProperty); set => SetValue(Event3DayStyleProperty, value); }

    private readonly Grid[] DaysGrids = new Grid[3];
    private readonly CalendarDayView[,,] DaysArray = new CalendarDayView[3, 6, 7];
    private readonly CalendarDayNameView[] DaysNamesArray = new CalendarDayNameView[7];
    private DateTime CurrentMonth = DateTime.Today;
    private bool changingSelectedDate = false;
    private double totalPanX = 0;
    private bool runningPan = false;
    private bool animatingButton = false;
    private double monthWith = 0;
    private double columnMonthWith = 0;

    public CalendarView()
    {
        DefineDefaultStyles();
        InitializeComponent();

        for (int i = 0; i < 3; i++)
        {
            DaysGrids[i] = new Grid
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,
                RowDefinitions = { new(), new(), new(), new(), new() },
                ColumnDefinitions = { new(), new(), new(), new(), new(), new(), new() }
            };
            MonthsGrid.Add(DaysGrids[i]);
        }

        MonthsGrid.SizeChanged += Control_SizeChanged;
        PanGestureRecognizer panGesture = new();
        panGesture.PanUpdated += OnPanUpdated;
        MonthsGrid.GestureRecognizers.Add(panGesture);
        if (DeviceInfo.Current.Platform == DevicePlatform.WinUI)
        {
            PointerGestureRecognizer pointerGesture = new();
            pointerGesture.PointerExited += PointerGesture_PointerExited;
            MonthsGrid.GestureRecognizers.Add(pointerGesture);
        }
        MonthsGrid.Background = Brush.Transparent;

        PrevPath.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(PREVBUTTONDATA);
        PrevPathStk.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { PrevMonth(); }) });

        NextPath.Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(NEXTBUTTONDATA);
        NextPathStk.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => { NextMonth(); }) });


        for (int i = 0; i < 7; i++)
        {
            DaysNamesArray[i] = new CalendarDayNameView { BindingContext = this };
            DaysNamesArray[i].SetBinding(StyleProperty, nameof(DayNameStyle));
            Grid.SetRow(DaysNamesArray[i], 1);
            Grid.SetColumn(DaysNamesArray[i], i);
            TitleGrid.Add(DaysNamesArray[i]);
        }
        DaysNamesArray[0].SetBinding(StyleProperty, nameof(DayNameFirstDayOfWeekStyle));
        DaysNamesArray[6].SetBinding(StyleProperty, nameof(DayNameLastDayOfWeekStyle));

        for (int i = 0; i < 3; i++)
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    DaysArray[i, x, y] = new CalendarDayView { BindingContext = this, IsVisible = i == 1 };
                    Grid.SetRow(DaysArray[i, x, y], x);
                    Grid.SetColumn(DaysArray[i, x, y], y);
                    DaysArray[i, x, y].GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command((d) => { TapDay(d); }), CommandParameter = DaysArray[i, x, y] });
                    if (DeviceInfo.Current.Platform != DevicePlatform.WinUI)
                    {
                        panGesture = new();
                        panGesture.PanUpdated += OnPanUpdated;
                        DaysArray[i, x, y].GestureRecognizers.Add(panGesture);
                    }
                    DaysGrids[i].Add(DaysArray[i, x, y]);
                }
            }
        }
        FillMonthDays();
    }

    private void Control_SizeChanged(object sender, EventArgs e)
    {
        if (MonthLabel.Height > 0 || PrevPath.Height > 0 || NextPath.Height > 0)
        {
            TitleGrid.RowDefinitions[0].Height = Math.Max(Math.Max(MonthLabel.Height, PrevPath.Height), NextPath.Height) + 15;
        }
        BaseGrid.MinimumWidthRequest = MonthLabel.Width + PrevPath.Width + NextPath.Width + 20;
        
        if (MonthsGrid.Width > 0 && MonthsGrid.Width != monthWith)
        {
            monthWith = MonthsGrid.Width;
            columnMonthWith = monthWith / 7;
            for (int i = 0; i < 3; i++)
            {
                DaysGrids[i].WidthRequest = monthWith;
            }
            DaysGrids[0].TranslationX = -monthWith;
            DaysGrids[1].TranslationX = 0;
            DaysGrids[2].TranslationX = monthWith;
        }
        Debug.WriteLine($"Title={TitleGrid.Height} BaseStack={BaseGrid.Height} MonthsGrid={MonthsGrid.Height}");
    }

    private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
    {
        if (AllowPanGesture)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    totalPanX = 0;
                    runningPan = true;
                    break;
                case GestureStatus.Running:
                    if (runningPan)
                    {
                        totalPanX = e.TotalX;
                        DaysGrids[0].TranslationX = Math.Max(-monthWith, -monthWith + totalPanX);
                        DaysGrids[1].TranslationX = totalPanX;
                        DaysGrids[2].TranslationX = Math.Min(monthWith, monthWith + totalPanX);
                        SetCalendarColumnsVisibility(totalPanX);
                        if (Math.Abs(totalPanX) > MonthsGrid.Width / 3) EndPan(true);
                    }
                    break;
                case GestureStatus.Completed:
                case GestureStatus.Canceled:
                    if (runningPan) EndPan(Math.Abs(totalPanX) > MonthsGrid.Width / 3);
                    break;
            }
        }
    }
    private void PointerGesture_PointerExited(object sender, PointerEventArgs e)
    {
        if (runningPan) EndPan(Math.Abs(totalPanX) > MonthsGrid.Width / 2);
    }

    private void EndPan(bool changeMonth)
    {
        double initAnim = Math.Min(Math.Abs(totalPanX), MonthsGrid.Width);
        double endAnim = changeMonth ? MonthsGrid.Width : 0;
        int nextMonth = changeMonth ? (totalPanX < 0 ? 1 : -1) : 0;

        if (totalPanX < 0) 
        {
            initAnim *= -1;
            endAnim *= -1;
        }
        runningPan = false;
        var anim = new Animation(v =>
        {
            DaysGrids[0].TranslationX = Math.Max(-monthWith, -monthWith + v);
            DaysGrids[1].TranslationX = v;
            DaysGrids[2].TranslationX = Math.Min(monthWith, monthWith + v);
            SetCalendarColumnsVisibility(v);
        }, initAnim, endAnim);
        anim.Commit(this, "EndPanAnim", 16, 200, finished: (v, c) =>
        {
            if (nextMonth != 0)
            {
                CurrentMonth = CurrentMonth.AddMonths(nextMonth);
                FillMonthDays();
                DaysGrids[0].TranslationX = -monthWith;
                DaysGrids[1].TranslationX = 0;
                DaysGrids[2].TranslationX = monthWith;
            }
            SetCalendarColumnsVisibility(0);
        });

    }
    private void SetCalendarColumnsVisibility(double currentPan)
    {
        bool columnVisible;
        for (int i = 0; i < 3; i++)
        {
            for (int y = 0; y < 7; y++)
            {
                if (currentPan < 0)
                {
                    if (i == 0)
                        columnVisible = false;
                    else if (i == 2)
                        columnVisible = columnMonthWith * (y + 1) < Math.Abs(currentPan) + columnMonthWith;
                    else
                        columnVisible = columnMonthWith * (y + 1) >= Math.Abs(currentPan);
                }
                else if (currentPan > 0)
                {
                    if (i == 0)
                        columnVisible = columnMonthWith * (7 - y) < Math.Abs(currentPan) + columnMonthWith;
                    else if (i == 2)
                        columnVisible = false;
                    else
                        columnVisible = columnMonthWith * (7 - y) >= Math.Abs(currentPan);
                }
                else
                {
                    columnVisible = i == 1;
                }
                for (int x = 0; x < 6; x++)
                {
                    DaysArray[i, x, y].IsVisible = columnVisible;
                }
            }
        }

    }
    private void TapDay(object dView)
    {
        if (dView is CalendarDayView dayView && dayView.Day.Date >= MinSelectedDate.Date && dayView.Day.Date <= MaxSelectedDate.Date)
        {
            changingSelectedDate = true;
            var oldDayView = FindDate(SelectedDate);
            SelectedDate = dayView.Day;
            if (oldDayView != null) RefreshDay(oldDayView, CurrentMonth);
            if (dayView.Day.Month == CurrentMonth.Month)
                RefreshDay(dayView, CurrentMonth);
            else
            {
                CurrentMonth = dayView.Day;
                FillMonthDays();
            }
            changingSelectedDate = false;
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private void PrevMonth()
    {
        if (!animatingButton)
        {
            animatingButton = true;
            new Animation
            {
                { 0, 0.5, new Animation (v => PrevPath.Stroke = (SolidColorBrush)(ButtonsColor.WithAlpha((float)v)), 1, 0.5) },
                { 0.5, 1, new Animation (v => PrevPath.Stroke = (SolidColorBrush)(ButtonsColor.WithAlpha((float)v)), 0.5, 1) },
            }.Commit(this, "ButtonsAnim", 16, 200, null, (a, b) => { PrevPath.SetBinding(Shape.StrokeProperty, nameof(ButtonsColor)); animatingButton = false; });
        }
        totalPanX = 1;
        EndPan(true);
    }
    private void NextMonth()
    {
        if (!animatingButton)
        {
            animatingButton = true;
            new Animation
            {
                { 0, 0.5, new Animation (v => NextPath.Stroke = (SolidColorBrush)(ButtonsColor.WithAlpha((float)v)), 1, 0.5) },
                { 0.5, 1, new Animation (v => NextPath.Stroke = (SolidColorBrush)(ButtonsColor.WithAlpha((float)v)), 0.5, 1) },
            }.Commit(this, "ButtonsAnim", 16, 200, null, (a, b) => { NextPath.SetBinding(Shape.StrokeProperty, nameof(ButtonsColor)); animatingButton = false; });
        }
        totalPanX = -1;
        EndPan(true);
    }

    private void FillMonthDays()
    {
        try
        {
            MonthLabel.Text = CapName(CurrentMonth.ToString(MonthFormat, CalendarCulture));
        }
        catch
        {
            MonthLabel.Text = CapName(CurrentMonth.ToString("MMMM yy", CalendarCulture));
        }

        DateTime currentDay = new(CurrentMonth.Year, CurrentMonth.Month, 1);
        while (currentDay.DayOfWeek != FirstDayOfWeek) currentDay = currentDay.AddDays(-1);
        for (int i = 0; i < 7; i++)
        {
            DaysNamesArray[i].DayName = CalendarCulture.DateTimeFormat.ShortestDayNames[(int)currentDay.AddDays(i).DayOfWeek];
        }
        int monthIdx = -1;
        for (int i = 0; i < 3; i++)
        {
            currentDay = new(CurrentMonth.AddMonths(monthIdx).Year, CurrentMonth.AddMonths(monthIdx).Month, 1);
            while (currentDay.DayOfWeek != FirstDayOfWeek) currentDay = currentDay.AddDays(-1);

            for (int x = 0; x < 6; x++)
                for (int y = 0; y < 7; y++)
                {
                    DaysArray[i, x, y].Day = currentDay;
                    DaysArray[i, x, y].IsLastDayOfWeek = y == 6;
                    DaysArray[i, x, y].IsVisible = true;
                    RefreshDay(DaysArray[i, x, y], CurrentMonth.AddMonths(monthIdx), i != 1);
                    currentDay = currentDay.AddDays(1);
                }
            if (DaysArray[i, 5, 0].Day.Month != CurrentMonth.AddMonths(monthIdx).Month)
            {
                for (int y = 0; y < 7; y++) DaysArray[i, 5, y].IsVisible = false;
                if (DaysGrids[i].RowDefinitions.Count > 5)
                {
                    DaysGrids[i].RowDefinitions.RemoveAt(5);
                }
            }
            else if (DaysGrids[i].RowDefinitions.Count <= 5)
            {
                DaysGrids[i].RowDefinitions.Add(new());
            }

            monthIdx++;
        }
        //BaseGrid.TranslationX = 0;
    }
    private void RefreshDay(CalendarDayView dayView, DateTime refreshMonth, bool ignoreSelectedDay = false)
    {
        if (refreshMonth.Month != CurrentMonth.Month)
        {
            if (dayView.Day.Month != refreshMonth.Month || dayView.Day.Date < MinSelectedDate.Date || dayView.Day.Date > MaxSelectedDate.Date)
                dayView.SetBinding(StyleProperty, nameof(OtherMonthDayStyle));
            else if (dayView.Day.DayOfWeek == FirstDayOfWeek)
                dayView.SetBinding(StyleProperty, nameof(FirstDayOfWeekStyle));
            else if (dayView.IsLastDayOfWeek)
                dayView.SetBinding(StyleProperty, nameof(LastDayOfWeekStyle));
            else
                dayView.SetBinding(StyleProperty, nameof(DayStyle));
        }
        else if (dayView.Day.Date == SelectedDate?.Date && !ignoreSelectedDay)
        {
            dayView.SetBinding(StyleProperty, nameof(SelectedDayStyle));
        }
        else if (dayView.Day.Month != refreshMonth.Month || dayView.Day.Date < MinSelectedDate.Date || dayView.Day.Date > MaxSelectedDate.Date)
        {
            dayView.SetBinding(StyleProperty, nameof(OtherMonthDayStyle));
        }
        else
        {
            var eventInfo = CalendarEvents?.Where(e => e.Date.Date == dayView.Day.Date);
            if (eventInfo != null && eventInfo.Any())
            {
                if (eventInfo.Any(e => e.Type == CalendarEventType.Event1))
                    dayView.SetBinding(StyleProperty, nameof(Event1DayStyle));
                else if (eventInfo.Any(e => e.Type == CalendarEventType.Event2))
                    dayView.SetBinding(StyleProperty, nameof(Event2DayStyle));
                else
                    dayView.SetBinding(StyleProperty, nameof(Event3DayStyle));
            }
            else if (dayView.Day.Date == DateTime.Today.Date)
            {
                dayView.SetBinding(StyleProperty, nameof(TodayStyle));
            }
            else
            {
                if (dayView.Day.DayOfWeek == FirstDayOfWeek)
                    dayView.SetBinding(StyleProperty, nameof(FirstDayOfWeekStyle));
                else if (dayView.IsLastDayOfWeek)
                    dayView.SetBinding(StyleProperty, nameof(LastDayOfWeekStyle));
                else
                    dayView.SetBinding(StyleProperty, nameof(DayStyle));
            }
        }
    }
    private CalendarDayView FindDate(DateTime? date)
    {
        CalendarDayView dayView = null;

        if (date != null)
        {
            for (int x = 0; x < 6; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    if (DaysArray[1, x, y].Day.Date == date?.Date) dayView = DaysArray[1, x, y];
                    if (dayView != null) break;
                }
                if (dayView != null) break;
            }
        }

        return dayView;
    }
    private static void SelectedDateChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            var cView = bindable as CalendarView;
            if (!cView.changingSelectedDate)
            {
                if (oldValue is DateTime oldDate)
                {
                    var oldDayView = cView.FindDate(oldDate);
                    if (oldDayView != null) cView.RefreshDay(oldDayView, cView.CurrentMonth, true);
                }
                if (newValue is DateTime newDate)
                {
                    var newDayView = cView.FindDate(newDate);
                    if (newDayView != null)
                    {
                        if (newDayView.Day.Month == cView.CurrentMonth.Month)
                            cView.RefreshDay(newDayView, cView.CurrentMonth);
                        else
                        {
                            cView.CurrentMonth = newDayView.Day;
                            cView.FillMonthDays();
                        }
                    }
                }
            }
        }
    }
    private static void CalendarEventsChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (oldValue != newValue)
        {
            CalendarView calendar = bindable as CalendarView;
            if (newValue != null && newValue is INotifyCollectionChanged events)
            {
                events.CollectionChanged += calendar.Events_CollectionChanged;
            }
            if (oldValue != null && oldValue is INotifyCollectionChanged eventsOld)
            {
                eventsOld.CollectionChanged -= calendar.Events_CollectionChanged;
            }
            calendar.FillMonthDays();
        }
    }

    private void Events_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        FillMonthDays();
    }

    private static void DefineDefaultStyles()
    {
        if (!STYLESINITIATED)
        {
            BORDERSTYLE.Setters.Add(MarginProperty, new Thickness(10));
            BORDERSTYLE.Setters.Add(BackgroundProperty, Brush.Transparent);

            MONTHSTYLE.Setters.Add(HorizontalOptionsProperty, LayoutOptions.Center);
            MONTHSTYLE.Setters.Add(VerticalOptionsProperty, LayoutOptions.Center);
            MONTHSTYLE.Setters.Add(Label.FontSizeProperty, 15d);
            MONTHSTYLE.Setters.Add(Label.FontAttributesProperty, FontAttributes.Bold);
            MONTHSTYLE.Setters.Add(Label.TextColorProperty, Colors.Black);

            OTHERMONTHDAYSTYLE.Setters.Add(CalendarDayView.DayColorProperty, Colors.LightGrey);
            OTHERMONTHDAYSTYLE.Setters.Add(CalendarDayView.DayFontAttributesProperty, FontAttributes.Italic);

            
            SELECTEDDAYSTYLE.Setters.Add(CalendarDayView.DayColorProperty, Colors.Orange);
            SELECTEDDAYSTYLE.Setters.Add(CalendarDayView.DayFontAttributesProperty, FontAttributes.Bold);
            SELECTEDDAYSTYLE.Setters.Add(CalendarDayView.DayTagStrokeColorProperty, Brush.Orange);
            SELECTEDDAYSTYLE.Setters.Add(CalendarDayView.DayFontSizeProperty, 13d);
            SELECTEDDAYSTYLE.Setters.Add(CalendarDayView.DayTagIsVisibleProperty, true);

            TODAYSTYLE.Setters.Add(CalendarDayView.DayColorProperty, Colors.Blue);
            TODAYSTYLE.Setters.Add(CalendarDayView.DayFontAttributesProperty, FontAttributes.Bold);

            EVENT1DAYSTYLE.Setters.Add(CalendarDayView.DayColorProperty, Colors.White);
            EVENT1DAYSTYLE.Setters.Add(CalendarDayView.DayFontAttributesProperty, FontAttributes.Bold);
            EVENT1DAYSTYLE.Setters.Add(CalendarDayView.DayTagFillColorProperty, Brush.Orange);
            EVENT1DAYSTYLE.Setters.Add(CalendarDayView.DayTagIsVisibleProperty, true);

            EVENT2DAYSTYLE.Setters.Add(CalendarDayView.DayColorProperty, Colors.White);
            EVENT2DAYSTYLE.Setters.Add(CalendarDayView.DayFontAttributesProperty, FontAttributes.Bold);
            EVENT2DAYSTYLE.Setters.Add(CalendarDayView.DayTagFillColorProperty, Brush.Green);
            EVENT2DAYSTYLE.Setters.Add(CalendarDayView.DayTagIsVisibleProperty, true);

            EVENT3DAYSTYLE.Setters.Add(CalendarDayView.DayColorProperty, Colors.White);
            EVENT3DAYSTYLE.Setters.Add(CalendarDayView.DayFontAttributesProperty, FontAttributes.Bold);
            EVENT3DAYSTYLE.Setters.Add(CalendarDayView.DayTagFillColorProperty, Brush.Brown);
            EVENT3DAYSTYLE.Setters.Add(CalendarDayView.DayTagIsVisibleProperty, true);

            STYLESINITIATED = true;
        }
    }

    private static string CapName(string baseName)
    {
        if (baseName?.Length > 0)
            return char.ToUpper(baseName[0]) + baseName[1..];
        else
            return baseName;
    }
}