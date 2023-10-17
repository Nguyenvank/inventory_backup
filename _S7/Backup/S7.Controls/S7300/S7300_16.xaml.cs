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
    public partial class S7300_16 : UserControl
    {
        public static DependencyProperty ValueByte0Property;
        private static void OnValueByte0Changed(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_16)o).Byte0.Value = (int)e.NewValue;
        }
        public int ValueByte0
        {
            get { return (int)GetValue(ValueByte0Property); }
            set { SetValue(ValueByte0Property, value); }
        }

        public static DependencyProperty ValueByte1Property;
        private static void OnValueByte1Changed(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_16)o).Byte1.Value = (int)e.NewValue;
        }
        public int ValueByte1
        {
            get { return (int)GetValue(ValueByte1Property); }
            set { SetValue(ValueByte1Property, value); }
        }

        public static DependencyProperty TextLeftProperty;
        private static void OnTextLeftChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_16)o).textLeft.Content = (string)e.NewValue;
        }
        public string TextLeft
        {
            get { return (string)GetValue(TextLeftProperty); }
            set { SetValue(TextLeftProperty, value); }
        }

        public static DependencyProperty TextTypeProperty;
        private static void OnTextTypeChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_16)o).textType.Content = (string)e.NewValue;
        }
        public string TextType
        {
            get { return (string)GetValue(TextTypeProperty); }
            set { SetValue(TextTypeProperty, value); }
        }

        public static DependencyProperty TextProperty;
        private static void OnTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            ((S7300_16)o).text.Content = (string)e.NewValue;
        }
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        static S7300_16()
        {
            ValueByte0Property = DependencyProperty.Register("ValueByte0",
                typeof(int), typeof(S7300_16),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueByte0Changed)));
            ValueByte1Property = DependencyProperty.Register("ValueByte1",
                typeof(int), typeof(S7300_16),
                new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnValueByte1Changed)));

            TextLeftProperty = DependencyProperty.Register("TextLeft",
                typeof(string), typeof(S7300_16),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextLeftChanged)));
            TextTypeProperty = DependencyProperty.Register("TextType",
                typeof(string), typeof(S7300_16),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextTypeChanged)));
            TextProperty = DependencyProperty.Register("Text",
                typeof(string), typeof(S7300_16),
                new FrameworkPropertyMetadata("", new PropertyChangedCallback(OnTextChanged)));
        }


        public S7300_16()
        {
            InitializeComponent();
        }
    }
}
