﻿using System;
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
			var sourcefilepath = Get_filepath (args);
			var folderpaths = Create_folders (sourcefilepath);

			var markdown = Load_markdown (sourcefilepath);
			Extract_embedded_images_from_markdown((string)folderpaths.RelativeImagesFolderpath, markdown,
				cleanedMarkdown => Store_markdown(folderpaths.ManuscriptFolderpath, sourcefilepath, cleanedMarkdown),
				(imagefilename, dataUri) => {
					var image = Deserialize_image(dataUri);
					Store_image(folderpaths.ImagesFolderpath, imagefilename, image);
				});
		}


		static string Get_filepath(string[] args) {
			return args [0];
		}

		static dynamic Create_folders(string sourcefilename) {
			var manuscriptfolderpath = sourcefilename + ".manuscript";

			if (Directory.Exists(manuscriptfolderpath)) 
				Directory.Delete(manuscriptfolderpath, true);
			Directory.CreateDirectory (manuscriptfolderpath);

			var imagesfolderpath = Path.Combine (manuscriptfolderpath, "images");
			Directory.CreateDirectory (imagesfolderpath);

			Console.WriteLine ("Created manuscript folder: {0}", manuscriptfolderpath);

			return new{ ManuscriptFolderpath = manuscriptfolderpath, ImagesFolderpath = imagesfolderpath, RelativeImagesFolderpath = "images" };
		}

		static string[] Load_markdown(string filepath) {
			return File.ReadAllLines (filepath);
		}

		static void Extract_embedded_images_from_markdown(string imagesfolderpath, string[] markdown, 
														  Action<string[]> onCleanedMarkdown, 
														  Action<string,string> onImage) {
			var cleanedMarkdown = new List<string> ();
			var imagenumber = 0;
			var imagefilename = "";
			var dataUri = "";

			for (var i = 0; i < markdown.Length; i++) {
				var line = markdown [i].Trim ();

				if (string.IsNullOrEmpty (dataUri)) {
					if (line.StartsWith ("![](data:image")) {
						dataUri = line.Substring (4);
						imagefilename = "image" + imagenumber.ToString ();
						line = string.Format ("![{0}]", Path.Combine(imagesfolderpath, imagefilename));
						cleanedMarkdown.Add (line);
					} else
						cleanedMarkdown.Add (markdown [i]);
				} else {
					if (line.EndsWith (")")) {
						dataUri += line.Substring (0, line.Length - 1);
						onImage (imagefilename, dataUri);
						dataUri = "";
						imagenumber++;
					} else
						dataUri += line;
				}
			}

			onCleanedMarkdown (cleanedMarkdown.ToArray());
		}

		static void Store_markdown(string manuscriptfolderpath, string sourcefilepath, string[] markdown) {
			var manuscriptfilepath = Path.Combine (manuscriptfolderpath, Path.GetFileName (sourcefilepath));
			File.WriteAllLines (manuscriptfilepath, markdown);
			Console.WriteLine ("  Stored manuscript: {0}", Path.GetFileName(manuscriptfilepath));
		}

		static byte[] Deserialize_image(string dataUri) {
			Console.WriteLine ("<<<{0}>>>", dataUri);
			return new byte[]{ 1, 2, 3 };
		}

		static void Store_image(string imagesfolderpath, string imagefilename, byte[] data) {
			var imagefilepath = Path.Combine (imagesfolderpath, imagefilename);
			File.WriteAllBytes (imagefilepath, data);
			Console.WriteLine ("  Extracted image: {0}", imagefilename);
		}
	}
}