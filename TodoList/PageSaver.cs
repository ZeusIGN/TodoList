using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using TodoList.gui;
using Page = TodoList.gui.Page;

namespace TodoList;

public class PageSaver {
    public MainWindow MainWindow { get; }
    
    public static string directory = @"..\..\..\..\Data\";
    
    public PageSaver(MainWindow window) {
        MainWindow = window;
    }


    public void SavePageToFile(Page page) {
        var groupings = page.Groupings.ToArray();
        JsonArray groupArray = new JsonArray();
        foreach (var grouping in groupings) {
            if (grouping.Equals(page.Groupings[^1])) continue;
            var items = grouping.Items.Children;
            JsonArray itemArray = new JsonArray();
            foreach (ListItem item in items) {
                var itemObject = new JsonObject {
                    ["Name"] = item.ViewModel.ListName,
                    ["ShortDescription"] = item.ViewModel.ShortDescription,
                    ["ID"] = item.ViewModel.MdId,
                    ["Index"] = grouping.Items.Children.IndexOf(item)
                };
                itemArray.Add(itemObject);
            }

            var groupingObject = new JsonObject {
                ["Name"] = grouping.TextBox.Text,
                ["Index"] = page.Groupings.IndexOf(grouping),
                ["Items"] = itemArray
            };
            groupArray.Add(groupingObject);
        }

        File.WriteAllText($@"{directory}\save.json", groupArray.ToString());
    }
    
    public Page LoadFromFile(string name) {
        Page page = new Page(MainWindow);
        var json = File.ReadAllText($@"{directory}\{name}.json");
        var groupArray = JsonNode.Parse(json).AsArray();
        foreach (var grouping in groupArray) {
            var group = page.CreateGroup(grouping["Name"].ToString());
            foreach (var item in grouping["Items"].AsArray()) {
                group.CreateItem(item["Name"].ToString(), item["ShortDescription"].ToString(), Guid.Parse(item["ID"].ToString()));
            }
        }

        return page;
    }
    
    public Guid CreateListMd() {
        var guid = Guid.NewGuid();
        File.Create($@"{directory}\ItemMds\{guid}.md").Close();
        return guid;
    }

    public void UpdateMd(ListItem item, string markdown) {
        Guid? id = item.ViewModel.MdId;
        if (id == null) return;
        item.ViewModel.Markdown = markdown;
        File.WriteAllText($@"{directory}\ItemMds\{id}.md", markdown);
    }
    
    public void RemoveListMd(Guid? id) {
        if (id == null) return;
        var file = $@"{directory}\ItemMds\{id}.md";
        if (!File.Exists(file)) return;
        File.Delete($@"{directory}\ItemMds\{id}.md");
    }
    
}