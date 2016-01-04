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
        public string EmojiFile { get; set; } = "emoji.json";

        public string OutputDir { get; set; }

        public static void Main(string[] args) {
            var program = new Program();
            if (args.Length == 1) {
                program.EmojiFile = args[0];
            }
            program.OutputDir = Environment.CurrentDirectory;
            program.Execute();
        }

        /// <summary>
        /// Executes the task.
        /// </summary>
        /// <returns></returns>
        public bool Execute() {
            try {
                // load emoji.json
                string json = File.ReadAllText(EmojiFile);

                // parse it
                var emojis = JsonConvert.DeserializeObject<Dictionary<string, Emoji>>(json);

                // get categories
                var categories = new HashSet<string>();
                foreach (var emoji in emojis.Values) {
                    categories.Add(emoji.Category);
                }

                // write css
                using (StreamWriter sw = new StreamWriter(Path.Combine(OutputDir, "emoji.css"), false, Encoding.UTF8)) {
                    foreach (var emoji in emojis) {
                        sw.WriteLine(@".e1-{0} {{
    background-image: url(""../img/e1/{1}.svg"");
}}", emoji.Value.Shortname.Replace("_", "-").Replace(":", ""), emoji.Value.Unicode.ToUpper());
                    }
                }

                // write dictionaries
                using (StreamWriter sw = new StreamWriter(Path.Combine(OutputDir, "Emoji.cs"), false, Encoding.UTF8)) {

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

                    sw.Write(@"        private const string UNICODE_PATTERN = @""(");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(ToSurrogateString(emoji.Unicode));
                        if (!string.IsNullOrEmpty(emoji.Alternates)) {
                            sw.Write("|");
                            sw.Write(ToSurrogateString(emoji.Alternates));
                        }
                        //if (emoji.Alternates.Any()) {
                        //    sw.Write("|");
                        //    for (int j = 0; j < emoji.Alternates.Count; j++) {
                        //        sw.Write(ToSurrogateString(emoji.Alternates[j]));
                        //        if (j < emoji.Alternates.Count - 1) {
                        //            sw.Write("|");
                        //        }
                        //    }
                        //}
                        if (i < emojis.Count - 1) {
                            sw.Write("|");
                        }
                    }
                    sw.WriteLine(@")"";");
                    sw.WriteLine();

                    sw.WriteLine(@"        private static readonly Dictionary<string, string> _ascii_to_codepoint = new Dictionary<string, string> {");
                    for (int i = 0; i < asciis.Count(); i++) {
                        var emoji = asciis.ElementAt(i);
                        for (int j = 0; j < emoji.Asciis.Count; j++) {
                            sw.Write(@"            {{""{0}"", ""{1}""}}", emoji.Asciis[j].Replace("\\", "\\\\"), emoji.Unicode.ToLower());
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


                    sw.WriteLine(@"        private static readonly Dictionary<string, string> _shortname_to_codepoint = new Dictionary<string, string> {");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(@"            {{""{0}"", ""{1}""}}", emoji.Shortname, emoji.Unicode.ToLower());
                        if (i < emojis.Count - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine(@"        };");
                    sw.WriteLine();
                    sw.WriteLine(@"        private static readonly Dictionary<string, string> _codepoint_to_shortname = new Dictionary<string, string> {");
                    for (int i = 0; i < emojis.Count; i++) {
                        var emoji = emojis.ElementAt(i).Value;
                        sw.Write(@"            {{""{0}"", ""{1}""}}", emoji.Unicode.ToLower(), emoji.Shortname);
                        if (!string.IsNullOrEmpty(emoji.Alternates)) {
                            sw.WriteLine(",");
                            sw.Write(@"            {{""{0}"", ""{1}""}}", emoji.Alternates.ToLower(), emoji.Shortname);
                        }
                        //    sw.WriteLine(",");
                        //    for (int j = 0; j < emoji.Alternates.Count; j++) {
                        //        sw.Write(@"            {{""{0}"", ""{1}""}}", emoji.Alternates[j].ToLower(), emoji.Shortname);
                        //        if (j < emoji.Alternates.Count - 1) {
                        //            sw.WriteLine(",");
                        //        }
                        //    }
                        //}
                        if (i < emojis.Count - 1) {
                            sw.WriteLine(",");
                        }
                    }
                    sw.WriteLine();
                    sw.WriteLine(@"        };");
                }

                using (StreamWriter sw = new StreamWriter(Path.Combine(OutputDir, "emoji.js"), false, Encoding.UTF8)) {

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


