using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace TodoList.gui;

public class Page : INotifyPropertyChanged {
    private ObservableCollection<ListGrouping> _groupings = new();
    public ObservableCollection<ListGrouping> Groupings => _groupings;
    public MainWindow MainWindow { get; }

    public bool IsDragging;
    public int DraggingIndex = -1;
    public int[] DraggingItemIndex = { -1, -1 };

    private ListItem? _currentItem;

    public ListItem? CurrentItem {
        get => _currentItem;
        set {
            if (_currentItem != null) 
                UpdateList(_currentItem);
            _currentItem = value;
            MainWindow.PropertySelector.Visibility = value == null ? Visibility.Hidden : Visibility.Visible;
            UpdateCard();
        }
    }

    public Page(MainWindow mainWindow) {
        MainWindow = mainWindow;
        var addNew = new ListGrouping(this, "Add New Group") {
            Margin = new Thickness(10, 10, 0, 0),
            TextBox = { Focusable = false }
        };
        _groupings.Add(addNew);
    }

    private void MoveGroup(int index, int to) {
        if (to.Equals(_groupings.Count - 1) || index.Equals(_groupings.Count - 1) || index == to || index == -1 ||
            to == -1 || _groupings.Count <= 0) return;
        _groupings.Move(index, to);
    }

    public ListGrouping CreateGroup(string groupName = "New Group") {
        var listGrouping = new ListGrouping(this, groupName) {
            Margin = new Thickness(10, 10, 0, 0)
        };
        _groupings.Insert(_groupings.Count - 1, listGrouping);
        return listGrouping;
    }

    public void AddListItem(ListGrouping listGrouping, string name = "New Item",
        string shortDescription = "Short Description") {
        listGrouping.CreateItem(name, shortDescription);
    }

    public void MouseMove(int closestGroupIndex = -1, int closestItemIndex = -1) {
        if (DraggingItemIndex[0] != -1) {
            HandleItemDragging(closestGroupIndex, closestItemIndex);
            return;
        }

        HandleGroupDragging(closestGroupIndex);
    }

    private void HandleItemDragging(int closestGroupIndex, int closestItemIndex) {
        if (DraggingItemIndex[0] == -1 || DraggingItemIndex[1] == -1 ||
            closestGroupIndex == _groupings.Count - 1) return;
        var group = _groupings[DraggingItemIndex[0]];
        var item = (ListItem)group.Items.Children[DraggingItemIndex[1]];
        item.Group = group;
        group.Items.Children.RemoveAt(DraggingItemIndex[1]);
        if (_groupings.Count < closestGroupIndex) return;
        _groupings[closestGroupIndex].Items.Children.Insert(closestItemIndex, item);
        DraggingItemIndex[0] = closestGroupIndex;
        DraggingItemIndex[1] = closestItemIndex;
    }

    private void HandleGroupDragging(int closestIndex) {
        if (DraggingIndex == -1 || closestIndex == -1 || closestIndex == DraggingIndex) return;
        MoveGroup(DraggingIndex, closestIndex);
        DraggingIndex = closestIndex;
    }

    public int GetClosestGroupIndex(Point point) {
        var returnIndex = -1;
        var closestDistance = double.MaxValue;
        for (var i = 0; i < _groupings.Count; i++) {
            var grouping = _groupings[i];
            if (grouping.Equals(_groupings[^1]) || PresentationSource.FromVisual(grouping) == null) continue;
            var groupingPoint = grouping.TransformToAncestor(Application.Current.MainWindow).Transform(new Point(0, 0));
            var distance =
                Math.Sqrt(Math.Pow(point.X - (groupingPoint.X + grouping.Width / 2) - grouping.Margin.Left, 2) +
                          Math.Pow(point.Y - groupingPoint.Y, 2));
            if (distance < 0) continue;
            if (distance > closestDistance) continue;
            closestDistance = distance;
            returnIndex = i;
        }

        return returnIndex;
    }

    private void UpdateCard() {
        MainWindow.PropertySelector.DataContext = CurrentItem?.ViewModel;
        var listName = MainWindow.ListName;
        var shortDesc = MainWindow.ShortDescription;
        var markdown = MainWindow.MarkdownEditor;
        listName.Text = CurrentItem?.ViewModel.ListName;
        shortDesc.Text = CurrentItem?.ViewModel.ShortDescription;
        markdown.Markdown = CurrentItem?.ViewModel.Markdown;
    }

    private void UpdateList(ListItem item) {
        var listName = MainWindow.ListName;
        var shortDesc = MainWindow.ShortDescription;
        item.ViewModel.ListName = listName.Text;
        item.ViewModel.ShortDescription = shortDesc.Text;
        item.Reload();
    }

    public void SavePageToFile() {
        
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}