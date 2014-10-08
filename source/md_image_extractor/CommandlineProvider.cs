using System;
using System.IO;
using System.Collections.Generic;

namespace md_image_extractor
{
	class CommandlineProvider {
		public static string Get_filepath(string[] args) {
			if (args.Length == 0) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Error.WriteLine ("Usage: [mono] md_image_extractor.exe filename");
				Console.ResetColor ();
				Environment.Exit (1);
			}
			return args [0];
		}
	}
}
