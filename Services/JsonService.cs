using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Verdict.Models;

namespace Verdict.Services;

public static class JsonService
{
    private static readonly string Input = Path.Combine(Environment.CurrentDirectory, "data.json");

    private static readonly string Output = Path.Combine(
        Environment.CurrentDirectory,
        "verdict.json"
    );

    private static bool FilterExceptions(Exception ex)
    {
        return ex is FileNotFoundException;
    }

    public static async Task<TextItem[]> Read()
    {
        try
        {
            await using var stream = new FileStream(Input, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<TextItem[]>(stream) ?? [];
        }
        catch (Exception ex) when (FilterExceptions(ex))
        {
            return [];
        }
    }

    public static async Task Write(bool?[] items)
    {
        await using var stream = new FileStream(Output, FileMode.Create);
        await JsonSerializer.SerializeAsync(stream, items);
    }
}
