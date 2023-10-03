using System;
using System.IO;

namespace UnicodeCharsetDetector;

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
    /// <exception cref="IOException">An I/O error occurred.</exception>
    /// <exception cref="NotSupportedException">The stream does not support seeking, such as if the <see cref="FileStream" /> is constructed from a pipe or console output.</exception>
    /// <exception cref="ObjectDisposedException">Methods were called after the stream was closed.</exception>
    public abstract Charset Check(Stream stream);
}
