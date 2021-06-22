using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using UnicodeCharsetDetector;

class Program
{
    private static readonly UnicodeCharsetDetector.UnicodeCharsetDetector UnicodeCharsetDetector = new();

    static int Main(string[] args)
    {
        if (!args.Any())
        {
            Console.WriteLine($"Usage: {Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly()!.Location)} file [files]{Environment.NewLine}{Environment.NewLine}Detect file encoding.");
            return 1;
        }

        foreach (var fileName in args)
        {
            var encoding = DetectEncoding(fileName);

            Console.WriteLine($"{fileName}{Environment.NewLine}{encoding?.WebName ?? "Could not detect encoding."}");
        }

        return 0;

        static Encoding? DetectEncoding(string fileName)
        {
            try
            {
                using var stream = File.OpenRead(fileName);
                return UnicodeCharsetDetector.Check(stream).ToEncoding();
            }
            catch
            {
                return null;
            }
        }
    }
}
