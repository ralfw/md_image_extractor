using System;
using System.IO;
using System.Collections.Generic;

namespace md_image_extractor
{
	class Domain {
		public static void Extract_embedded_images_from_markdown(string imagesfolderpath, string[] markdown, 
			Action<string[]> onCleanedMarkdown, 
			Action<string,string> onImage) {
			var cleanedMarkdown = new List<string> ();
			var imagenumber = 0;

			for (var i = 0; i < markdown.Length; i++) {
				var line = markdown [i].Trim ();

				if (line.StartsWith ("![](data:image")) {
					var dataUri = line.Substring (4, line.Length-4-1);
					var imagefilename = "image" + imagenumber.ToString ();
					onImage (imagefilename, dataUri);

					line = string.Format ("![]({0})", Path.Combine(imagesfolderpath, imagefilename));
					cleanedMarkdown.Add (line);

					imagenumber++;
				} else
					cleanedMarkdown.Add (markdown [i]);
			}

			onCleanedMarkdown (cleanedMarkdown.ToArray());
		}
	

		public static byte[] Deserialize_image(string dataUri) {
			Console.WriteLine ("  Data: {0}, ca. {1:###,###,###,###,##0} bytes", dataUri.Substring(0, Math.Min(dataUri.Length, 50)), dataUri.Length * 0.66);
			var i = dataUri.IndexOf (",");
			return Convert.FromBase64String (dataUri.Substring (i + 1));
		}
	}
}
