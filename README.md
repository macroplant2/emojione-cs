# emojione-cs

A C# implementation of Emojione's lib for converting Unicode emoji characters to :shortnames: and :shortnames: to emoji images etc. 

## Build

* Open Emojione.sln
* Build the Codegen project.
* Run Codegen.exe to generate the partial class Emojione.generated.cs. 
* Build the Emojione project.

## Usage

Add a reference to Emojione.dll in your application and use the following methods to convert Unicode emoji characters to :shortnames: and :shortnames: to emoji images etc.

`Emojione.ToShort()`  
Converts unicode emoji to shortnames.

`Emojione.ShortnameToImage()`  
Takes input containing emoji shortnames and converts it to emoji images.

`Emojione.UnicodeToImage()`  
Takes native unicode emoji input, such as that from your mobile device, and outputs image markup (png or svg).

`Emojione.ToImage()`  
Takes an input string containing both native unicode emoji and shortnames, and translates it into emoji images for display.

---

`Emojione.ShortnameToAscii()`  
Replaces shortnames with their ascii equivalent, e.g. :smile: -> :)  This is useful for systems that don't support unicode or images.

`Emojione.ShortnameToUnicode()`  
Converts shortname emojis to unicode, useful for sending emojis back to mobile devices.

`Emojione.UnifyUnicode()`  
Unifies all emoji to their standard unicode types. 

## License
emojione-cs is under the [MIT license](http://opensource.org/licenses/MIT).