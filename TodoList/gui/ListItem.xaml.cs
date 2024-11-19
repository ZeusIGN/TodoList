using System.Windows.Controls;
using System.Windows.Input;

namespace TodoList.gui {
    public partial class ListItem : UserControl {
        public ListItemViewModel ViewModel { get; }
        public ListGrouping Group { get; set; }
        public Page Page { get; }

        public ListItem(ListGrouping grouping, string listName, string shortDescription) {
            Group = grouping;
            Page = grouping.Page;
            ViewModel = new ListItemViewModel(listName, shortDescription);
            DataContext = ViewModel;
            InitializeComponent();
        }

        public void HandleMouseButton(object sender, MouseButtonEventArgs e) {
            if (e.ClickCount == 1) {
                Page.DraggingItemIndex[0] = Page.Groupings.IndexOf(Group);
                Page.DraggingItemIndex[1] = Group.Items.Children.IndexOf(this);
                Page.IsDragging = true;
            }
        }
    }
}