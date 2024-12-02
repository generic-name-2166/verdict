/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;
using Verdict.Models;

namespace Verdict.Services;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(TextItem[]))]
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

    public static async Task<TextItem[]> Read(IStorageProvider provider)
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
                await JsonSerializer.DeserializeAsync<TextItem[]>(
                    stream,
                    SourceGenerationContext.Default.TextItemArray
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
            new FilePickerSaveOptions() { Title = "Save JSON File", DefaultExtension = ".json" }
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
}
