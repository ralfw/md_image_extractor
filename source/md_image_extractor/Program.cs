using System;
using System.IO;
using System.Collections.Generic;

namespace md_image_extractor
{
	class MainClass
	{
		/*
		 * usage: mono md_image_extractor foo.md
		 * 
		 * Will create folder/file hierarchy next to source file:
		 *   foo.md.manuscript/
		 *     foo.md
		 *     images/
		 *       image1
		 *       image2
		 *       ...
		 * 
		 * Image embedded as data URI (http://en.wikipedia.org/wiki/Data_URI_scheme, http://www.nczonline.net/blog/2009/10/27/data-uris-explained/)
		 *  ![](data:image/*;base64,iVBORw0KG...
		 *  ...
		 *  Az0hpWiie32HAAAAAElFTkSuQmCC)
		 * 
		 * 
		 */

		public static void Main (string[] args)
		{
			var sourcefilename = Get_filename (args);
			var foldernames = Create_folders (sourcefilename);

			var markdown = Load_markdown (sourcefilename);
			Extract_embedded_images_from_markdown((string)foldernames.ImagesFoldername, markdown,
				cleanedMarkdown => Store_markdown(foldernames.ManuscriptFoldername, sourcefilename, cleanedMarkdown),
				(imagefilename, dataUri) => {
					var image = Deserialize_image(dataUri);
					Store_image(imagefilename, image);
				});
		}


		static string Get_filename(string[] args) {
			return args [0];
		}

		static dynamic Create_folders(string sourcefilename) {
			var manuscriptfoldername = sourcefilename + ".manuscript";

			if (Directory.Exists(manuscriptfoldername)) 
				Directory.Delete(manuscriptfoldername, true);
			Directory.CreateDirectory (manuscriptfoldername);

			var imagesfoldername = Path.Combine (manuscriptfoldername, "images");
			Directory.CreateDirectory (imagesfoldername);

			Console.WriteLine ("Created manuscript folder: {0}", manuscriptfoldername);

			return new{ ManuscriptFoldername = manuscriptfoldername, ImagesFoldername = imagesfoldername };
		}

		static string[] Load_markdown(string filename) {
			return File.ReadAllLines (filename);
		}

		static void Extract_embedded_images_from_markdown(string imagesfoldername, string[] markdown, 
														  Action<string[]> onCleanedMarkdown, 
														  Action<string,string> onImage) {
			var cleanedMarkdown = new List<string> ();
			var imagenumber = 0;
			var dataUri = "";

			Func<string> build_imagefilename = () => Path.Combine (imagesfoldername, "image" + imagenumber.ToString ());

			for (var i = 0; i < markdown.Length; i++) {
				var line = markdown [i].Trim ();

				if (string.IsNullOrEmpty (dataUri)) {
					if (line.StartsWith ("![](data:image")) {
						dataUri = line.Substring (4);
						//TODO: stores absolute path; not a good thing
						line = string.Format ("![{0}]", build_imagefilename());
						cleanedMarkdown.Add (line);
					} else
						cleanedMarkdown.Add (markdown [i]);
				} else {
					if (line.EndsWith (")")) {
						dataUri += line.Substring (0, line.Length - 1);
						onImage (build_imagefilename(), dataUri);
						dataUri = "";
						imagenumber++;
					} else
						dataUri += line;
				}
			}

			onCleanedMarkdown (cleanedMarkdown.ToArray());
		}

		static void Store_markdown(string manuscriptfoldername, string sourcefilename, string[] markdown) {
			var manuscriptfilename = Path.Combine (manuscriptfoldername, Path.GetFileName (sourcefilename));
			File.WriteAllLines (manuscriptfilename, markdown);
			Console.WriteLine ("  Stored manuscript: {0}", Path.GetFileName(manuscriptfilename));
		}

		static byte[] Deserialize_image(string dataUri) {
			return new byte[0];
		}

		static void Store_image(string imagefilename, byte[] data) {
			//TODO: really write the bytes
			File.WriteAllText (imagefilename, imagefilename);
			Console.WriteLine ("  Extracted image: {0}", Path.GetFileName(imagefilename));
		}
	}
}
