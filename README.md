# Calendar.MAUI

#### Create a Calendar in your code
```C#
using CalendarView.MAUI;

...

public CalendarView MyCalendar { get; set; } = new CalendarView();

...

MyCalendar.SelectionChanged += MyCalendar_SelectionChanged;

...

    private void MyCalendar_SelectionChanged(object sender, EventArgs e)
    {
        Debug.WriteLine($"Selected day = {MyCalendar.SelectedDate}");
    }

```


#### Add the following xmlns to your page or view
```xaml
xmlns:cc="clr-namespace:CalendarView.MAUI;assembly=CalendarView.MAUI"
```

#### Use and custom the calendar in your xaml
```xaml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:cc="clr-namespace:CalendarView.MAUI;assembly=CalendarView.MAUI"
             x:Name="mainPage"
             x:Class="Calendar.MAUI.Test.MainPage" Background="white">
    <ContentPage.Resources>
        <Style TargetType="Label" x:Key="CalendarMonthStyle">
            <Setter Property="HorizontalOptions" Value="Center"/>
            <Setter Property="VerticalOptions" Value="Center"/>
            <Setter Property="TextColor" Value="White"/>
            <Setter Property="FontSize" Value="15"/>
            <Setter Property="FontAttributes" Value="Bold"/>
        </Style>
        <Style TargetType="cc:CalendarDayView" x:Key="CalendarEvent1">
            <Setter Property="DayTagStrokeThickness" Value="0"/>
            <Setter Property="DayTagFillColor" Value="orange"/>
            <Setter Property="DayTagWidthRequest" Value="8"/>
            <Setter Property="DayTagHeightRequest" Value="8"/>
            <Setter Property="DayTagHorizontalOptions" Value="Center"/>
            <Setter Property="DayTagVerticalOptions" Value="End"/>
            <Setter Property="DayTagMargin" Value="0,0,0,3"/>
            <Setter Property="DayTagIsVisible" Value="True"/>
        </Style>
        <Style TargetType="cc:CalendarDayView" x:Key="CalendarEvent2">
            <Setter Property="DayTagStrokeThickness" Value="0"/>
            <Setter Property="DayTagFillColor" Value="green"/>
            <Setter Property="DayTagWidthRequest" Value="8"/>
            <Setter Property="DayTagHeightRequest" Value="8"/>
            <Setter Property="DayTagHorizontalOptions" Value="Center"/>
            <Setter Property="DayTagVerticalOptions" Value="End"/>
            <Setter Property="DayTagMargin" Value="0,0,0,3"/>
            <Setter Property="DayTagIsVisible" Value="True"/>
        </Style>
        <Style TargetType="cc:CalendarDayView" x:Key="CalendarEvent3">
            <Setter Property="DayTagStrokeThickness" Value="0"/>
            <Setter Property="DayTagFillColor" Value="brown"/>
            <Setter Property="DayTagWidthRequest" Value="8"/>
            <Setter Property="DayTagHeightRequest" Value="8"/>
            <Setter Property="DayTagHorizontalOptions" Value="Center"/>
            <Setter Property="DayTagVerticalOptions" Value="End"/>
            <Setter Property="DayTagMargin" Value="0,0,0,3"/>
            <Setter Property="DayTagIsVisible" Value="True"/>
        </Style>
        <Style TargetType="cc:CalendarDayView" x:Key="CalendarFirstDayOfWeek">
            <Setter Property="DayColor" Value="Red"/>
        </Style>
        <Style TargetType="cc:CalendarDayNameView" x:Key="CalendarFirstDayOfWeekName">
            <Setter Property="DayNameColor" Value="Red"/>
        </Style>
    </ContentPage.Resources>

    <Grid>
        <Grid.RowDefinitions>
            <RowDefinition Height="*"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>
        <cc:CalendarView x:Name="EN_Calendar" BindingContext="{x:Reference mainPage}" Grid.Row="0" Background="white"
                         CalendarEvents="{Binding Events}" WidthRequest="300" HeightRequest="300"
                         MonthStyle="{StaticResource Key=CalendarMonthStyle}" MinSelectedDate="{Binding MinDay}" SelectedDate="{Binding SelectedDay, Mode=TwoWay}"
                         HeaderBackground="blue" MonthFormat="MMMM yyyy" ButtonsColor="White"
                         CalendarCulture="{Binding CalendarCulture}" FirstDayOfWeekStyle="{StaticResource Key=CalendarFirstDayOfWeek}" DayNameFirstDayOfWeekStyle="{StaticResource Key=CalendarFirstDayOfWeekName}"
                         Event1DayStyle="{StaticResource Key=CalendarEvent1}" Event2DayStyle="{StaticResource Key=CalendarEvent2}" Event3DayStyle="{StaticResource Key=CalendarEvent3}"
                         ShowChangeMonthArrows="True" ButtonsScale="1" IsVisible="true"/>
        <cc:CalendarView Grid.Row="1" BindingContext="{x:Reference mainPage}" CalendarEvents="{Binding Events}" ShowChangeMonthArrows="False" Background="white"
                         WidthRequest="300" HeightRequest="300" SelectedDate="{Binding SelectedDay, Mode=TwoWay}"
                         LastDayOfWeekStyle="{StaticResource Key=CalendarFirstDayOfWeek}" DayNameLastDayOfWeekStyle="{StaticResource Key=CalendarFirstDayOfWeekName}"/>
    </Grid>

</ContentPage>
```
