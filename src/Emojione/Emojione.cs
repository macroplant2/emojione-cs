//  The MIT License (MIT)
//  Copyright (c) 2016 Linus Birgerstam
//    
//  Permission is hereby granted, free of charge, to any person obtaining a copy of
//  this software and associated documentation files (the "Software"), to deal in
//  the Software without restriction, including without limitation the rights to
//  use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//  of the Software, and to permit persons to whom the Software is furnished to do
//  so, subject to the following conditions:
//    
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//    
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace Emojione {

  /// <summary>
  /// Helper class for converting emoji to different formats.
  /// </summary>
  public static partial class Emojione {

    private static string LocalImagePathPng { get; set; } = "pack://application:,,,/Emojione;component/PNGs/";

    /// <summary>
    /// 
    /// </summary>
    private static string ImagePathPng { get; set; } = "//cdn.jsdelivr.net/emojione/assets/png/";

    /// <summary>
    /// 
    /// </summary>
    private static string ImagePathSvg { get; set; } = "//cdn.jsdelivr.net/emojione/assets/svg/";

    /// <summary>
    /// 
    /// </summary>
    private static string ImagePathSvgSprites { get; set; } = "./../assets/sprites/emojione.sprites.svg";

    /// <summary>
    /// You can [optionally] modify this to force browsers to refresh their cache, it will be appended to the end of the filenames.
    /// </summary>
    internal static string CacheBustParam { get; set; } = "?v=2.2.6";

    /// <summary>
    /// Takes an input string containing both native unicode emoji and shortnames, and translates it into emoji images for display.
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="ascii"><c>true</c> to also convert ascii emoji to images.</param>
    /// <param name="unicodeAlt"><c>true</c> to use the unicode char instead of the shortname as the alt attribute (makes copy and pasting the resulting text better).</param>
    /// <param name="svg"><c>true</c> to use output svg markup instead of png</param>
    /// <param name="sprites"><c>true</c> to enable sprite mode instead of individual images.</param>
    /// <returns>A string with appropriate html for rendering emoji.</returns>
    public static string ToImage(string str, bool ascii = false, bool unicodeAlt = true, bool svg = false, bool sprites = false, bool awesome = false) {
      // first pass changes unicode characters into emoji markup
      str = UnicodeToImage(str, unicodeAlt, svg, sprites, awesome);
      // second pass changes any shortnames into emoji markup
      str = ShortnameToImage(str, ascii, unicodeAlt, svg, sprites, awesome);
      return str;
    }

    /// <summary>
    /// Unifies all emoji to their standard unicode types. 
    /// </summary>
    /// <param name="str">The input string.</param>
    /// <param name="ascii"><c>true</c> to also convert ascii emoji to unicode.</param>
    /// <returns>A string with standardized unicode.</returns>
    public static string UnifyUnicode(string str, bool ascii = false) {
      // transform all unicode into a standard shortname
      str = ToShort(str);
      // then transform the shortnames into unicode
      str = ShortnameToUnicode(str, ascii);
      return str;
    }

    /// <summary>
    /// Converts shortname emojis to unicode, useful for sending emojis back to mobile devices.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="ascii"><c>true</c> to also convert ascii emoji in the inpur string to unicode.</param>
    /// <returns>A string with unicode replacements</returns>
    public static string ShortnameToUnicode(string str, bool ascii = false) {
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + SHORTNAME_PATTERN, ShortnameToUnicodeCallback, RegexOptions.IgnoreCase);
      }
      if (ascii) {
        str = AsciiToUnicode(str);
      }
      return str;
    }

    /// <summary>
    /// This will replace shortnames with their ascii equivalent, e.g. :wink: -> ;^. 
    /// This is useful for systems that don't support unicode or images.
    /// </summary>
    /// <param name="str"></param>
    /// <returns>A string with ascii replacements.</returns>
    public static string ShortnameToAscii(string str) {
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + SHORTNAME_PATTERN, ShortnameToAsciiCallback, RegexOptions.IgnoreCase);
      }
      return str;
    }

    /// <summary>
    /// Takes input containing emoji shortnames and converts it to emoji images.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="ascii"><c>true</c> to also convert ascii emoji to images.</param>
    /// <param name="unicodeAlt"><c>true</c> to use the unicode char instead of the shortname as the alt attribute (makes copy and pasting the resulting text better).</param>
    /// <param name="svg"><c>true</c> to use output svg markup instead of png</param>
    /// <param name="sprites"><c>true</c> to enable sprite mode instead of individual images.</param>
    /// <returns>A string with appropriate html for rendering emoji.</returns>
    public static string ShortnameToImage(string str, bool ascii = false, bool unicodeAlt = true, bool svg = false, bool sprites = false, bool awesome = false) {
      if (ascii) {
        str = AsciiToShortname(str);
      }
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + SHORTNAME_PATTERN, match => ShortnameToImageCallback(match, unicodeAlt, svg, sprites, awesome), RegexOptions.IgnoreCase);
      }
      return str;
    }

    /// <summary>
    /// Converts unicode emoji to shortnames.
    /// </summary>
    /// <param name="str">The input string</param>
    /// <returns>A string with shortname replacements.</returns>
    public static string ToShort(string str) {
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + UNICODE_PATTERN, UnicodeToShortnameCallback);
      }
      return str;
    }

    /// <summary>
    /// Takes native unicode emoji input, such as that from your mobile device, and outputs image markup (png or svg).
    /// </summary>
    /// <param name="str">The input string</param>
    /// <param name="unicodeAlt"><c>true</c> to use the unicode char instead of the shortname as the alt attribute (makes copy and pasting the resulting text better).</param>
    /// <param name="svg"><c>true</c> to use output svg markup instead of png</param>
    /// <param name="sprites"><c>true</c> to enable sprite mode instead of individual images.</param>
    /// <returns>A string with appropriate html for rendering emoji.</returns>
    public static string UnicodeToImage(string str, bool unicodeAlt = true, bool svg = false, bool sprites = false, bool awesome = false) {
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + UNICODE_PATTERN, match => UnicodeToImageCallback(match, unicodeAlt, svg, sprites, awesome));
      }
      return str;
    }

    public static List<Inline> UnicodeToInlines(string str, int size = 12, bool unicodeAlt = true, bool svg = false, bool sprites = false, bool awesome = false) {
      if (str == null) return null;
      return Regex.Split(str, UNICODE_PATTERN).Select(s => GetInlineWithString(s, size)).ToList();
    }
    public static Inline GetInlineWithString(string str, int size = 12) {
      if (!Regex.IsMatch(str, UNICODE_PATTERN)) return new Run(str);
      var path = UnicodeToImageUrlCallback(str);
      if (path == null) return new Run(str);
      return new InlineUIContainer(new Image { Source = new BitmapImage(new Uri(path)), Height = size, Width = size });
    }
    public static string UnicodeToImageUrlCallback(string emoji) {
      // Remove the variation modifier (if it is present) - the PNG names do not include it.
      if (emoji.Length == 2 && emoji[1] == '\uFE0F')
        emoji = emoji.Substring(0, 1);
      if (!CODEPOINTS.ContainsKey(emoji)) return null;
      return string.Format(@"{0}{1}.png", LocalImagePathPng, CODEPOINTS[emoji]);
    }


    /// <summary>
    /// Converts ascii emoji to unicode, e.g. :) -> 😄
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string AsciiToUnicode(string str) {
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + ASCII_PATTERN, AsciiToUnicodeCallback);
      }
      return str;
    }

    /// <summary>
    /// Converts ascii emoji to shortname, e.g. :) -> :smile:
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string AsciiToShortname(string str) {
      if (str != null) {
        str = Regex.Replace(str, IGNORE_PATTERN + "|" + ASCII_PATTERN, AsciiToShortnameCallback);
      }
      return str;
    }

    private static string AsciiToUnicodeCallback(Match match) {
      // check if the emoji exists in our dictionaries
      var ascii = match.Value;
      if (ASCII_TO_CODEPOINT.ContainsKey(ascii)) {
        // convert codepoint to unicode char
        return ToUnicode(ASCII_TO_CODEPOINT[ascii]);
      }
      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    private static string AsciiToShortnameCallback(Match match) {
      // check if the emoji exists in our dictionaries
      var ascii = match.Value;
      if (ASCII_TO_CODEPOINT.ContainsKey(ascii)) {
        var codepoint = ASCII_TO_CODEPOINT[ascii];
        if (CODEPOINT_TO_SHORTNAME.ContainsKey(codepoint)) {
          return CODEPOINT_TO_SHORTNAME[codepoint];
        }
      }
      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    private static string ShortnameToImageCallback(Match match, bool unicodeAlt, bool svg, bool sprites, bool awesome) {
      // check if the emoji exists in our dictionaries
      var shortname = match.Value;
      if (SHORTNAME_TO_CODEPOINT.ContainsKey(shortname)) {
        var codepoint = SHORTNAME_TO_CODEPOINT[shortname];
        string alt = unicodeAlt ? ToUnicode(codepoint) : shortname;
        if (awesome) {
          return string.Format(@"<i class=""e1a-{0}""></i>", shortname.Replace("_", "-").Replace(":", ""));
        }
        else if (svg) {
          if (sprites) {
            return string.Format(@"<svg class=""emojione""><description>{0}</description><use xlink:href=""{1}#emoji-{2}""></use></svg>", unicodeAlt ? alt : shortname, ImagePathSvgSprites, codepoint);
          }
          else {
            return string.Format(@"<object class=""emojione"" data=""{0}{1}.svg{2}"" type=""image/svg+xml"" standby=""{3}"">{3}</object>", ImagePathSvg, codepoint, CacheBustParam, unicodeAlt ? alt : shortname);
          }
        }
        else {
          if (sprites) {
            return string.Format(@"<span class=""emojione emojione-{0}"" title=""{1}"">{2}</span>", codepoint, shortname, unicodeAlt ? alt : shortname);
          }
          else {
            return string.Format(@"<img class=""emojione"" alt=""{0}"" src=""{1}{2}.png{3}"" />", unicodeAlt ? alt : shortname, ImagePathPng, codepoint, CacheBustParam);
          }
        }
      }

      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    private static string ShortnameToAsciiCallback(Match match) {
      // check if the emoji exists in our dictionaries
      var shortname = match.Value;
      if (SHORTNAME_TO_CODEPOINT.ContainsKey(shortname)) {
        var codepoint = SHORTNAME_TO_CODEPOINT[shortname];
        if (CODEPOINT_TO_ASCII.ContainsKey(codepoint)) {
          return CODEPOINT_TO_ASCII[codepoint];
        }
      }

      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    private static string ShortnameToUnicodeCallback(Match match) {
      // check if the emoji exists in our dictionaries
      var shortname = match.Value;
      if (SHORTNAME_TO_CODEPOINT.ContainsKey(shortname)) {
        // convert codepoint to unicode char
        return ToUnicode(SHORTNAME_TO_CODEPOINT[shortname]);
      }

      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    private static string UnicodeToImageCallback(Match match, bool unicodeAlt, bool svg, bool sprites, bool awesome) {
      // check if the emoji exists in our dictionaries
      var codepoint = ToCodePoint(match.Groups[1].Value);
      if (CODEPOINT_TO_SHORTNAME.ContainsKey(codepoint)) {
        var shortname = CODEPOINT_TO_SHORTNAME[codepoint];
        string alt = unicodeAlt ? ToUnicode(codepoint) : shortname;
        if (awesome) {
          return string.Format(@"<i class=""e1a-{0}""></i>", shortname.Replace("_", "-").Replace(":", ""));
        }
        else if (svg) {
          if (sprites) {
            return string.Format(@"<svg class=""emojione""><description>{0}</description><use xlink:href=""{1}#emoji-{2}""></use></svg>", unicodeAlt ? alt : shortname, ImagePathSvgSprites, codepoint);
          }
          else {
            return string.Format(@"<object class=""emojione"" data=""{0}{1}.svg{2}"" type=""image/svg+xml"" standby=""{3}"">{3}</object>", ImagePathSvg, codepoint, CacheBustParam, unicodeAlt ? alt : shortname);
          }
        }
        else {
          if (sprites) {
            return string.Format(@"<span class=""emojione emojione-{0}"" title=""{1}"">{2}</span>", codepoint, shortname, unicodeAlt ? alt : shortname);
          }
          else {
            return string.Format(@"<img class=""emojione"" alt=""{0}"" src=""{1}{2}.png{3}"" />", unicodeAlt ? alt : shortname, ImagePathPng, codepoint, CacheBustParam);
          }
        }
      }

      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    private static string UnicodeToShortnameCallback(Match match) {
      // check if the emoji exists in our dictionaries
      var unicode = match.Groups[1].Value;
      var codepoint = ToCodePoint(unicode);
      if (CODEPOINT_TO_SHORTNAME.ContainsKey(codepoint)) {
        return CODEPOINT_TO_SHORTNAME[codepoint];
      }

      // we didn't find a replacement so just return the entire match
      return match.Value;
    }

    /// <summary>
    /// Convert a unicode character to its code point/code pair
    /// </summary>
    /// <param name="unicode"></param>
    /// <returns></returns>
    internal static string ToCodePoint(string unicode) {
      string codepoint = "";
      for (var i = 0; i < unicode.Length; i += char.IsSurrogatePair(unicode, i) ? 2 : 1) {
        if (i > 0) {
          codepoint += "-";
        }
        codepoint += string.Format("{0:X4}", char.ConvertToUtf32(unicode, i));
      }
      return codepoint.ToLower();
    }

    /// <summary>
    /// Converts a unicode code point/code pair to a unicode character
    /// </summary>
    /// <param name="codepoints"></param>
    /// <returns></returns>
    internal static string ToUnicode(string codepoint) {
      if (codepoint.Contains('-')) {
        var pair = codepoint.Split('-');
        string[] hilos = new string[pair.Length];
        char[] chars = new char[pair.Length];
        for (int i = 0; i < pair.Length; i++) {
          var part = Convert.ToInt32(pair[i], 16);
          if (part >= 0x10000 && part <= 0x10FFFF) {
            var hi = Math.Floor((decimal)(part - 0x10000) / 0x400) + 0xD800;
            var lo = ((part - 0x10000) % 0x400) + 0xDC00;
            hilos[i] = new String(new char[] { (char)hi, (char)lo });
          }
          else {
            chars[i] = (char)part;
          }
        }
        if (hilos.Any(x => x != null)) {
          return string.Concat(hilos);
        }
        else {
          return new String(chars);
        }

      }
      else {
        var i = Convert.ToInt32(codepoint, 16);
        return char.ConvertFromUtf32(i);
      }
    }
  }
}
