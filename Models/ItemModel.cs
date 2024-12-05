/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

namespace Verdict.Models;

public interface IItemModel { }

public class TextItem(string text) : IItemModel
{
    public string Text { get; set; } = text;

    // JSON deserializer needs a constructor with no arguments and a setter on all the properties
    public TextItem()
        : this(string.Empty) { }
}

public class ImageItem(string link) : IItemModel
{
    public string Link { get; set; } = link;

    public ImageItem()
        : this(string.Empty) { }
}
