using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Emojione.Tests {
    [TestClass]
    public class EmojioneTests {

        [TestMethod]
        public void Version224Emoji() {
            // test that new emoji from v2.2.4 works
            string text = ":first_place:";
            string expected = $@"<img class=""emojione"" alt=""🥇"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f947.png{Emojione.CacheBustParam}"" />";
            string actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);

            text = ":avocado:";
            expected = "🥑";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            text = "🖤";
            expected = ":black_heart:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void NumberShouldNotBeConvertedToShortname() {
            string text = "2";
            string notexpected = ":digit_two:";
            string actual = Emojione.ToShort(text);
            Assert.AreNotEqual(notexpected, actual);
        }

        [TestMethod]
        public void AsciiToUnicode() {
            // single smiley
            string text = ":D";
            string expected = "😃";
            string actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // single smiley with incorrect case (shouldn't convert)
            text = ":d";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // multiple smileys
            text = ";) :p :* :)";
            expected = "😉 😛 😘 🙂";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to start a sentence
            text = @":\ is our confused smiley.";
            expected = "😕 is our confused smiley.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to end a sentence
            text = "Our smiley to represent joy is :')";
            expected = "Our smiley to represent joy is 😂";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to end a sentence with puncuation
            text = "The reverse to the joy smiley is the cry smiley :'(.";
            expected = "The reverse to the joy smiley is the cry smiley 😢.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley to end a sentence with preceeding punctuation
            text = @"This is the ""flushed"" smiley: :$.";
            expected = @"This is the ""flushed"" smiley: 😳.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley inside of an IMG tag (shouldn't convert anything inside of the tag)
            text = @"Smile <img class=""emojione"" alt="":)"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png"" /> because it's going to be a good day.";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // smiley inside of OBJECT tag  (shouldn't convert anything inside of the tag)
            text = @"Smile <object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1f604.svg"" type=""image/svg+xml"" standby="":)"">:)</object> because it's going to be a good day.";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // typical username password fail  (shouldn't convert the user:pass, but should convert the last :P)
            text = @"Please log-in with user:pass as your credentials :P.";
            expected = @"Please log-in with user:pass as your credentials 😛.";
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shouldn't replace an ascii smiley in a URL (shouldn't replace :/)
            text = @"Check out http://www.emojione.com";
            expected = text;
            actual = Emojione.AsciiToUnicode(text);
            Assert.AreEqual(expected, actual);
        }


        [TestMethod]
        public void ShortnameToImage() {
            // shortname to image
            string text = "Hello world! 😄 :smile:";
            string expected = $@"Hello world! 😄 <img class=""emojione"" alt=""😄"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png{Emojione.CacheBustParam}"" />";
            string actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);

            // shortname at start of sentence with apostrophe
            text = ":snail:'s are cool!";
            expected = $@"<img class=""emojione"" alt=""🐌"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png{Emojione.CacheBustParam}"" />'s are cool!";
            actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);

            // shortname shares a colon
            text = ":invalid:snail:";
            expected = $@":invalid<img class=""emojione"" alt=""🐌"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = ":alien: is 👽 and 저 is not :alien: or :alien: also :randomy: is not emoji";
            expected = $@"<img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> is 👽 and 저 is not <img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> or <img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> also :randomy: is not emoji";
            actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);

            // multiline emoji string
            text = ":dancer:\n:dancer:";
            expected = $"<img class=\"emojione\" alt=\"💃\" src=\"//cdn.jsdelivr.net/emojione/assets/png/1f483.png{Emojione.CacheBustParam}\" />\n<img class=\"emojione\" alt=\"💃\" src=\"//cdn.jsdelivr.net/emojione/assets/png/1f483.png{Emojione.CacheBustParam}\" />";
            actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);

            // triple emoji string
            text = ":dancer::dancer::alien:";
            expected = $@"<img class=""emojione"" alt=""💃"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f483.png{Emojione.CacheBustParam}"" /><img class=""emojione"" alt=""💃"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f483.png{Emojione.CacheBustParam}"" /><img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ShortnameToImage(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShortnameToUnicode() {
            // shortname to unicode
            string text = "Hello world! 😄 :smile:";
            string expected = "Hello world! 😄 😄";
            string actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // single shortname
            text = ":snail:";
            expected = "🐌";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname mid sentence with a comma
            text = "The :snail:, is Emoji One's official mascot.";
            expected = "The 🐌, is Emoji One's official mascot.";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at start of sentence
            text = ":snail: mail.";
            expected = "🐌 mail.";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at start of sentence with apostrophe
            text = ":snail:'s are cool!";
            expected = "🐌's are cool!";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname shares a colon
            text = ":invalid:snail:";
            expected = ":invalid🐌";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = ":alien: is 👽 and 저 is not :alien: or :alien: also :randomy: is not emoji";
            expected = "👽 is 👽 and 저 is not 👽 or 👽 also :randomy: is not emoji";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // multiline emoji string
            text = ":dancer:\n:dancer:";
            expected = "💃\n💃";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // triple emoji string
            text = ":dancer::dancer::alien:";
            expected = "💃💃👽";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at end of sentence
            text = "Emoji One's official mascot is :snail:.";
            expected = "Emoji One's official mascot is 🐌.";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at end of sentence with alternate punctuation
            text = "Emoji One's official mascot is :snail:!";
            expected = "Emoji One's official mascot is 🐌!";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname at end of sentence with preceeding colon
            text = "Emoji One's official mascot: :snail:";
            expected = "Emoji One's official mascot: 🐌";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname inside of IMG tag
            text = @"The <img class=""emojione"" alt="":snail:"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png"" /> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname inside of OBJECT tag
            text = @"The <object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1f40c.svg"" type=""image/svg+xml"" standby="":snail:"">:snail:</object> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname to unicode with code pairs
            text = ":nine:";
            expected = "9⃣";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);

            // shortname alias
            text = ":poo:";
            expected = "💩";
            actual = Emojione.ShortnameToUnicode(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToShort() {
            // to short
            string text = "Hello world! 😄 :smile:";
            string expected = "Hello world! :smile: :smile:";
            string actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // single unicode character conversion
            text = "🐌";
            expected = ":snail:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = "👽 is not :alien: and 저 is not 👽 or 👽";
            expected = ":alien: is not :alien: and 저 is not :alien: or :alien:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // multiline emoji string
            text = "💃\n💃";
            expected = ":dancer:\n:dancer:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // single character with surrogate pair
            text = "9⃣";
            expected = ":nine:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character mid sentence
            text = "The 🐌 is Emoji One's official mascot.";
            expected = "The :snail: is Emoji One's official mascot.";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character mid sentence with a comma
            text = "The 🐌, is Emoji One's official mascot.";
            expected = "The :snail:, is Emoji One's official mascot.";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character at start of sentence
            text = "🐌 mail.";
            expected = ":snail: mail.";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character at start of sentence with apostrophe
            text = "🐌's are cool!";
            expected = ":snail:'s are cool!";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character at end of sentence
            text = "Emoji One's official mascot is 🐌.";
            expected = "Emoji One's official mascot is :snail:.";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character at end of sentence with alternate puncuation
            text = "Emoji One's official mascot is 🐌!";
            expected = "Emoji One's official mascot is :snail:!";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character at end of sentence with preceeding colon
            text = "Emoji One's official mascot: 🐌";
            expected = "Emoji One's official mascot: :snail:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // character inside of IMG tag
            text = @"The <img class=""emojione"" alt=""🐌"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png"" /> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // characters inside of OBJECT tag
            text = @"The <object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1f40c.svg"" type=""image/svg+xml"" standby=""🐌"">🐌</object> is Emoji One's official mascot.";
            expected = text;
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);

            // unicode alternate to short
            text = "#️⃣"; // 0023-fe0f-20e3
            expected = ":hash:";
            actual = Emojione.ToShort(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnifyUnicode() {
            // unify unicode
            string text = "Hello world! 😄 :smile:";
            string expected = "Hello world! 😄 😄";
            string actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = ":alien: is 👽 and 저 is not :alien: or :alien: also :randomy: is not emoji";
            expected = "👽 is 👽 and 저 is not 👽 or 👽 also :randomy: is not emoji";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // multiline emoji string
            text = ":dancer:\n:dancer:";
            expected = "💃\n💃";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // triple emoji string
            text = ":dancer::dancer::alien:";
            expected = "💃💃👽";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // single unicode character conversion
            text = ":snail:";
            expected = "🐌";
            actual = Emojione.UnifyUnicode(text);
            Assert.AreEqual(expected, actual);

            // mixed unicode, shortname and ascii conversion
            text = "😄 :smile: :)";
            expected = "😄 😄 🙂";
            actual = Emojione.UnifyUnicode(text, ascii: true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToImage() {
            // to image
            string text = "Hello world! 😄 :smile:";
            string expected = $@"Hello world! <img class=""emojione"" alt=""😄"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png{Emojione.CacheBustParam}"" /> <img class=""emojione"" alt=""😄"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png{Emojione.CacheBustParam}"" />";
            string actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // mixed ascii, regular unicode and duplicate emoji
            text = ":alien: is 👽 and 저 is not :alien: or :alien: also :randomy: is not emoji";
            expected = $@"<img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> is <img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> and 저 is not <img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> or <img class=""emojione"" alt=""👽"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f47d.png{Emojione.CacheBustParam}"" /> also :randomy: is not emoji";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // single shortname conversion
            text = ":snail:";
            expected = $@"<img class=""emojione"" alt=""🐌"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // shortname shares a colon
            text = ":invalid:snail:";
            expected = $@":invalid<img class=""emojione"" alt=""🐌"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // single unicode character conversion
            text = "🐌";
            expected = $@"<img class=""emojione"" alt=""🐌"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f40c.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ToImage(text);
            Assert.AreEqual(expected, actual);

            // mixed unicode, shortname and ascii conversion
            text = "😄 :smile: :)";
            expected = $@"<img class=""emojione"" alt=""😄"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png{Emojione.CacheBustParam}"" /> <img class=""emojione"" alt=""😄"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png{Emojione.CacheBustParam}"" /> <img class=""emojione"" alt=""🙂"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f642.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ToImage(text, ascii: true);
            Assert.AreEqual(expected, actual);

            // shortname alt
            text = "😄";
            expected = $@"<img class=""emojione"" alt="":smile:"" src=""//cdn.jsdelivr.net/emojione/assets/png/1f604.png{Emojione.CacheBustParam}"" />";
            actual = Emojione.ToImage(text, unicodeAlt: false);
            Assert.AreEqual(expected, actual);

            // svg conversion
            text = "😄";
            expected = $@"<object class=""emojione"" data=""//cdn.jsdelivr.net/emojione/assets/svg/1f604.svg{Emojione.CacheBustParam}"" type=""image/svg+xml"" standby=""😄"">😄</object>";
            actual = Emojione.ToImage(text, svg: true);
            Assert.AreEqual(expected, actual);

            // png sprite
            text = "😄";
            expected = @"<span class=""emojione-1f604"" title="":smile:"">😄</span>";
            actual = Emojione.ToImage(text, sprites: true);
            Assert.AreEqual(expected, actual);

            // svg sprite
            text = "😄";
            expected = @"<svg class=""emojione""><description>😄</description><use xlink:href=""./../assets/sprites/emojione.sprites.svg#emoji-1f604""></use></svg>";
            actual = Emojione.ToImage(text, svg: true, sprites: true);
            Assert.AreEqual(expected, actual);

            // emojione-awesome
            text = "😄";
            expected = @"<i class=""e1a-smile""></i>";
            actual = Emojione.ToImage(text, awesome: true);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ShortnameToAscii() {
            string text = ":smiley: :slight_smile:";
            string expected = @":D :)";
            string actual = Emojione.ShortnameToAscii(text);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UnicodeToCodepoint() {
            string unicode = "😀"; // :grinning:
            string expected = "1f600";
            string actual = Emojione.ToCodePoint(unicode);
            Assert.AreEqual(expected, actual);

            expected = "D83D-DE00";
            actual = ShowX4(unicode);
            Assert.AreEqual(expected, actual);

            string codepoint = "1f600";
            expected = "😀";
            actual = Emojione.ToUnicode(codepoint);
            Assert.AreEqual(expected, actual);
            expected = "D83D-DE00";
            actual = ShowX4(actual);
            Assert.AreEqual(expected, actual);

            expected = "\uD83D\uDE00";
            actual = "😀";
            Assert.AreEqual(expected, actual);

            expected = "\\uD83D\\uDE00";
            actual = ToSurrogateString("1f600");
            Assert.AreEqual(expected, actual);
        }


        private static string ShowX4(string s, int? i = null) {
            string s2 = "";
            for (int x = 0; x < s.Length; x++) {
                s2 += string.Format("{0:X4}{1}", (int)s[x], ((x == s.Length - 1) ? String.Empty : "-"));
            }
            if (i != null) {
                return string.Format("{0} => {1:X}", s2, i);
            } else {
                return s2;
            }
        }

        /// <summary>
        /// Converts a unicode character to surrogate pairs
        /// </summary>
        /// <param name="unicode"></param>
        /// <returns></returns>
        private static string ToSurrogateString(string codepoint) {
            string unicode = Emojione.ToUnicode(codepoint);
            string s2 = "";
            for (int x = 0; x < unicode.Length; x++) {
                s2 += string.Format("\\u{0:X4}", (int)unicode[x]);
            }
            return s2;
        }

    }
}
