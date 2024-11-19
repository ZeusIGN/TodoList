using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using TodoList.gui;

namespace TodoList {
    public partial class MainWindow : Window {
        public Page CurrentPage { get; set; }

        public MainWindow() {
            var page = new Page();
            CurrentPage = page;
            DataContext = CurrentPage;
            InitializeComponent();
        }

        private void Header_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                ToggleWindowState();
            }
            else {
                DragMove();
            }
        }

        private void MinimizeClick(object sender, RoutedEventArgs e) {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeClick(object sender, RoutedEventArgs e) {
            ToggleWindowState();
        }

        private void CloseClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private new void MouseMove(object sender, MouseEventArgs e) {
            if (CurrentPage.Groupings.Count > 1) {
                var group = CurrentPage.Groupings[0];
                var index = group.GetClosestItemIndex(e.GetPosition(this));
                Debug.WriteLine(index);
            }

            if (!CurrentPage.IsDragging) return;
            CurrentPage.MouseMove(
                CurrentPage.GetClosestGroupIndex(e.GetPosition(this)),
                CurrentPage.DraggingItemIndex[0] == -1
                    ? -1
                    : CurrentPage.Groupings[CurrentPage.DraggingItemIndex[0]].GetClosestItemIndex(e.GetPosition(this)));
        }

        private void MouseButtonUp(object sender, MouseEventArgs e) {
            CurrentPage.IsDragging = false;
            CurrentPage.DraggingIndex = -1;
            CurrentPage.DraggingItemIndex[0] = -1;
        }

        private void ToggleWindowState() {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}