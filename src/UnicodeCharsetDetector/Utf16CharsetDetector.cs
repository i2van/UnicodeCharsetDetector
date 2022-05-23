using System.IO;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace UnicodeCharsetDetector
{
    /// <summary>
    /// Represents UTF-16 charsets detector.
    /// </summary>
    public class Utf16CharsetDetector : CharsetDetector
    {
        /// <summary>
        /// Gets or sets the expected binary zeroes percentage in the stream used for UTF-16 charsets detection.
        /// </summary>
        /// <returns>The expected binary zeroes percentage.</returns>
        public double ExpectedNullPercent { get; set; } = 70;

        /// <summary>
        /// Gets or sets the unexpected binary zeroes percentage in the stream used for UTF-16 charsets detection.
        /// </summary>
        /// <returns>The unexpected binary zeroes percentage.</returns>
        public double UnexpectedNullPercent { get; set; } = 10;

        /// <inheritdoc />
        public override Charset Check(Stream stream)
        {
            var leControlChars = 0;
            var beControlChars = 0;
            var numOddNulls = 0;
            var numEvenNulls = 0;

            var buffer = new byte[2];
            var size = stream.Length;

            while (stream.Read(buffer, 0, 2) == 2)
            {
                var ch1 = buffer[0];
                var ch2 = buffer[1];

                if (ch1 == 0) numEvenNulls++;
                if (ch2 == 0) numOddNulls++;

                if (ch1 == 0)
                {
                    if (ch2 is 0x0a or 0x0d)
                    {
                        ++beControlChars;
                    }
                }
                else if (ch2 == 0 && ch1 is 0x0a or 0x0d)
                {
                    ++leControlChars;
                }
            }

            // Checks if a buffer contains text that looks like UTF-16 by scanning for newline chars that 
            // would be present even in non-english text.

            if (leControlChars > 0 && beControlChars == 0)
            {
                return Charset.Utf16Le;
            }

            if (beControlChars > 0 && leControlChars == 0)
            {
                return Charset.Utf16Be;
            }

            // Checks if a buffer contains text that looks like UTF-16. This is done based the use of nulls 
            // which in ASCII/script like text can be useful to identify.

            var evenNullThreshold = numEvenNulls * 2d / size;
            var oddNullThreshold = numOddNulls * 2d / size;
            var expectedNullThreshold = ExpectedNullPercent / 100d;
            var unexpectedNullThreshold = UnexpectedNullPercent / 100d;

            // Lots of odd nulls, low number of even nulls
            if (evenNullThreshold < unexpectedNullThreshold && oddNullThreshold > expectedNullThreshold)
            {
                return Charset.Utf16Le;
            }

            // Lots of even nulls, low number of odd nulls
            if (oddNullThreshold < unexpectedNullThreshold && evenNullThreshold > expectedNullThreshold)
            {
                return Charset.Utf16Be;
            }

            // Don't know
            return Charset.None;
        }
    }
}