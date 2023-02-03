using Microsoft.Maui.Controls.Shapes;

namespace CalendarView.MAUI;

public class CalendarDayView : ContentView
{
    public static readonly BindableProperty DayColorProperty = BindableProperty.Create(nameof(DayColor), typeof(Color), typeof(CalendarDayView), Colors.Black);
    public static readonly BindableProperty DayFontSizeProperty = BindableProperty.Create(nameof(DayFontSize), typeof(double), typeof(CalendarDayView), 12d);
    public static readonly BindableProperty DayFontAttributesProperty = BindableProperty.Create(nameof(DayFontAttributes), typeof(FontAttributes), typeof(CalendarDayView), FontAttributes.None);
    public static readonly BindableProperty DayLabelHorizontalOptionsProperty = BindableProperty.Create(nameof(DayLabelHorizontalOptions), typeof(LayoutOptions), typeof(CalendarDayView), LayoutOptions.Fill);
    public static readonly BindableProperty DayLabelVerticalOptionsProperty = BindableProperty.Create(nameof(DayLabelVerticalOptions), typeof(LayoutOptions), typeof(CalendarDayView), LayoutOptions.Fill);
    public static readonly BindableProperty DayHorizontalTextAlignmentProperty = BindableProperty.Create(nameof(DayHorizontalTextAlignment), typeof(TextAlignment), typeof(CalendarDayView), TextAlignment.Center);
    public static readonly BindableProperty DayVerticalTextAlignmentProperty = BindableProperty.Create(nameof(DayVerticalTextAlignment), typeof(TextAlignment), typeof(CalendarDayView), TextAlignment.Center);
    public static readonly BindableProperty DayMarginProperty = BindableProperty.Create(nameof(DayMargin), typeof(Thickness), typeof(CalendarDayView), new Thickness(0));

    public static readonly BindableProperty DayTagIsVisibleProperty = BindableProperty.Create(nameof(DayTagIsVisible), typeof(bool), typeof(CalendarDayView), false);
    public static readonly BindableProperty DayTagStrokeThicknessProperty = BindableProperty.Create(nameof(DayTagStrokeThickness), typeof(double), typeof(CalendarDayView), 1.5d);
    public static readonly BindableProperty DayTagStrokeColorProperty = BindableProperty.Create(nameof(DayTagStrokeColor), typeof(Brush), typeof(CalendarDayView), null);
    public static readonly BindableProperty DayTagFillColorProperty = BindableProperty.Create(nameof(DayTagFillColor), typeof(Brush), typeof(CalendarDayView), null);
    public static readonly BindableProperty DayTagHorizontalOptionsProperty = BindableProperty.Create(nameof(DayTagHorizontalOptions), typeof(LayoutOptions), typeof(CalendarDayView), LayoutOptions.Fill);
    public static readonly BindableProperty DayTagVerticalOptionsProperty = BindableProperty.Create(nameof(DayTagVerticalOptions), typeof(LayoutOptions), typeof(CalendarDayView), LayoutOptions.Fill);
    public static readonly BindableProperty DayTagMarginProperty = BindableProperty.Create(nameof(DayTagMargin), typeof(Thickness), typeof(CalendarDayView), new Thickness(1));
    public static readonly BindableProperty DayTagWidthRequestProperty = BindableProperty.Create(nameof(DayTagWidthRequest), typeof(double), typeof(CalendarDayView), -1d);
    public static readonly BindableProperty DayTagHeightRequestProperty = BindableProperty.Create(nameof(DayTagHeightRequest), typeof(double), typeof(CalendarDayView), -1d);

    public Color DayColor { get => (Color)GetValue(DayColorProperty); set => SetValue(DayColorProperty, value); }
    public double DayFontSize { get => (double)GetValue(DayFontSizeProperty); set => SetValue(DayFontSizeProperty, value); }
    public FontAttributes DayFontAttributes { get => (FontAttributes)GetValue(DayFontAttributesProperty); set => SetValue(DayFontAttributesProperty, value); }
    public LayoutOptions DayLabelHorizontalOptions { get => (LayoutOptions)GetValue(DayLabelHorizontalOptionsProperty); set => SetValue(DayLabelHorizontalOptionsProperty, value); }
    public LayoutOptions DayLabelVerticalOptions { get => (LayoutOptions)GetValue(DayLabelVerticalOptionsProperty); set => SetValue(DayLabelVerticalOptionsProperty, value); }
    public TextAlignment DayHorizontalTextAlignment { get => (TextAlignment)GetValue(DayHorizontalTextAlignmentProperty); set => SetValue(DayHorizontalTextAlignmentProperty, value); }
    public TextAlignment DayVerticalTextAlignment { get => (TextAlignment)GetValue(DayVerticalTextAlignmentProperty); set => SetValue(DayVerticalTextAlignmentProperty, value); }
    public Thickness DayMargin { get => (Thickness)GetValue(DayMarginProperty); set => SetValue(DayMarginProperty, value); }

    public bool DayTagIsVisible { get => (bool)GetValue(DayTagIsVisibleProperty); set => SetValue(DayTagIsVisibleProperty, value); }
    public double DayTagStrokeThickness { get => (double)GetValue(DayTagStrokeThicknessProperty); set => SetValue(DayTagStrokeThicknessProperty, value); }
    public Brush DayTagStrokeColor { get => (Brush)GetValue(DayTagStrokeColorProperty); set => SetValue(DayTagStrokeColorProperty, value); }
    public Brush DayTagFillColor { get => (Brush)GetValue(DayTagFillColorProperty); set => SetValue(DayTagFillColorProperty, value); }
    public LayoutOptions DayTagHorizontalOptions { get => (LayoutOptions)GetValue(DayTagHorizontalOptionsProperty); set => SetValue(DayTagHorizontalOptionsProperty, value); }
    public LayoutOptions DayTagVerticalOptions { get => (LayoutOptions)GetValue(DayTagVerticalOptionsProperty); set => SetValue(DayTagVerticalOptionsProperty, value); }
    public Thickness DayTagMargin { get => (Thickness)GetValue(DayTagMarginProperty); set => SetValue(DayMarginProperty, value); }
    public double DayTagWidthRequest { get => (double)GetValue(DayTagWidthRequestProperty); set => SetValue(DayTagWidthRequestProperty, value); }
    public double DayTagHeightRequest { get => (double)GetValue(DayTagHeightRequestProperty); set => SetValue(DayTagHeightRequestProperty, value); }

    public bool IsLastDayOfWeek = false;
    private Grid BaseGrid { get; set; }
    private Label DayLabel { get; set; }
    private Ellipse DayTag { get; set; }

    private DateTime day;
    public DateTime Day
    {
        get => day;
        set
        {
            day = value;
            DayLabel.Text = day.Day.ToString();
        }
    }

    public CalendarDayView() 
    {
        BaseGrid = new Grid { HorizontalOptions = LayoutOptions.Fill, VerticalOptions = LayoutOptions.Fill };
        DayLabel = new Label { BindingContext = this };
        DayLabel.SetBinding(HorizontalOptionsProperty, nameof(DayLabelHorizontalOptions));
        DayLabel.SetBinding(VerticalOptionsProperty, nameof(DayLabelVerticalOptions));
        DayLabel.SetBinding(Label.HorizontalTextAlignmentProperty, nameof(DayHorizontalTextAlignment));
        DayLabel.SetBinding(Label.VerticalTextAlignmentProperty, nameof(DayVerticalTextAlignment));
        DayLabel.SetBinding(Label.FontSizeProperty, nameof(DayFontSize));
        DayLabel.SetBinding(Label.FontAttributesProperty, nameof(DayFontAttributes));
        DayLabel.SetBinding(Label.FontSizeProperty, nameof(DayFontSize));
        DayLabel.SetBinding(Label.TextColorProperty, nameof(DayColor));
        DayLabel.SetBinding(MarginProperty, nameof(DayMargin));

        DayTag = new Ellipse { BindingContext = this };
        DayTag.SetBinding(IsVisibleProperty, nameof(DayTagIsVisible));
        DayTag.SetBinding(HorizontalOptionsProperty, nameof(DayTagHorizontalOptions));
        DayTag.SetBinding(VerticalOptionsProperty, nameof(DayTagVerticalOptions));
        DayTag.SetBinding(MarginProperty, nameof(DayTagMargin));
        DayTag.SetBinding(Shape.StrokeThicknessProperty, nameof(DayTagStrokeThickness));
        DayTag.SetBinding(Shape.StrokeProperty, nameof(DayTagStrokeColor));
        DayTag.SetBinding(Shape.FillProperty, nameof(DayTagFillColor));
        DayTag.SetBinding(WidthRequestProperty, nameof(DayTagWidthRequest));
        DayTag.SetBinding(HeightRequestProperty, nameof(DayTagHeightRequest));

        BaseGrid.Add(DayTag);
        BaseGrid.Add(DayLabel);

        Content = BaseGrid;
    }
}
