/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using SixLabors.ImageSharp;
using Verdict.Models;

namespace Verdict.Services;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(TextItem[]))]
[JsonSerializable(typeof(List<ImageItem>))]
[JsonSerializable(typeof(List<ItemModel>))]
[JsonSerializable(typeof(bool?[]))]
internal partial class SourceGenerationContext : JsonSerializerContext { }

public static class JsonService
{
    private static bool FilterExceptions(Exception ex)
    {
        Console.WriteLine(ex.Message);
        return ex is FileNotFoundException or JsonException;
    }

    private static readonly FilePickerFileType JsonFileType = new("JSON")
    {
        Patterns = ["*.json"],
        AppleUniformTypeIdentifiers = ["public.json"],
        MimeTypes = ["application/json"],
    };

    public static async Task<List<ItemModel>> Read(IStorageProvider provider)
    {
        var files = await provider.OpenFilePickerAsync(
            new FilePickerOpenOptions()
            {
                Title = "Open JSON File",
                AllowMultiple = false,
                FileTypeFilter = [JsonFileType],
            }
        );

        if (files.Count < 1)
            return [];
        var input = files[0];
        try
        {
            await using var stream = await input.OpenReadAsync();
            var items =
                await JsonSerializer.DeserializeAsync<List<ItemModel>>(
                    stream,
                    SourceGenerationContext.Default.ListItemModel
                ) ?? [];
            return items;
        }
        catch (Exception ex) when (FilterExceptions(ex))
        {
            return [];
        }
    }

    public static async Task Write(IStorageProvider provider, bool?[] items)
    {
        var file = await provider.SaveFilePickerAsync(
            new FilePickerSaveOptions() { Title = "Save JSON File", DefaultExtension = "json" }
        );
        if (file is null)
            return;

        await using var stream = await file.OpenWriteAsync();
        await JsonSerializer.SerializeAsync(
            stream,
            items,
            SourceGenerationContext.Default.NullableBooleanArray
        );
    }

    private static (bool isImage, int width) CheckImageFile(string path)
    {
        try
        {
            // Load the image
            using var image = Image.Load(path);
            return (true, image.Width);
        }
        catch (Exception)
        {
            // Handle exceptions related to loading the image
            return (false, 0);
        }
    }

    public static async Task AggregateImages(IStorageProvider provider)
    {
        var folders = await provider.OpenFolderPickerAsync(
            new FolderPickerOpenOptions() { Title = "Aggregate Images", AllowMultiple = false }
        );
        if (folders.Count < 1)
            return;
        var items = folders[0].GetItemsAsync();
        List<ImageItem> images = [];
        await foreach (var item in items)
        {
            var uri = item.Path;
            var link = uri.AbsolutePath;
            var (isImage, width) = CheckImageFile(link);
            if (uri.IsFile && isImage)
                images.Add(new ImageItem(link, width, item.Name));
        }

        var output = await provider.SaveFilePickerAsync(
            new FilePickerSaveOptions() { Title = "Save JSON File", DefaultExtension = "json" }
        );
        if (output is null)
            return;

        await using var stream = await output.OpenWriteAsync();
        await JsonSerializer.SerializeAsync(
            stream,
            images,
            SourceGenerationContext.Default.ListImageItem
        );
    }
}
