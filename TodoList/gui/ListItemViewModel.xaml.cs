using System;
using Brush = System.Windows.Media.Brush;

namespace TodoList.gui {
    public class ListItemViewModel {
        public string ListName { get; set; }
        public string ShortDescription { get; set; }
        public int Progress { get; set; } = 0;
        public Brush[] ProgressBrushes { get; } = {
            System.Windows.Media.Brushes.Gray,
            System.Windows.Media.Brushes.CornflowerBlue,
            System.Windows.Media.Brushes.LightGreen
        };

        public string? Markdown { get; set; }
        public Guid? MdId { get; set; }

        public ListItemViewModel(string listName, string shortDescription) {
            ListName = listName;
            ShortDescription = shortDescription;
        }
    }
}