using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codegen {

        //"unicode": "1F604",
        //"unicode_alternates": [
        //],
        //"name": "smiling face with open mouth and smiling eyes",
        //"shortname": ":smile:",
        //"category": "people",
        //"emoji_order": "1",
        //"aliases": [
        //],
        //"aliases_ascii": [
        //    ":)",
        //    ":-)",
        //    "=]",
        //    "=)",
        //    ":]"
        //],
        //"keywords": [
        //    "face",
        //    "funny",
        //    "haha",
        //    "happy",
        //    "joy",
        //    "laugh",
        //    "smile",
        //    "smiley",
        //    "smiling"
        //]

    public class Emoji {

        public string Unicode { get; set; }

        public List<string> Alternates { get; set; }

        public string Name { get; set; }

        public string Shortname { get; set; }

        public string Category { get; set; }

        public int EmojiOrder { get; set; }

        public List<string> Aliases { get; set; }

        public List<string> Asciis { get; set; }

        public List<string> Keywords { get; set; }

        public Emoji() {
            Alternates = new List<string>();
            Aliases = new List<string>();
            Asciis = new List<string>();
            Keywords = new List<string>();
        }

        public override string ToString() {
            return Shortname + " (" + Unicode + ")";
        }
    }
}
