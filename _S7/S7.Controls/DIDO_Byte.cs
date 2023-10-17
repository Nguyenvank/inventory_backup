using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace S7.Controls
{
    /// <summary>
    /// Interaktionslogik für DIDO_Byte.xaml
    /// </summary>
    public class DIDO_Byte : UserControl
    {
        private Border bit0 = new Border();
        private Border bit1 = new Border();
        private Border bit2 = new Border();
        private Border bit3 = new Border();
        private Border bit4 = new Border();
        private Border bit5 = new Border();
        private Border bit6 = new Border();
        private Border bit7 = new Border();

        public static DependencyProperty ValueProperty;
        static DIDO_Byte()
        {
            ValueProperty = DependencyProperty.Register("Value",
                typeof(int),
                typeof(DIDO_Byte),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueChanged)));
        }

        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            int v = (int)e.NewValue;

            if ((v & 1) == 0)
                ((DIDO_Byte)o).bit0.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit0.Background = Brushes.Lime;

            if ((v & 2) == 0)
                ((DIDO_Byte)o).bit1.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit1.Background = Brushes.Lime;

            if ((v & 4) == 0)
                ((DIDO_Byte)o).bit2.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit2.Background = Brushes.Lime;

            if ((v & 8) == 0)
                ((DIDO_Byte)o).bit3.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit3.Background = Brushes.Lime;

            if ((v & 16) == 0)
                ((DIDO_Byte)o).bit4.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit4.Background = Brushes.Lime;

            if ((v & 32) == 0)
                ((DIDO_Byte)o).bit5.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit5.Background = Brushes.Lime;

            if ((v & 64) == 0)
                ((DIDO_Byte)o).bit6.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit6.Background = Brushes.Lime;

            if ((v & 128) == 0)
                ((DIDO_Byte)o).bit7.Background = Brushes.Transparent;
            else
                ((DIDO_Byte)o).bit7.Background = Brushes.Lime;
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Height = 112;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);
            
            int thickness = (int)((sizeInfo.NewSize.Height - 80) / 16);
            StackPanel panel = this.Content as StackPanel;
            foreach (StackPanel p in panel.Children)
            {
                p.Margin = new Thickness(0, thickness, 0, thickness);
            }
        }

        public DIDO_Byte()
        {
            SnapsToDevicePixels = true;
            int panelHeight = (int)(Height / 9);

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Vertical;
            panel.Children.Add(GetStackPanel(bit0, "0"));
            panel.Children.Add(GetStackPanel(bit1, "1"));
            panel.Children.Add(GetStackPanel(bit2, "2"));
            panel.Children.Add(GetStackPanel(bit3, "3"));
            panel.Children.Add(GetStackPanel(bit4, "4"));
            panel.Children.Add(GetStackPanel(bit5, "5"));
            panel.Children.Add(GetStackPanel(bit6, "6"));
            panel.Children.Add(GetStackPanel(bit7, "7"));

            this.Content = panel;
        }

        StackPanel GetStackPanel(Border border, string text)
        {
            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Margin = new Thickness(0, 2, 0, 2);

            border.BorderBrush = Brushes.Black;
            border.BorderThickness = new Thickness(1);
            border.Background = Brushes.Transparent;
            border.Height = 10;
            border.Width = 10;
            panel.Children.Add(border);

            Label label = new Label();
            label.Content = text;
            label.VerticalContentAlignment = VerticalAlignment.Center;
            label.Width = 10;
            label.Padding = new Thickness(2, 0, 0, 0);
            panel.Children.Add(label);

            return panel;
        }
    }
}
