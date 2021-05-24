using System.Windows;

namespace WPFCommon
{
    public delegate void TextBlockClickEventHandler(object sender, TimeSelecterClickEventArgs e);

    public enum TimeSelecterClickGroup
    {
        Hours,
        Minutes,
        Seconds
    }

    public static class EventHelper
    {
        public static readonly RoutedEvent TextBlockClickEvent = EventManager.RegisterRoutedEvent(
            nameof(TextBlockClickEvent),
            RoutingStrategy.Bubble,
            typeof(TextBlockClickEventHandler),
            typeof(EventHelper)
        );

        public static readonly RoutedEvent TimeSelecterChangeEvent = EventManager.RegisterRoutedEvent(
            nameof(TimeSelecterChangeEvent),
            RoutingStrategy.Bubble,
            typeof(RoutedEventHandler),
            typeof(EventHelper)
        );
    }

    public class TimeSelecterClickEventArgs : RoutedEventArgs
    {
        public TimeSelecterClickEventArgs(RoutedEvent revent, TimeSelecterClickGroup group, int? selectedNumber) : base(revent)
        {
            this.SelectedNumber = selectedNumber;
            this.ClickGroup = group;
        }

        public TimeSelecterClickGroup ClickGroup { get; set; }

        public int? SelectedNumber { get; set; }
    }
}
