namespace CalendarView.MAUI;

public class CalendarDayNameView : ContentView
{
    public static readonly BindableProperty DayNameColorProperty = BindableProperty.Create(nameof(DayNameColor), typeof(Color), typeof(CalendarDayNameView), Colors.Black);
    public static readonly BindableProperty DayNameFontSizeProperty = BindableProperty.Create(nameof(DayNameFontSize), typeof(double), typeof(CalendarDayNameView), 12d);
    public static readonly BindableProperty DayNameFontAttributesProperty = BindableProperty.Create(nameof(DayNameFontAttributes), typeof(FontAttributes), typeof(CalendarDayNameView), FontAttributes.Bold);
    public static readonly BindableProperty DayNameHorizontalOptionsProperty = BindableProperty.Create(nameof(DayNameHorizontalOptions), typeof(LayoutOptions), typeof(CalendarDayNameView), LayoutOptions.Center);
    public static readonly BindableProperty DayNameVerticalOptionsProperty = BindableProperty.Create(nameof(DayNameVerticalOptions), typeof(LayoutOptions), typeof(CalendarDayNameView), LayoutOptions.Start);
    public static readonly BindableProperty DayNameMarginProperty = BindableProperty.Create(nameof(DayNameMargin), typeof(Thickness), typeof(CalendarDayNameView), new Thickness(0));
    public static readonly BindableProperty DayNameBackgroundProperty = BindableProperty.Create(nameof(DayNameBackground), typeof(Brush), typeof(CalendarDayNameView), null);

    public Color DayNameColor { get => (Color)GetValue(DayNameColorProperty); set => SetValue(DayNameColorProperty, value); }
    public double DayNameFontSize { get => (double)GetValue(DayNameFontSizeProperty); set => SetValue(DayNameFontSizeProperty, value); }
    public FontAttributes DayNameFontAttributes { get => (FontAttributes)GetValue(DayNameFontAttributesProperty); set => SetValue(DayNameFontAttributesProperty, value); }
    public LayoutOptions DayNameHorizontalOptions { get => (LayoutOptions)GetValue(DayNameHorizontalOptionsProperty); set => SetValue(DayNameHorizontalOptionsProperty, value); }
    public LayoutOptions DayNameVerticalOptions { get => (LayoutOptions)GetValue(DayNameVerticalOptionsProperty); set => SetValue(DayNameVerticalOptionsProperty, value); }
    public Thickness DayNameMargin { get => (Thickness)GetValue(DayNameMarginProperty); set => SetValue(DayNameMarginProperty, value); }
    public Brush DayNameBackground { get => (Brush)GetValue(DayNameBackgroundProperty); set => SetValue(DayNameBackgroundProperty, value); }
    
    private readonly Grid BaseGrid;
    private Label DayNameLabel { get; set; }
    private string dayName = string.Empty;
    public string DayName
    {
        get => dayName;
        set
        {
            dayName = value;
            DayNameLabel.Text = dayName;
        }
    }

    public CalendarDayNameView() 
    {
        BaseGrid = new Grid { BindingContext = this, HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
        BaseGrid.SetBinding(BackgroundProperty, nameof(DayNameBackground));
        DayNameLabel = new Label { BindingContext = this };
        DayNameLabel.SetBinding(HorizontalOptionsProperty, nameof(DayNameHorizontalOptions));
        DayNameLabel.SetBinding(VerticalOptionsProperty, nameof(DayNameVerticalOptions));
        DayNameLabel.SetBinding(Label.FontSizeProperty, nameof(DayNameFontSize));
        DayNameLabel.SetBinding(Label.FontAttributesProperty, nameof(DayNameFontAttributes));
        DayNameLabel.SetBinding(Label.FontSizeProperty, nameof(DayNameFontSize));
        DayNameLabel.SetBinding(Label.TextColorProperty, nameof(DayNameColor));
        DayNameLabel.SetBinding(MarginProperty, nameof(DayNameMargin));

        BaseGrid.Add(DayNameLabel);
        Content = BaseGrid;
    }
}
