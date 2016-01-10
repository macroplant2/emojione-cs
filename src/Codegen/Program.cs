using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Codegen {

    public class Program {

        /// <summary>
        /// Path to the emoji.json file.
        /// </summary>
        public string EmojiFile { get; set; } = "../../../../emoji.json";

        public string SourceDir { get; set; } = "../../../Emojione";

        public string WebDir { get; set; } = "../../../../bin";

        public static void Main(string[] args) {
            var program = new Program();
            program.Execute();
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public bool Execute() {
            try {
                // load and parse emoji.json
                var file = new FileInfo(EmojiFile);
                Console.WriteLine("Loading " + file.FullName);

                string json = File.ReadAllText(EmojiFile);
                var emojis = JsonConvert.DeserializeObject<Dictionary<string, Emoji>>(json);

                // write regex patternas and dictionaries to partial class
                Directory.CreateDirectory(SourceDir);
                file = new FileInfo(Path.Combine(SourceDir, "Emojione.generated.cs"));
                Console.WriteLine("Writing code to " + file.FullName);
                using (StreamWriter sw = new StreamWriter(Path.Combine(SourceDir, "Emojione.generated.cs"), false, Encoding.UTF8)) {
                    sw.WriteLine(@"using System.Collections.Generic;");
                    sw.WriteLine();
                    sw.WriteLine(@"namespace Emojione {");
                    sw.WriteLine();
                    sw.WriteLine(@"    public static partial class Emojione {");
                    sw.WriteLine();
                    var asciis = emojis.Values.Where(x => x.Asciis.Any());
                    sw.Write(@"        private const string ASCII_PATTERN = @""(?<=\s|^)(");
                    for (int i = 0; i < asciis.Count(); i++) {
                        var emoji = asciis.ElementAt(i);
                        for (int j = 0; j < emoji.Asciis.Count; j++) {
                            sw.Write(Regex.Escape(emoji.Asciis[j]));
                            if (j < emoji.Asciis.Count - 1) {
                                sw.Write("|");
                            }
                        }
                        if (i < asciis.Count() - 1) {
                            sw.Write("|");
                        }
                    }
                    sw.WriteLine(@")(?=\s|$|[!,\.])"";");
                    sw.WriteLine();
                    sw.WriteLine(@"        private const string IGNORE_PATTERN = @""<object[^>]*>.*?</object>|<span[^>]*>.*?</span>|<i[^>]*>.*?</i>|<(?:object|embed|svg|img|div|span|p|a)[^>]*>"";");
                    sw.WriteLine();
                    sw.Write(@"        private const string SHORTNAME_PATTERN = @""(");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        if (i > 0) {
                            sw.Write("|");
                        }
                        sw.Write(Regex.Escape(emoji.Shortname));
                        for (int j = 0; j < emoji.Aliases.Count; j++) {
                            sw.Write("|");
                            sw.Write(Regex.Escape(emoji.Aliases[j]));
                        }
                    }
                    sw.WriteLine(@")"";");
                    sw.WriteLine();
                    sw.Write(@"        private const string UNICODE_PATTERN = @""(");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(ToSurrogateString(emoji.Unicode));
                        if (!string.IsNullOrEmpty(emoji.Alternates)) {
                            sw.Write("|");
                            sw.Write(ToSurrogateString(emoji.Alternates));
                        }
                        if (i < emojis.Count - 1) {
                            sw.Write("|");
                        }
                    }
                    sw.WriteLine(@")"";");
                    sw.WriteLine();
                    sw.WriteLine(@"        private static readonly Dictionary<string, string> ASCII_TO_CODEPOINT = new Dictionary<string, string> {");
                    for (int i = 0; i < asciis.Count(); i++) {
                        var emoji = asciis.ElementAt(i);
                        for (int j = 0; j < emoji.Asciis.Count; j++) {
                            sw.Write(@"            [""{0}""] = ""{1}""", emoji.Asciis[j].Replace("\\", "\\\\"), emoji.Unicode.ToLower());
                            if (j < emoji.Asciis.Count - 1) {
                                sw.WriteLine(",");
                            }
                        }
                        if (i < asciis.Count() - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine(@"        };");
                    sw.WriteLine();
                    sw.WriteLine(@"        private static readonly Dictionary<string, string> CODEPOINT_TO_ASCII = new Dictionary<string, string> {");
                    for (int i = 0; i < asciis.Count(); i++) {
                        var emoji = asciis.ElementAt(i);
                        sw.Write(@"            [""{0}""] = ""{1}""", emoji.Unicode.ToLower(), emoji.Asciis.First().Replace("\\", "\\\\"));
                        if (i < asciis.Count() - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine(@"        };");
                    sw.WriteLine();
                    sw.WriteLine(@"        private static readonly Dictionary<string, string> CODEPOINT_TO_SHORTNAME = new Dictionary<string, string> {");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(@"            [""{0}""] = ""{1}""", emoji.Unicode.ToLower(), emoji.Shortname);
                        if (!string.IsNullOrEmpty(emoji.Alternates)) {
                            sw.WriteLine(",");
                            sw.Write(@"            [""{0}""] = ""{1}""", emoji.Alternates.ToLower(), emoji.Shortname);
                        }
                        if (i < emojis.Count - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine(@"        };");
                    sw.WriteLine();
                    sw.WriteLine(@"        private static readonly Dictionary<string, string> SHORTNAME_TO_CODEPOINT = new Dictionary<string, string> {");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(@"            [""{0}""] = ""{1}""", emoji.Shortname, emoji.Unicode.ToLower());
                        for (int j = 0; j < emoji.Aliases.Count; j++) {
                            sw.WriteLine(",");
                            sw.Write(@"            [""{0}""] = ""{1}""", emoji.Aliases[j], emoji.Unicode.ToLower());
                        }
                        if (i < emojis.Count - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine(@"        };");
                    sw.WriteLine(@"    }");
                    sw.WriteLine(@"}");
                }

                // write css
                Directory.CreateDirectory(WebDir);
                using (StreamWriter sw = new StreamWriter(Path.Combine(WebDir, "emojione.css"), false, Encoding.UTF8)) {
                    foreach (var emoji in emojis) {
                        sw.WriteLine(@".e1a-{0} {{
    background-image: url(""//cdn.jsdelivr.net/emojione/assets/svg/{1}.svg"");
}}", emoji.Value.Shortname.Replace("_", "-").Replace(":", ""), emoji.Value.Unicode);
                    }
                }

                // get categories
                var categories = new HashSet<string>();
                foreach (var emoji in emojis.Values) {
                    categories.Add(emoji.Category);
                }

                using (StreamWriter sw = new StreamWriter(Path.Combine(WebDir, "emojione.js"), false, Encoding.UTF8)) {

                    sw.WriteLine("    var categories = [");
                    for (int i = 0; i < categories.Count; i++) {
                        string cat = categories.ElementAt(i);
                        sw.Write("        {{ name: '{0}', key: '{1}', icon: '' }}", cat, cat);
                        if (i < categories.Count - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine("    ];");
                    sw.WriteLine();
                    sw.WriteLine(@"    var icons = [");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(@"        {{ unicode: '{0}', shortname: '{1}', category: '{2}', order: {3} }}", emoji.Unicode, emoji.Shortname, emoji.Category, emoji.EmojiOrder);
                        if (i < emojis.Count - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine("    ];");

                }


                Console.WriteLine("Done!");
            } catch (Exception e) {
                Console.WriteLine(e.Message);
                return false;
            }
            return true;
        }



        /// <summary>
        /// Converts a codepoint to unicode surrogate pairs
        /// </summary>
        /// <param name="unicode"></param>
        /// <returns></returns>
        private string ToSurrogateString(string codepoint) {
            var unicode = ToUnicode(codepoint);
            string s2 = "";
            for (int x = 0; x < unicode.Length; x++) {
                s2 += string.Format("\\u{0:X4}", (int)unicode[x]);
            }
            return s2;
        }

        /// <summary>
        /// Converts a unicode code point/code pair to a unicode character
        /// </summary>
        /// <param name="codepoints"></param>
        /// <returns></returns>
        private string ToUnicode(string codepoint) {
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
                    } else {
                        chars[i] = (char)part;
                    }
                }
                if (hilos.Any(x => x != null)) {
                    return string.Concat(hilos);
                } else {
                    return new String(chars);
                }

            } else {
                var i = Convert.ToInt32(codepoint, 16);
                return char.ConvertFromUtf32(i);
            }
        }


    }


}


