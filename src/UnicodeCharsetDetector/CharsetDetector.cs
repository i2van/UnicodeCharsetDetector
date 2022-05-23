using System.IO;

namespace UnicodeCharsetDetector
{
    /// <summary>
    /// Provides base class for a charset detector.
    /// </summary>
    public abstract class CharsetDetector
    {
        /// <summary>
        /// Checks the charset of the stream.
        /// </summary>
        /// <param name="stream">The stream which charset to check.</param>
        /// <returns>The charset of the stream.</returns>
        public abstract Charset Check(Stream stream);
    }
}
