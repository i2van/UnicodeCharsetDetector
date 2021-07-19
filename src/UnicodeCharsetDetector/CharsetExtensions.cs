using System;
using System.Text;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace UnicodeCharsetDetector
{
    public static class CharsetExtensions
    {
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
                _ => throw new ArgumentOutOfRangeException(nameof(charset), charset, null)
            };

        public static int BomLength(this Charset charset) =>
            charset switch
            {
                Charset.Utf32BeBom or Charset.Utf32LeBom => 4,
                Charset.Utf16BeBom or Charset.Utf16LeBom => 2,
                Charset.Utf7Bom or Charset.Utf8Bom => 3,
                _ => 0
            };
    }
}