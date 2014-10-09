md_image_extractor
==================

Extract images embedded in data URIs in markdown textfiles.

1. Upload a .doc(x) file to this service to get it converted to Markdown: [https://word-to-markdown.herokuapp.com](https://word-to-markdown.herokuapp.com).
2. What you get back is a Markdown text into which any images are embedded using [data URIs](http://en.wikipedia.org/wiki/Data_URI_scheme).
3. Store the Markdown text in a .md file, e.g. _foo.md_.
4. If any images are embedded run _md_image_extractor.exe foo.md_ in the file's folder.[^fmono]

What you'll get is a new folder named like your .md file with ".manuscript" appended.

In that folder you find the .md file with the embedded images replaced by links to image files.
The images will have been extracted to a subfolder called "images". Therein they are named "image0", "image1" etc.

If you open the new .md file with a Markdown editor like [Mou](http://25.io/mou/) or [MarkdownPad](http://markdownpad.com) you should see the images correctly rendered in the preview pane.

Now you can go on an take the manuscript folder content and put it in a [Leanpub](http://leanpub.com) manuscript folder in your Dropbox.

Enjoy!

[^fmono]: This should work on Windows machines where .NET is already installed by default. On Linux or OSX install the [Mono runtime](http://www.mono-project.com/download/) first. Then run the program like this: _mono md_image_extractor.exe foo.md_.
