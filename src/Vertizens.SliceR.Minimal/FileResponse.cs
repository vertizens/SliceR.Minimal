namespace Vertizens.SliceR.Minimal;

/// <summary>
/// Wrapper around generic FileResponse for returning binary streamed data fron an API endpoint
/// </summary>
/// <param name="content">Binary data</param>
/// <param name="filename">filename of streamed file</param>
public class FileResponse(Stream content, string? filename, string? contentType = null)
{
    public string? Filename { get; set; } = ReplaceInvalidFilenameCharacters(filename);
    public Stream Content { get; set; } = content;
    public string? ContentType { get; set; } = contentType;

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

