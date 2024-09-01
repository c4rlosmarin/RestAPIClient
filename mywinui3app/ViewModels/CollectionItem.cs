﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace mywinui3app.ViewModels;

public partial class CollectionItem : ObservableRecipient
{
    [ObservableProperty]
    public string name;
    [ObservableProperty]
    public string description;
    [ObservableProperty]
    public DateTime? creationTime;
    [ObservableProperty]
    public DateTime? lastModifiedTime;
    [ObservableProperty]
    public ObservableCollection<GroupItem> groups;
}