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

    public static bool IsShortEmojiOnlySequence(string str) { return ShortEmojiSequenceRegex.IsMatch(str); }
    public static List<Inline> UnicodeToInlines(string str, int size) {
      if (str == null) return null;
      return SingleEmojiRegex.Split(str).Select(s => GetInlineWithString(s, size)).ToList();
    }
    public static Inline GetInlineWithString(string str, int size) {
      if (!SingleEmojiRegex.IsMatch(str)) return new Run(str);
      var path = UnicodeToImageUrlCallback(str);
      if (path == null) return new Run(str);
      return new InlineUIContainer(new Image { Source = new BitmapImage(new Uri(path)), Height = size, Width = size });
    }
    public static string UnicodeToImageUrlCallback(string emoji) {
      if (!Codepoints.ContainsKey(emoji)) {
        emoji += "\uFE0F";
        if (!Codepoints.ContainsKey(emoji)) return null;
      }
      
      return string.Format(@"{0}{1}.png", LocalImagePathPng, Codepoints[emoji]);
    }
  }
}
