using System.Windows;

namespace WPFCommon
{
    public delegate void TextBlockClickEventHandler(object sender, TimeSelecterClickEventArgs e);

    public enum TimeSelecterClieckGroup
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
    }

    public class TimeSelecterClickEventArgs : RoutedEventArgs
    {
        public TimeSelecterClickEventArgs(RoutedEvent revent, TimeSelecterClieckGroup group, int? selectedNumber) : base(revent)
        {
            this.SelectedNumber = selectedNumber;
            this.ClickGroup = group;
        }

        public TimeSelecterClieckGroup ClickGroup { get; set; }

        public int? SelectedNumber { get; set; }
    }
}
