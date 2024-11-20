using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoList.gui {
    public partial class ListItem : UserControl {
        public ListItemViewModel ViewModel { get; }
        public ListGrouping Group { get; set; }
        private Page Page { get; }

        public ListItem(ListGrouping grouping, string listName, string shortDescription) {
            Group = grouping;
            Page = grouping.Page;
            ViewModel = new ListItemViewModel(listName, shortDescription);
            DataContext = ViewModel;
            InitializeComponent();
        }

        public void Reload() {
            ShortDesc.Text = ViewModel.ShortDescription;
            ListName.Text = ViewModel.ListName;
        }

        private void Remove() {
            Group.RemoveItem(this);
        }

        private void HandleMouseButton(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 2) {
                Page.CurrentItem = this;
                return;
            }

            Page.DraggingItemIndex[0] = Page.Groupings.IndexOf(Group);
            Page.DraggingItemIndex[1] = Group.Items.Children.IndexOf(this);
            Page.IsDragging = true;
        }

        private void ButtonRemove(object sender, RoutedEventArgs e) {
            Remove();
        }
    }
}