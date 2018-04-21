using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using PlutoLauncher.Windows.Content;

namespace PlutoLauncher.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = ListViewMenu.SelectedIndex;
            MoveCursorMenu(index);

            switch (index)
            {
                case 0: // Home
                    MainContent.Children.Clear();
                    MainContent.Children.Add(new Home());
                    break;
                case 1: // T6
                    MainContent.Children.Clear();
                    MainContent.Children.Add(new T6());
                    break;
                case 2: // IW5
                    MainContent.Children.Clear();
                    MainContent.Children.Add(new IW5());
                    break;
                case 3: // Social
                    MainContent.Children.Clear();
                    //MainContent.Children.Add(new Social());
                    break;
                case 4: // Settings
                    MainContent.Children.Clear();
                    MainContent.Children.Add(new Settings());
                    break;
                default:
                    break;
            }
        }

        private void MoveCursorMenu(int index)
        {
            TrainsitionigContentSlide.OnApplyTemplate();
            GridCursor.Margin = new Thickness(0, (60 * index), 0, 0);
        }
    }
}
