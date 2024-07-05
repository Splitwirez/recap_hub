using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReCap.Builder
{
	public static class CLIHelp
    {
		const string BOOL_VALS = " <true|false>";
		public static void PrintHelp(bool showModes, bool showOptions)
		{
			Console.WriteLine("==== DOCUMENTATION FOR RESURRECTION CAPSULE PUBLISH ASSISTANT ====");
			Console.WriteLine("\n");

			
			

			Console.WriteLine("Command usage:");
			PrintTopLevelHelp();
			if (showModes)
			{
				Console.WriteLine($"\t{ScriptFileName} {PublishArgs.PUB_MODE} [PUBLISH MODE]");
			}
			if (showModes && showOptions)
				Console.WriteLine("or");
			if (showOptions)
				Console.WriteLine($"\t{ScriptFileName} [OPTIONS, IF DESIRED]");
			Console.WriteLine("\n");

			if (showModes)
			{
				PrintModes();
			}

			if (showOptions)
			{
				if (showModes)
					Console.WriteLine("\n");

				PrintOptions();
			}
		}

		public static void PrintTopLevelHelp()
		{
			Console.WriteLine($"\t{ScriptFileName} {PublishArgs.PUB_MODE} [MODE] for [PLATFORM]");
			Console.WriteLine($"to publish for the target mode and platform, or");
			Console.WriteLine($"\t{ScriptFileName} {PublishArgs.PUB_MODE} [OPTIONS...]");
			Console.WriteLine($"to publish with the specified individual options, or");
			Console.WriteLine($"\t{ScriptFileName} for me");
			Console.WriteLine($"to produce a build for your own use on the current machine, or");
			Console.WriteLine($"\t{ScriptFileName} --help");
			Console.WriteLine("for more information.");
		}

		public static readonly string ScriptFileName = new Func<string>(() =>
			{
				bool isWindows = OperatingSystem.IsWindows();
				string scriptFileName = "publish";
				scriptFileName += isWindows
					? ".bat"
					: ".sh"
				;

				if (!isWindows)
					scriptFileName = $"./{scriptFileName}";
				return scriptFileName;
			})();

		static void PrintModes()
		{
			Console.WriteLine($"== PUBLISH MODES ==");
			Console.WriteLine("\n");
			Console.WriteLine($"{PublishArgs.MODE_DEV_PREFIX}{PublishArgs.MODE_DEV_DEMO_SUFFIX}");
			Console.WriteLine($"{PublishArgs.MODE_PUBLIC_PREFIX}{PublishArgs.MODE_PUBLIC_LOCAL_SUFFIX}");
			Console.WriteLine($"{PublishArgs.MODE_PUBLIC_PREFIX}{PublishArgs.MODE_PUBLIC_ONLINE_SUFFIX}");
		}

		static void PrintOptions()
		{
			Console.WriteLine($"== AVAILABLE OPTIONS ==");
			Console.WriteLine("\n");

			
			string argTitle = PublishArgs.GET_HELP.First();
			foreach (string argAltName in PublishArgs.GET_HELP.Skip(1))
			{
				argTitle += "|" + argAltName;
			}
			Console.WriteLine(argTitle);
			Console.WriteLine("\tShow the help information you're looking at right now.");
			Console.WriteLine($"\tNo publishing will be performed if this option is passed by any of its {PublishArgs.GET_HELP.Count} names.");
			
			Console.WriteLine("\n");
			argTitle = PublishArgs.PUB_TARGET + " <";
			var platformValues = PublishArgs.PLATFORM_VALUES.Replace(" ", string.Empty).Replace("'", string.Empty).Trim(',').Split(",");
			argTitle += platformValues[0];
			foreach (string targetName in platformValues.Skip(1))
			{
				argTitle += "|" + targetName;//$"\t\t{targetName}");
			}
			argTitle += ">";
			Console.WriteLine(argTitle);
			Console.WriteLine("\tSelect which OS/platform to publish for."); //Acceptable values are as follows:");
			//Console.WriteLine($",".Replace(", ", "\n\t\t"));
			Console.WriteLine("\tCurrent OS is used if this option is not specified on a supported OS. On an unsupported OS, publishing will be aborted.");
			Console.WriteLine("\tIMPORTANT FRUIT WARNING:");
			Console.WriteLine($"\t\tThe {PublishTarget.MacOS.ToString()} target is planned, but is not fully implemented into the publish assistant at this time."
			+ "\n\t\tVitor, I'll probably need you to pass that one so we can figure stuff out."
			+ "\n\t\tAnyone else reading this, pretend there's no such target until further notice, unless you know .NET development on macOS better than I do (aka \"at all\").");
			
			
			Console.WriteLine("\n");
			Console.WriteLine(PublishArgs.SELF_CONTAINED + BOOL_VALS);
			Console.WriteLine("\tIf true, packages the .NET runtime in with the Resurrection Capsule Hub, avoiding the need for it to be installed separately. (larger file size)");
			Console.WriteLine("\tIf false, requires a compatible .NET runtime to be installed separately. (smaller file size)");
			Console.WriteLine("\tDefaults to true if not specified.");
			
			
			Console.WriteLine("\n");
			Console.WriteLine(PublishArgs.SINGLE_FILE + BOOL_VALS);
			Console.WriteLine("\tIf true, smooshes the Resurrection Capsule Hub down to as few files as possible (one file, ideally).");
			Console.WriteLine("\tIf false, leaves all files separated from one another.");
			Console.WriteLine("\tDefaults to false if not specified.");
			
			
			Console.WriteLine("\n");
			Console.WriteLine(PublishArgs.TRIM + BOOL_VALS);
			Console.WriteLine("\tIf true, trims out unused portions of the .NET runtime and any other dependencies to reduce file size.");
			Console.WriteLine("\tIf false, leaves everything in.");
			Console.WriteLine("\tDefaults to false if not specified.");
			Console.WriteLine($"\tIgnored if {PublishArgs.SELF_CONTAINED} is false.");
			
			
			Console.WriteLine("\n");
			argTitle = PublishArgs.CONFIGURATION + " <";
			argTitle += "Debug";
			argTitle += "|";
			argTitle += "Release";
			argTitle += ">";
			Console.WriteLine(argTitle);
			Console.WriteLine("\tSpecifies the build configuration to use when publishing. Currently, acceptable values are as follows:");
			Console.WriteLine("\tDefaults to Release if not specified.");
			
			//PublishArgs.PLATFORM_VALUES
		/*PublishArgs.SELF_CONTAINED
		PublishArgs.SINGLE_FILE
		PublishArgs.TRIM
		PublishArgs.CONFIGURATION*/
		}
    }
}
