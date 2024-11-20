using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Page = TodoList.gui.Page;

namespace TodoList {
    public partial class MainWindow : Window {
        public Page CurrentPage { get; set; }
        public PageSaver PageSaver { get; }

        public MainWindow() {
            PageSaver = new PageSaver(this);
            CurrentPage = PageSaver.LoadFromFile("save");
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
            PageSaver.SavePageToFile(CurrentPage);
            Close();
        }

        private void CloseItem(object sender, RoutedEventArgs e) {
            if (Editor.Visibility == Visibility.Visible) {
                EditItem(sender, e);
            }
            CurrentPage.CurrentItem = null;
        }

        private void EditItem(object sender, RoutedEventArgs e) {
            var item = CurrentPage.CurrentItem;
            if (item == null) return;
            var visible = Editor.Visibility == Visibility.Visible;
            Editor.Visibility = visible ? Visibility.Hidden : Visibility.Visible;
            if (visible) {
                PageSaver.UpdateMd(item, Editor.Text);
            }
            else {
                Editor.Text = item.ViewModel.Markdown ?? "";
            }
            MarkdownEditor.Visibility = !visible ? Visibility.Hidden : Visibility.Visible;
            MarkdownEditor.Markdown = item.ViewModel.Markdown ?? "";
        }

        private new void MouseMove(object sender, MouseEventArgs e) {
            if (!CurrentPage.IsDragging) return;
            CurrentPage.MouseMove(
                CurrentPage.GetClosestGroupIndex(e.GetPosition(this)),
                CurrentPage.DraggingItemIndex[0] == -1
                    ? -1
                    : CurrentPage.Groupings[CurrentPage.DraggingItemIndex[0]].GetClosestItemIndex(e.GetPosition(this)));
        }

        private void MouseButtonUp(object sender, MouseEventArgs e) {
            if (Keyboard.FocusedElement is TextBox focused) {
                focused.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }

            CurrentPage.IsDragging = false;
            CurrentPage.DraggingIndex = -1;
            CurrentPage.DraggingItemIndex[0] = -1;
        }

        private void ToggleWindowState() {
            WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
        }
    }
}