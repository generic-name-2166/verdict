/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System.Text.Json.Serialization;

namespace Verdict.Models;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$type")]
[JsonDerivedType(typeof(TextItem), typeDiscriminator: "text")]
[JsonDerivedType(typeof(ImageItem), typeDiscriminator: "image")]
public abstract class ItemModel { }

[JsonDerivedType(typeof(TextItem), typeDiscriminator: "text")]
public class TextItem(string text) : ItemModel
{
    public string Text { get; set; } = text;

    // JSON deserializer needs a constructor with no arguments and a setter on all the properties
    public TextItem()
        : this(string.Empty) { }
}

[JsonDerivedType(typeof(ImageItem), typeDiscriminator: "image")]
public class ImageItem(string link, int width, string name) : ItemModel
{
    /// <summary>
    /// Absolute path. Idk how well this works on other platforms
    /// </summary>
    public string Link { get; set; } = link;
    public int Width { get; set; } = width;

    public string Name { get; set; } = name;

    public ImageItem()
        : this(string.Empty, 0, string.Empty) { }
}
