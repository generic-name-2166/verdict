/*
Copyright 2024 generic-name-2166

SPDX-License-Identifier: AGPL-3.0-or-later
*/

namespace Verdict.Models;

public class TextItem(string text)
{
    public string Text { get; set; } = text;

    // JSON deserializer needs a constructor with no arguments and a setter on all the properties
    public TextItem()
        : this(string.Empty) { }
}
