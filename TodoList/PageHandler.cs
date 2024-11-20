using System;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using TodoList.gui;
using Page = TodoList.gui.Page;

namespace TodoList;

public class PageHandler {
    private MainWindow MainWindow { get; }

    public static string directory = @"..\..\..\..\Data\";

    public PageHandler(MainWindow window) {
        MainWindow = window;
    }

    public void SwitchPage(int index) {
        Page? newPage = null;
        if (index == -1) {
            newPage = new Page(MainWindow);
        }

        newPage ??= MainWindow.Pages[index];
        MainWindow.CurrentPage = newPage;
        UpdatePage();
    }

    public void DeletePage(Guid id) {
        int index = MainWindow.Pages.ToList().FindIndex(page => page.Id.Equals(id));
        if (index == -1) return;
        Page page = MainWindow.Pages[index];
        RemovePage(page.Id);
        MainWindow.Pages = MainWindow.Pages.Where(page => !page.Id.Equals(id)).ToArray();
        if (MainWindow.Pages.Length == 0) {
            SwitchPage(-1);
        }
        else {
            MainWindow.CurrentPage = MainWindow.Pages[0];
        }

        UpdatePage();
    }

    public void SwitchPage(Guid id) {
        MainWindow.CurrentPage = MainWindow.Pages.First(page => page.Id.Equals(id));
        if (MainWindow.CurrentPageButton == null) return;
        UpdatePage();
    }

    public void UpdatePage() {
        if (MainWindow.CurrentPage == null || MainWindow.CurrentPageButton == null) return;
        MainWindow.CurrentPageButton.Content = $"{MainWindow.CurrentPage.Name}";
        MainWindow.PageName.Text = MainWindow.CurrentPage.Name;
        MainWindow.PageControl.ItemsSource = MainWindow.CurrentPage.Groupings;
        MainWindow.PagesList.ItemsSource = MainWindow.Pages;
        MainWindow.PagesList.Items.Refresh();
    }

    private void SavePageToFile(Page page) {
        var groupings = page.Groupings.ToArray();

        var groupArray = new JsonArray();
        foreach (var grouping in groupings) {
            if (grouping.Equals(page.Groupings[^1])) continue;
            var items = grouping.Items.Children;
            var itemArray = new JsonArray();
            foreach (ListItem item in items) {
                var itemObject = new JsonObject {
                    ["Name"] = item.ViewModel.ListName,
                    ["ShortDescription"] = item.ViewModel.ShortDescription,
                    ["ID"] = item.ViewModel.MdId,
                    ["Progress"] = item.ViewModel.Progress,
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

        var pageObject = new JsonObject {
            ["Name"] = page.Name,
            ["ID"] = page.Id,
            ["Groupings"] = groupArray
        };
        Directory.CreateDirectory(directory);
        File.WriteAllText($@"{directory}\{page.Id}.json", pageObject.ToString());
    }

    private Page LoadFromFile(string name) {
        var json = File.ReadAllText($@"{directory}\{name}.json");
        var pageObject = JsonNode.Parse(json).AsObject();
        var page = new Page(MainWindow, Guid.Parse(pageObject["ID"].ToString()), pageObject["Name"].ToString());
        foreach (var grouping in pageObject["Groupings"].AsArray()) {
            var group = page.CreateGroup(grouping["Name"].ToString());
            foreach (var item in grouping["Items"].AsArray()) {
                group.CreateItem(
                    item["Name"].ToString(),
                    item["ShortDescription"].ToString(),
                    Guid.Parse(item["ID"].ToString()),
                    item["Progress"].GetValue<int>());
            }
        }

        return page;
    }

    public Page[] LoadPages() {
        Directory.CreateDirectory(directory);
        string[] files = Directory.GetFiles($@"{directory}\");
        return files.Select(file => LoadFromFile(Path.GetFileNameWithoutExtension(file))).ToArray();
    }

    public void SavePages() {
        foreach (var page in MainWindow.Pages) {
            SavePageToFile(page);
        }
    }

    public Guid CreateListMd() {
        var guid = Guid.NewGuid();
        Directory.CreateDirectory($@"{directory}\ItemMds");
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

    public void RemovePage(Guid id) {
        var file = $@"{directory}\{id}.json";
        if (!File.Exists(file)) return;
        File.Delete($@"{directory}\{id}.json");
    }
}