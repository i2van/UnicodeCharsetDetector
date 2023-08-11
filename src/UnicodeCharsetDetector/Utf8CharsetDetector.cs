using System.IO;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace UnicodeCharsetDetector
{
    /// <summary>
    /// Represents UTF-8 charsets detector.
    /// </summary>
    public class Utf8CharsetDetector : CharsetDetector
    {
        /// <summary>
        /// Indicates that the stream is considered binary if binary zero is present in the stream.
        /// </summary>
        /// <returns><see langword="true" /> if binary zero is present in the stream then the stream is considered binary; otherwise, <see langword="false" />.</returns>
        public bool NullSuggestsBinary { get; set; } = true;

        /// <inheritdoc />
        public override Charset Check(Stream stream)
        {
            // UTF8 Valid sequences
            // 0xxxxxxx  ASCII
            // 110xxxxx 10xxxxxx  2-byte
            // 1110xxxx 10xxxxxx 10xxxxxx  3-byte
            // 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx  4-byte
            //
            // Width in UTF8
            // Decimal      Width
            // 0-127        1 byte
            // 194-223      2 bytes
            // 224-239      3 bytes
            // 240-244      4 bytes
            //
            // Subsequent chars are in the range 128-191
            var onlySawAsciiRange = true;

            int ch;
            while ((ch = stream.ReadByte()) >= 0)
            {
                if (ch == 0 && NullSuggestsBinary)
                {
                    return Charset.None;
                }

                int moreChars;
                switch (ch)
                {
                    case <= 127:
                        moreChars = 0;
                        break;
                    case >= 194 and <= 223:
                        moreChars = 1;
                        break;
                    case <= 239:
                        moreChars = 2;
                        break;
                    case <= 244:
                        moreChars = 3;
                        break;
                    default:
                        return Charset.None;
                }

                // Check secondary chars are in range if we are expecting any
                while (moreChars > 0 && (ch = stream.ReadByte()) >= 0)
                {
                    // Seen non-ascii chars now
                    onlySawAsciiRange = false;
                    if (ch is < 128 or > 191)
                    {
                        return Charset.None;
                    }
                    --moreChars;
                }
            }

            return onlySawAsciiRange
                    ? Charset.Ascii
                    : Charset.Utf8;
        }
    }
}