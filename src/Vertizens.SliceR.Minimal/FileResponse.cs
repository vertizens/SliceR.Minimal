namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Wrapper around generic FileResponse for returning binary streamed data fron an API endpoint
/// </summary>
/// <param name="content">Binary data</param>
/// <param name="filename">filename of streamed file</param>
/// <param name="contentType">Content type to set</param>
public class FileResponse(Stream content, string? filename, string? contentType = null)
{
    /// <summary>
    /// Filename to return with api response
    /// </summary>
    public string? Filename { get; set; } = ReplaceInvalidFilenameCharacters(filename);

    /// <summary>
    /// Stream that contains content to return
    /// </summary>
    public Stream Content { get; init; } = content;

    /// <summary>
    /// Any content type to specify when returning
    /// </summary>
    public string? ContentType { get; set; } = contentType;

    /// <summary>
    /// Replaces filename invalid characters with a specified character
    /// </summary>
    /// <param name="filename">filename to strip invalid characters from</param>
    /// <param name="replaceChar">character to replace invalid characters with</param>
    /// <returns></returns>
    public static string? ReplaceInvalidFilenameCharacters(string? filename, char replaceChar = '_')
    {
        if (filename != null)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
            {
                filename = filename.Replace(invalidChar, replaceChar);
            }
        }

        return filename;
    }
}

