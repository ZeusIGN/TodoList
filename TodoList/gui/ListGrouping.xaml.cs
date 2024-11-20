using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoList.gui {
    public partial class ListGrouping : UserControl {
        public Page Page { get; }
        public ListGroupingViewModel ViewModel { get; }

        public ListGrouping(Page page, string groupName) {
            Page = page;
            ViewModel = new ListGroupingViewModel(groupName);
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void AddItem(ListItem item) {
            if (item.ViewModel.MdId == null) {
                var id = Page.MainWindow.PageHandler.CreateListMd();
                item.ViewModel.MdId = id;
            }
            
            string text = File.ReadAllText($@"{PageHandler.directory}\ItemMds\{item.ViewModel.MdId}.md");
            item.ViewModel.Markdown = text;
            item.Reload();
            Items.Children.Add(item);
            item.Margin = new Thickness(0, 5, 0, 5);
        }

        public void RemoveItem(ListItem item) {
            var id = item.ViewModel.MdId;
            Page.MainWindow.PageHandler.RemoveListMd(id);
            Items.Children.Remove(item);
        }

        public void CreateItem(string name = "New Item", string shortDescription = "Short Description",
            Guid? id = null, int progress = 0) {
            AddItem(new ListItem(this, name, shortDescription) {
                ViewModel = { MdId = id, Progress = progress }
            });
        }

        public int GetClosestItemIndex(Point point) {
            var returnIndex = -1;
            var closestDistance = double.MaxValue;
            for (var i = 0; i < Items.Children.Count; i++) {
                var item = (ListItem)Items.Children[i];
                if (PresentationSource.FromVisual(item) == null) continue;
                var itemPoint = item.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
                var distance =
                    Math.Sqrt(Math.Pow(point.X - (itemPoint.X + item.Width / 2) - Margin.Left, 2) +
                              Math.Pow(point.Y - (itemPoint.Y + item.Height / 2), 2));
                if (distance < 0) continue;
                if (distance > closestDistance) continue;
                closestDistance = distance;
                returnIndex = i;
            }

            return returnIndex;
        }

        private void HandleButton(object sender, RoutedEventArgs e) {
            if (Page.Groupings[^1].Equals(this)) {
                Page.CreateGroup();
                return;
            }

            CreateItem();
        }

        private void MouseButtonLeft(object sender, MouseButtonEventArgs e) {
            Page.DraggingIndex = Page.Groupings.IndexOf(this);
            Page.IsDragging = true;
        }

        private void MouseButtonRight(object sender, MouseButtonEventArgs e) {
            if (!Page.Groupings[^1].Equals(this) && e.ClickCount == 2) {
                Page.Groupings.Remove(this);
            }
        }
    }
}