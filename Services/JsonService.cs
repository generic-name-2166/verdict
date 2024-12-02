/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Verdict.Models;

namespace Verdict.Services;

[JsonSourceGenerationOptions(WriteIndented = false)]
[JsonSerializable(typeof(TextItem[]))]
[JsonSerializable(typeof(bool?[]))]
internal partial class SourceGenerationContext : JsonSerializerContext { }

public static class JsonService
{
    private static readonly string Input = Path.Combine(Environment.CurrentDirectory, "data.json");

    private static readonly string Output = Path.Combine(
        Environment.CurrentDirectory,
        "verdict.json"
    );

    private static bool FilterExceptions(Exception ex)
    {
        return ex is FileNotFoundException or JsonException;
    }

    public static async Task<TextItem[]> Read()
    {
        try
        {
            await using var stream = new FileStream(Input, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<TextItem[]>(
                    stream,
                    SourceGenerationContext.Default.TextItemArray
                ) ?? [];
        }
        catch (Exception ex) when (FilterExceptions(ex))
        {
            return [];
        }
    }

    public static async Task Write(bool?[] items)
    {
        await using var stream = new FileStream(Output, FileMode.Create);
        await JsonSerializer.SerializeAsync(
            stream,
            items,
            SourceGenerationContext.Default.NullableBooleanArray
        );
    }
}
