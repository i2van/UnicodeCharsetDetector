using System;
using System.Collections.Generic;
using System.IO;

// ReSharper disable once UnusedMember.Global
// ReSharper disable once MemberCanBePrivate.Global

namespace UnicodeCharsetDetector
{
    /// <summary>
    /// Represents Unicode charsets detector which detects UTF-8 and UTF-16 charsets.
    /// </summary>
    public class UnicodeCharsetDetector : CharsetDetector
    {
        private readonly List<CharsetDetector> _detectors = new()
        {
            new Utf8CharsetDetector(),
            new Utf16CharsetDetector()
        };

        /// <inheritdoc />
        public override Charset Check(Stream stream)
        {
            var startPos = stream.Position;
            var charset = DoCheck(stream, startPos);
            stream.Seek(startPos, SeekOrigin.Begin);
            return charset;
        }

        private Charset DoCheck(Stream stream, long startPos)
        {
            var bomBuffer = new byte[4];
            var size = stream.Read(bomBuffer, 0, 4);
            var charset = CheckBom(bomBuffer, size);
            if (charset != Charset.None)
            {
                return charset;
            }

            foreach (var detector in _detectors)
            {
                stream.Seek(startPos, SeekOrigin.Begin);
                charset = detector.Check(stream);
                if (charset != Charset.None)
                {
                    return charset;
                }
            }

            stream.Seek(startPos, SeekOrigin.Begin);

            return DetectBinary(stream)
                    ? Charset.None
                    : Charset.Ansi;
        }

        private static bool DetectBinary(Stream stream)
        {
            int ch;
            while ((ch = stream.ReadByte()) >= 0)
            {
                if (ch == 0) return true;
            }
            return false;
        }

        internal static Charset CheckBom(byte[] buffer, int size)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (buffer.Length < size)
                throw new ArgumentException("The size is greater than the length of buffer.", nameof(size));

            return size switch
            {
                > 3 when buffer[0] == 0xFF && buffer[1] == 0xFE && buffer[2] == 0 && buffer[3] == 0 => Charset.Utf32LeBom,
                > 3 when buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xFE && buffer[3] == 0xFF => Charset.Utf32BeBom,
                > 3 when buffer[0] == 0x2B && buffer[1] == 0x2F && buffer[2] == 0x76 &&
                         (buffer[3] == 0x38 || buffer[3] == 0x39 || buffer[3] == 0x2B || buffer[3] == 0x2F) => Charset.Utf7Bom,
                > 2 when buffer[0] == 0xEF && buffer[1] == 0xBB && buffer[2] == 0xBF => Charset.Utf8Bom,
                > 1 when buffer[0] == 0xFF && buffer[1] == 0xFE => Charset.Utf16LeBom,
                > 1 when buffer[0] == 0xFE && buffer[1] == 0xFF => Charset.Utf16BeBom,
                _ => Charset.None
            };
        }
    }
}