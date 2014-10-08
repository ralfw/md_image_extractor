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
		 * Sample files:
		 *   test1.md
		 *   "../../../../sample data/word2mdsample.md"
		 * 
		 */

		public static void Main (string[] args)
		{
			var sourcefilepath = CommandlineProvider.Get_filepath (args);
			var folderpaths = FilesystemProvider.Create_folders (sourcefilepath);

			var markdown = FilesystemProvider.Load_markdown (sourcefilepath);
			Domain.Extract_embedded_images_from_markdown((string)folderpaths.RelativeImagesFolderpath, markdown,
				cleanedMarkdown => FilesystemProvider.Store_markdown(folderpaths.ManuscriptFolderpath, sourcefilepath, cleanedMarkdown),
				(imagefilename, dataUri) => {
					var image = Domain.Deserialize_image(dataUri);
					FilesystemProvider.Store_image(folderpaths.ImagesFolderpath, imagefilename, image);
				});
		}
	}
}
