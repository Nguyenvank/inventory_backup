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

namespace S7.Controls.S7300
{
    /// <summary>
    /// Interaktionslogik für S7300_DI16DO16.xaml
    /// </summary>
    public partial class S7300_08 : UserControl
    {
        public static DependencyProperty ValueProperty;
        private static void OnValueChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_08)o).Byte0.Value = (int)e.NewValue;
        }
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static DependencyProperty TextTypeProperty;
        private static void OnTextTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_08)o).textType.Content = (string)e.NewValue;
        }
        public string TextType
        {
            get { return (string)GetValue(TextTypeProperty); }
            set { SetValue(TextTypeProperty, value); }
        }

        public static DependencyProperty TextProperty;
        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_08)o).text.Content = (string)e.NewValue;
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        static S7300_08()
        {
            ValueProperty = DependencyProperty.Register("Value",
                typeof(int), typeof(S7300_08),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueChanged)));

            TextTypeProperty = DependencyProperty.Register("TextType",
                typeof(string), typeof(S7300_08),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextTypeChanged)));
            TextProperty = DependencyProperty.Register("Text",
                typeof(string), typeof(S7300_08),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextChanged)));
        }


        public S7300_08()
        {
            InitializeComponent();
        }
    }
}
