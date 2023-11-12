# UnicodeCharsetDetector #

[![Latest build](https://github.com/i2van/UnicodeCharsetDetector/workflows/build/badge.svg)](https://github.com/i2van/UnicodeCharsetDetector/actions)
[![NuGet](https://img.shields.io/nuget/v/UnicodeCharsetDetector.Standard)](https://www.nuget.org/packages/UnicodeCharsetDetector.Standard)
[![Downloads](https://img.shields.io/nuget/dt/UnicodeCharsetDetector.Standard)](https://www.nuget.org/packages/UnicodeCharsetDetector.Standard)
[![License](https://img.shields.io/badge/license-MIT-yellow)](https://opensource.org/licenses/MIT)

## Table of Contents ##

* [Description](#description)
* [Website](#website)
* [Download](#download)
* [Example](#example)
* [License](#license)

## Description ##

Text files Unicode charset w/wo [BOM](https://en.wikipedia.org/wiki/Byte_order_mark) detector. Encodings supported:

* `UTF-7`
* `UTF-8`
* `UTF-16`
* `UTF-32`

## Website ##

* [This project](https://github.com/i2van/UnicodeCharsetDetector)
* [Original project](https://github.com/posledam/UnicodeCharsetDetector)

## Download ##

[![NuGet](https://img.shields.io/nuget/v/UnicodeCharsetDetector.Standard)](https://www.nuget.org/packages/UnicodeCharsetDetector.Standard)

## Example ##

Example application can be found [here](https://github.com/i2van/UnicodeCharsetDetector/tree/master/src/UnicodeCharsetDetector.Example).

```csharp
// Get file encoding.

var unicodeCharsetDetector = new UnicodeCharsetDetector.UnicodeCharsetDetector();

using var stream = File.OpenRead(fileName);

var charset = unicodeCharsetDetector.Check(stream);
var encoding = charset.ToEncoding();
```

## License ##

* [LICENSE](https://github.com/i2van/UnicodeCharsetDetector/blob/master/LICENSE) ([MIT](https://opensource.org/licenses/MIT))
