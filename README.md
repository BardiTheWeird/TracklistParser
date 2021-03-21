# What is this unfinished project?

It's a tool for "conveniently" parsing a tracklist, and then splitting an audio file accordingly. The splitting part is not (yet) implemented.

The idea behind this is that oftentimes you're required to manually create a .cue file when splitting a single big audio file, e.g. an entire album or a big mix of songs. 
However, working with a .cue file is not the most fun thing ever. That's why I tried to create this tool.

Instead of painstakingly inputting data into a .cue template, you can create a regex-based parsing template for a given tracklist. 
That would also go around certain limitations of .cue, e.g. a limited set of tags.

Parsing templates are designed as an HTML-template-esque kind of language which is parsed with a 1:1 correspondence into a set of commands written in C#.
The parser is written using the Sprache framework.

![](https://raw.githubusercontent.com/BardiTheWeird/TracklistParser/master/pics/tracklist-parser-input-code.png)
