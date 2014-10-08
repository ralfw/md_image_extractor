using System;
using System.IO;
using System.Collections.Generic;

namespace md_image_extractor
{
	class FilesystemProvider {
		public static dynamic Create_folders(string sourcefilename) {
			var manuscriptfolderpath = sourcefilename + ".manuscript";

			if (Directory.Exists(manuscriptfolderpath)) 
				Directory.Delete(manuscriptfolderpath, true);
			Directory.CreateDirectory (manuscriptfolderpath);

			var imagesfolderpath = Path.Combine (manuscriptfolderpath, "images");
			Directory.CreateDirectory (imagesfolderpath);

			Console.WriteLine ("Created manuscript folder: {0}", manuscriptfolderpath);

			return new{ ManuscriptFolderpath = manuscriptfolderpath, ImagesFolderpath = imagesfolderpath, RelativeImagesFolderpath = "images" };
		}


		public static string[] Load_markdown(string filepath) {
			return File.ReadAllLines (filepath);
		}

		public static void Store_markdown(string manuscriptfolderpath, string sourcefilepath, string[] markdown) {
			var manuscriptfilepath = Path.Combine (manuscriptfolderpath, Path.GetFileName (sourcefilepath));
			File.WriteAllLines (manuscriptfilepath, markdown);
			Console.WriteLine ("  Stored manuscript: {0}", Path.GetFileName(manuscriptfilepath));
		}


		public static void Store_image(string imagesfolderpath, string imagefilename, byte[] data) {
			var imagefilepath = Path.Combine (imagesfolderpath, imagefilename);
			File.WriteAllBytes (imagefilepath, data);
			Console.WriteLine ("  Extracted image: {0}", imagefilename);
		}
	}
}
