using System;

namespace TodoList.gui {
    public class ListItemViewModel {
        public string ListName { get; set; }
        public string ShortDescription { get; set; }
        public string? Markdown { get; set; }
        public Guid? MdId { get; set; }

        public ListItemViewModel(string listName, string shortDescription) {
            ListName = listName;
            ShortDescription = shortDescription;
        }
    }
}