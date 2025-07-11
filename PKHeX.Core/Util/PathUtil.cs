using System;
using System.IO;

namespace PKHeX.Core;

/// <summary>
/// Logic for sanitizing file names and paths.
/// </summary>
/// <remarks>
/// Converting raw data to file names, trusting the data can lead to filesystem issues with invalid characters.
/// </remarks>
public static class PathUtil
{
    /// <summary>
    /// Cleans the <see cref="fileName"/> by removing any invalid filename characters.
    /// </summary>
    /// <returns>New string without any invalid characters.</returns>
    public static string CleanFileName(string fileName)
    {
        Span<char> result = stackalloc char[fileName.Length];
        int ctr = GetCleanFileName(fileName, result);
        if (ctr == fileName.Length)
            return fileName;
        return new string(result[..ctr]);
    }

    /// <inheritdoc cref="CleanFileName(string)"/>
    public static string CleanFileName(ReadOnlySpan<char> fileName)
    {
        Span<char> result = stackalloc char[fileName.Length];
        int ctr = GetCleanFileName(fileName, result);
        if (ctr == fileName.Length)
            return fileName.ToString();
        return new string(result[..ctr]);
    }

    /// <summary>
    /// Wish this were a ReadOnlySpan&lt;char&gt; instead of a char[].
    /// </summary>
    private static readonly char[] InvalidFileNameChars = Path.GetInvalidFileNameChars();

    /// <summary>
    /// Removes any invalid filename characters from the input string.
    /// </summary>
    /// <param name="input">String to clean</param>
    /// <param name="output">Buffer to write the cleaned string to</param>
    /// <returns>Length of the cleaned string</returns>
    private static int GetCleanFileName(ReadOnlySpan<char> input, Span<char> output)
    {
        ReadOnlySpan<char> invalid = InvalidFileNameChars;
        int ctr = 0;
        foreach (var c in input)
        {
            if (!invalid.Contains(c))
                output[ctr++] = c;
        }
        return ctr;
    }
}
