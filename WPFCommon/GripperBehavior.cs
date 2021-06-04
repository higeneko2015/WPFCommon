using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Documents;

namespace WPFCommon
{
    public class GripperBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.IsLoaded)
            {
                AdornerLayer.GetAdornerLayer(AssociatedObject)?.Add(new Gripper(AssociatedObject, null));
            }
            else
            {
                AssociatedObject.Loaded += (s, e) =>
                {
                    AdornerLayer.GetAdornerLayer(AssociatedObject)?.Add(new Gripper(AssociatedObject, null));
                };
            }
        }
    }
}
