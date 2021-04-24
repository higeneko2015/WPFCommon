using System;
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
        // ドラッグイベントが用意されていて便利なのでThumbコントロールを利用します。
        private readonly Thumb _resizeGrip;

        private readonly VisualCollection _visualChildren;

        public Gripper(UIElement adornedElement, ControlTemplate controlTemplate) : base(adornedElement)
        {
            _resizeGrip = new Thumb
            {
                Cursor = Cursors.SizeNWSE
            };
            _resizeGrip.SetValue(WidthProperty, 18d);
            _resizeGrip.SetValue(HeightProperty, 18d);
            _resizeGrip.DragDelta += OnGripDelta;
            _resizeGrip.Template = controlTemplate ?? MakeDefaultGripTemplate();
            //_resizeGrip.MouseEnter += this._resizeGrip_MouseEnter;
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
            }
            return finalSize;
        }

        protected override Visual GetVisualChild(int index)
        {
            return _visualChildren[index];
        }

        //private void _resizeGrip_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    var x = 0;
        //}

        private ControlTemplate MakeDefaultGripTemplate()
        {
            //! 指定なしの場合の見た目を作成
            //var visualTree = new FrameworkElementFactory(typeof(Border));
            //visualTree.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            //visualTree.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            //visualTree.SetValue(WidthProperty, 12d);
            //visualTree.SetValue(HeightProperty, 12d);
            //visualTree.SetValue(Border.BackgroundProperty, new SolidColorBrush(Colors.RoyalBlue));
            //visualTree.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
            var p1 = new FrameworkElementFactory(typeof(Path));
            p1.SetValue(Path.FillProperty, new SolidColorBrush(Colors.White));
            p1.SetValue(Path.DataProperty, Geometry.Parse("M0,14L14,0L14,14z"));
            var p2 = new FrameworkElementFactory(typeof(Path));
            p2.SetValue(Path.StrokeProperty, new SolidColorBrush(Colors.LightGray));
            p2.SetValue(Path.DataProperty, Geometry.Parse("M0,14L14,0"));
            var p3 = new FrameworkElementFactory(typeof(Path));
            p3.SetValue(Path.StrokeProperty, new SolidColorBrush(Colors.LightGray));
            p3.SetValue(Path.DataProperty, Geometry.Parse("M4,14L14,4"));
            var p4 = new FrameworkElementFactory(typeof(Path));
            p4.SetValue(Path.StrokeProperty, new SolidColorBrush(Colors.LightGray));
            p4.SetValue(Path.DataProperty, Geometry.Parse("M8,14L14,8"));
            var p5 = new FrameworkElementFactory(typeof(Path));
            p5.SetValue(Path.StrokeProperty, new SolidColorBrush(Colors.LightGray));
            p5.SetValue(Path.DataProperty, Geometry.Parse("M12,14L14,12"));

            var grid = new FrameworkElementFactory(typeof(Grid));
            grid.SetValue(Grid.MarginProperty, new Thickness(2));
            grid.AppendChild(p1);
            grid.AppendChild(p2);
            grid.AppendChild(p3);
            grid.AppendChild(p4);
            //grid.AppendChild(p5);

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
                w += e.HorizontalChange;
                h += e.VerticalChange;
                // clamp
                w = Math.Max(_resizeGrip.Width, w);
                h = Math.Max(_resizeGrip.Height, h);
                w = Math.Max(frameworkElement.MinWidth, w);
                h = Math.Max(frameworkElement.MinHeight, h);
                w = Math.Min(frameworkElement.MaxWidth, w);
                h = Math.Min(frameworkElement.MaxHeight, h);
                // ※ = で入れるとBindingが外れるので注意
                frameworkElement.SetValue(WidthProperty, w);
                frameworkElement.SetValue(HeightProperty, h);
            }
        }
    }
}
