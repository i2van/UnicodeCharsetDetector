using System;
using System.IO;

using NUnit.Framework;

namespace UnicodeCharsetDetector.Tests;

[TestFixture]
public class UnicodeCharsetDetectorTests
{
    [Test]
    public void CheckBom_Detection_Success()
    {
        Assert.Throws<ArgumentNullException>(() => UnicodeCharsetDetector.CheckBom(null!, 0));
        Assert.Throws<ArgumentException>(() => UnicodeCharsetDetector.CheckBom([], 1));

        // UTF-7
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0x2B, 0x2F, 0x76, 0x38 ], 4), Is.EqualTo(Charset.Utf7Bom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0x2B, 0x2F, 0x76, 0x39 ], 4), Is.EqualTo(Charset.Utf7Bom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0x2B, 0x2F, 0x76, 0x2B ], 4), Is.EqualTo(Charset.Utf7Bom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0x2B, 0x2F, 0x76, 0x2F ], 4), Is.EqualTo(Charset.Utf7Bom));

        // UTF-7 BAD
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0x2B, 0x2F, 0x76, 0x2F ], 3), Is.Not.EqualTo(Charset.Utf7Bom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0x2B, 0x2F, 0x76, 0 ], 4), Is.Not.EqualTo(Charset.Utf7Bom));

        // UTF-8
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xEF, 0xBB, 0xBF ], 3), Is.EqualTo(Charset.Utf8Bom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xEF, 0xBB, 0xBF, 0 ], 4), Is.EqualTo(Charset.Utf8Bom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xEF, 0xBB, 0xBF, 1 ], 4), Is.EqualTo(Charset.Utf8Bom));

        // UTF-16 LE
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFF, 0xFE ], 2), Is.EqualTo(Charset.Utf16LeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFF, 0xFE, 0 ], 3), Is.EqualTo(Charset.Utf16LeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFF, 0xFE, 1 ], 3), Is.EqualTo(Charset.Utf16LeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFF, 0xFE, 1, 0 ], 4), Is.EqualTo(Charset.Utf16LeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFF, 0xFE, 0, 1 ], 4), Is.EqualTo(Charset.Utf16LeBom));

        // UTF-16 BE
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFE, 0xFF ], 2), Is.EqualTo(Charset.Utf16BeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFE, 0xFF, 0 ], 3), Is.EqualTo(Charset.Utf16BeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFE, 0xFF, 1 ], 3), Is.EqualTo(Charset.Utf16BeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFE, 0xFF, 1, 0 ], 4), Is.EqualTo(Charset.Utf16BeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFE, 0xFF, 0, 1 ], 4), Is.EqualTo(Charset.Utf16BeBom));

        // UTF-32 LE/BE
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0xFF, 0xFE, 0, 0 ], 4), Is.EqualTo(Charset.Utf32LeBom));
        Assert.That(UnicodeCharsetDetector.CheckBom([ 0, 0, 0xFE, 0xFF ], 4), Is.EqualTo(Charset.Utf32BeBom));
    }

    [Test]
    public void Check_ForDataFiles_Success()
    {
        var dataFolder = Path.Combine(TestContext.CurrentContext.TestDirectory, "Data");
        var testFiles = Directory.EnumerateFiles(dataFolder, "*.txt");
        var detector = new UnicodeCharsetDetector();
        foreach (var path in testFiles)
        {
            var fileName = Path.GetFileName(path);

            using var stream = File.OpenRead(path);

            var charset = detector.Check(stream);
            var charsetName = charset.ToString();

            Console.WriteLine("File: {0}, Charset: {1}", fileName, charset);

            Assert.That(fileName.StartsWith(charsetName, StringComparison.InvariantCultureIgnoreCase), Is.True);
        }
    }
}
