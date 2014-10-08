using System;
using System.IO;
using System.Collections.Generic;

namespace md_image_extractor
{
	class CommandlineProvider {
		public static string Get_filepath(string[] args) {
			return args [0];
		}
	}
}
