using System;

namespace UnicodeCharsetDetector;

/// <summary>
/// Specifies the charset detected.
/// </summary>
[Flags]
public enum Charset
{
    /// <summary>
    /// The charset is unknown or binary.
    /// </summary>
    None = 0,

    /// <summary>
    /// The charset is ASCII charset with 0-127 characters range.
    /// </summary>
    Ascii = 1,

    /// <summary>
    /// The charset is ANSI charset with 0-255 characters range.
    /// </summary>
    Ansi = 3,

    /// <summary>
    /// The charset is UTF-7.
    /// </summary>
    Utf7 = 1 << 2,

    /// <summary>
    /// The charset is UTF-7 BOM.
    /// </summary>
    Utf7Bom = Utf7 | Bom,

    /// <summary>
    /// The charset is UTF-8.
    /// </summary>
    Utf8 = 1 << 3,

    /// <summary>
    /// The charset is UTF-8 BOM.
    /// </summary>
    Utf8Bom = Utf8 | Bom,

    /// <summary>
    /// The charset is UTF-16 LE.
    /// </summary>
    Utf16Le = 1 << 4,

    /// <summary>
    /// The charset is UTF-16 LE BOM.
    /// </summary>
    Utf16LeBom = Utf16Le | Bom,

    /// <summary>
    /// The charset is UTF-16 BE.
    /// </summary>
    Utf16Be = 1 << 5,

    /// <summary>
    /// The charset is UTF-16 BOM.
    /// </summary>
    Utf16BeBom = Utf16Be | Bom,

    /// <summary>
    /// The charset is UTF-32 LE.
    /// </summary>
    Utf32Le = 1 << 6,

    /// <summary>
    /// The charset is UTF-32 LE BOM.
    /// </summary>
    Utf32LeBom = Utf32Le | Bom,

    /// <summary>
    /// The charset is UTF-32 BE.
    /// </summary>
    Utf32Be = 1 << 7,

    /// <summary>
    /// The charset is UTF-32 BE BOM.
    /// </summary>
    Utf32BeBom = Utf32Be | Bom,

    /// <summary>
    /// The BOM flag is present.
    /// </summary>
    Bom = 1 << 16
}
