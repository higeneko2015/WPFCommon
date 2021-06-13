using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WPFCommon
{
    public class Gripper : Adorner
    {
        protected double _raito = 0.0;

        // ドラッグイベントが用意されていて便利なのでThumbコントロールを利用します。
        private readonly Thumb _resizeGrip;

        private readonly VisualCollection _visualChildren;

        private double initHeight = 0.0;
        private double initWidth = 0.0;

        public Gripper(UIElement adornedElement, ControlTemplate controlTemplate) : base(adornedElement)
        {
            _resizeGrip = new Thumb
            {
                Cursor = Cursors.SizeNWSE
            };
            _resizeGrip.SetValue(WidthProperty, 15d);
            _resizeGrip.SetValue(HeightProperty, 15d);
            _resizeGrip.DragDelta += OnGripDelta;

            _resizeGrip.Template = controlTemplate ?? MakeDefaultGripTemplate();
            _visualChildren = new VisualCollection(this)
            {
                _resizeGrip
            };
        }

        protected override int VisualChildrenCount => _visualChildren.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (AdornedElement is FrameworkElement frameworkElement)
            {
                var w = _resizeGrip.Width;
                var h = _resizeGrip.Height;
                var x = frameworkElement.ActualWidth - w;
                var y = frameworkElement.ActualHeight - h;
                _resizeGrip.Arrange(new Rect(x, y, w, h));

                if (initHeight == 0.0)
                {
                    initHeight = frameworkElement.ActualHeight;
                    initWidth = frameworkElement.ActualWidth;
                }

                _raito = frameworkElement.ActualWidth / frameworkElement.ActualHeight;
            }
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }

        private ControlTemplate MakeDefaultGripTemplate()
        {
            var p1 = new FrameworkElementFactory(typeof(Path));
            p1.SetValue(Path.FillProperty, new SolidColorBrush(Colors.Red));
            p1.SetValue(Path.DataProperty, Geometry.Parse("M 10,10 L 0,10 L 10,0 Z"));
            p1.SetValue(Path.StrokeThicknessProperty, 0d);

            var grid = new FrameworkElementFactory(typeof(Grid));
            grid.SetValue(Grid.WidthProperty, 15d);
            grid.SetValue(Grid.HeightProperty, 15d);
            grid.SetValue(Grid.MarginProperty, new Thickness(2));
            grid.AppendChild(p1);

            return new ControlTemplate(typeof(Thumb))
            {
                VisualTree = grid
            };
        }

        // Thumb のドラッグイベントです。
        private void OnGripDelta(object sender, DragDeltaEventArgs e)
        {
            if (AdornedElement is FrameworkElement frameworkElement)
            {
                var w = frameworkElement.Width;
                var h = frameworkElement.Height;
                if (w.Equals(double.NaN))
                {
                    w = frameworkElement.DesiredSize.Width;
                }
                if (h.Equals(double.NaN))
                {
                    h = frameworkElement.DesiredSize.Height;
                }

                if (e.HorizontalChange > e.VerticalChange)
                {
                    // 横方向の移動が多い場合
                    w += e.HorizontalChange;
                    h = w / _raito;
                }
                else
                {
                    // 縦方向の移動が多い場合
                    h += e.VerticalChange;
                    w = h * _raito;
                }
                if (h < initHeight || w < initWidth)
                {
                    w = initWidth;
                    h = initHeight;
                }
                frameworkElement.SetValue(WidthProperty, w);
                frameworkElement.SetValue(HeightProperty, h);
            }
        }
    }
}
