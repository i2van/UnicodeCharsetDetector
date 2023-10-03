using System;
using System.Text;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace UnicodeCharsetDetector;

/// <summary>
/// Provides a set of <see langword="static" /> (<see langword="Shared" /> in Visual Basic) extension methods for charset.
/// </summary>
public static class CharsetExtensions
{
    /// <summary>
    /// Converts charset to encoding.
    /// </summary>
    /// <param name="charset">The charset to convert.</param>
    /// <param name="addByteOrderMask"><see langword="true" /> to specify that the <see cref="M:System.Text.UnicodeEncoding.GetPreamble" /> method returns a Unicode byte order mark; otherwise, <see langword="false" />.</param>
    /// <param name="throwOnInvalidBytes"> <see langword="true" /> to specify that an exception should be thrown when an invalid encoding is detected; otherwise, <see langword="false" />.</param>
    /// <returns>The <see cref="Encoding"/> which corresponds to <paramref name="charset"/> specified.</returns>
    /// <exception cref="ArgumentOutOfRangeException">If the <paramref name="charset"/> is out of range.</exception>
    public static Encoding ToEncoding(this Charset charset, bool addByteOrderMask = true, bool throwOnInvalidBytes = true) =>
        charset switch
        {
            Charset.None or Charset.Ascii or Charset.Ansi or Charset.Bom => Encoding.ASCII,
            Charset.Utf7 or Charset.Utf7Bom => Encoding.UTF7,
            Charset.Utf8 or Charset.Utf8Bom => new UTF8Encoding(addByteOrderMask, throwOnInvalidBytes),
            Charset.Utf16Le or Charset.Utf16LeBom => new UnicodeEncoding(false, addByteOrderMask, throwOnInvalidBytes),
            Charset.Utf16Be or Charset.Utf16BeBom => new UnicodeEncoding(true, addByteOrderMask, throwOnInvalidBytes),
            Charset.Utf32Le or Charset.Utf32LeBom => new UTF32Encoding(false, addByteOrderMask, throwOnInvalidBytes),
            Charset.Utf32Be or Charset.Utf32BeBom => new UTF32Encoding(true, addByteOrderMask, throwOnInvalidBytes),
            _ => throw new ArgumentOutOfRangeException(nameof(charset), charset, $"Unknown charset {charset}")
        };

    /// <summary>
    /// Returns the length of the charset BOM.
    /// </summary>
    /// <param name="charset">The charset which BOM length to return.</param>
    /// <returns>The length of the <paramref name="charset"/> BOM.</returns>
    public static int BomLength(this Charset charset) =>
        charset switch
        {
            Charset.Utf32BeBom or Charset.Utf32LeBom => 4,
            Charset.Utf16BeBom or Charset.Utf16LeBom => 2,
            Charset.Utf7Bom or Charset.Utf8Bom => 3,
            _ => 0
        };
}
