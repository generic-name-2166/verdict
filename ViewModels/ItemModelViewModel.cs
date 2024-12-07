/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System.IO;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using Verdict.Models;

namespace Verdict.ViewModels;

public interface IItemModelViewModel { }

public partial class TextItemViewModel(TextItem textItem) : ViewModelBase, IItemModelViewModel
{
    [ObservableProperty]
    private string _text = textItem.Text;
}

public class ImageItemViewModel(ImageItem imageItem) : ViewModelBase, IItemModelViewModel
{
    public Bitmap ImageBitmap =>
        Bitmap.DecodeToWidth(File.OpenRead(imageItem.Link), imageItem.Width);
    public int Width => imageItem.Width;
    public string Name => imageItem.Name;
}
