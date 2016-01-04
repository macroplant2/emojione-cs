using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Codegen {

    /// <summary>
    /// "smile": {
    ///   "unicode": "1f604",
    ///   "unicode_alternates": "",
    ///   "name": "smiling face with open mouth and smiling eyes",
    ///   "shortname": ":smile:",
    ///   "category": "people",
    ///   "emoji_order": "6",
    ///   "aliases": [ ],
    ///   "aliases_ascii": [ ":)", ":-)", "=]", "=)", ":]" ],
    ///   "keywords": [ "funny", "haha", "happy", "joy", "laugh", "smile", "smiley", "eye", "person" ]
    /// }
    /// </summary>
    public class Emoji {

        [JsonProperty("unicode")]
        public string Unicode { get; set; }

        [JsonProperty("unicode_alternates")]
        public string Alternates { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shortname")]
        public string Shortname { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("emoji_order")]
        public int EmojiOrder { get; set; }

        [JsonProperty("aliases")]
        public List<string> Aliases { get; set; }

        [JsonProperty("aliases_ascii")]
        public List<string> Asciis { get; set; }

        [JsonProperty("keywords")]
        public List<string> Keywords { get; set; }

        public Emoji() {
            Aliases = new List<string>();
            Asciis = new List<string>();
            Keywords = new List<string>();
        }

        public override string ToString() {
            return Shortname + " (" + Unicode + ")";
        }
    }
}
