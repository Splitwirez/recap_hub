using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReCap.Builder
{
	public class Program
    {
		//public static readonly char S = PublishArgs.DIR_SEPARATOR;


        static void Main(string[] args)
        {
			/*var helpArg = args.FirstOrDefault(arg => PublishArgs.GET_HELP.Any(help => arg == help));
			if (helpArg != null)*/
			if (args.Any(arg => PublishArgs.GET_HELP.Any(help => arg == help)))
			{
				PrintHelp();
				Environment.Exit(2);
			}

			PublishParams param = new PublishParams();
			
			try
			{
				int argCount = args.Length;
				int extraOptionsStart = -1;
				//string extraOptionsArg = args.FirstOrDefault(x => )
				//if ()
				for (int i = 0; i < argCount; i++)
				{
					int remainingArgs = argCount - (i + 1);
					string arg = args[i];
					if (PublishArgs.UnEscape(arg) == PublishArgs.EXTRA_OPTIONS)
					{
						extraOptionsStart = i;
						break;
					}
					else if (remainingArgs > 0)
					{
						string nextArg = PublishArgs.UnEscape(args[i + 1]);
						if (string.IsNullOrEmpty(nextArg) || string.IsNullOrWhiteSpace(nextArg))
							continue;
						
						if (PublishArgs.IsArg(arg, PublishArgs.PUB_TARGET))
						{
							if (Enum.TryParse<PublishTarget>(nextArg, out PublishTarget target))
							{
								param.Target = target;
							}
							else
							{
								
								throw new PublishArgsException(arg, nextArg, $"Invalid publish target. Valid options are as follows:{PublishArgs.PLATFORM_VALUES}.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.SELF_CONTAINED))
						{
							if (!bool.TryParse(nextArg, out param.SelfContained))
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid boolean.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.TRIM))
						{
							if (!bool.TryParse(nextArg, out param.EnableTrimming))
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid boolean.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.SINGLE_FILE))
						{
							if (!bool.TryParse(nextArg, out param.SingleFile))
							{
								throw new PublishArgsException(arg, nextArg, $"Invalid boolean.");
							}
						}
						else if (PublishArgs.IsArg(arg, PublishArgs.CONFIGURATION))
						{
							param.Configuration = nextArg;
						}
						i++;
					}
				}
				if (extraOptionsStart >= 0)
				{
					string dotnetExtraArgs = null;
					var extraArgs = args.Skip(extraOptionsStart).ToArray();
					if (extraArgs.Length > 0)
					{
						var first = extraArgs.First();
						if (PublishArgs.UnEscape(first) != PublishArgs.EXTRA_OPTIONS)
						{
							dotnetExtraArgs = first;
							for (int i = 1; i < extraArgs.Length; i++)
							{
								string extraArg = extraArgs[i];
								dotnetExtraArgs += $" {extraArg}";
								/*if (extraArg == PublishArgs.EXTRA_OPTIONS)
								{
									extraArgs = extraArgs.Skip(i).ToArray();
									break;
								}
								else
								{
									dotnetExtraArgs
								}*/
							}
						}
					}

					if (dotnetExtraArgs != null)
						param.ExtraArgs = dotnetExtraArgs;
				}

				PublishHub(param);
			}
			catch (PublishArgsException ex)
			{
				Console.WriteLine(ex.Message);
				Environment.Exit(1);
			}
        }
        
        public static void PublishHub(PublishParams param)
        {
			string repoDir = Environment.CurrentDirectory;
			string hubProjDir = Path.Combine(repoDir, "src", "ReCap.Hub");

			var targetPlatform = param.Target;
			if (targetPlatform == null) //TODO: Implement publishing for macOS
			{
				if (OperatingSystem.IsLinux())
					targetPlatform = PublishTarget.Linux;
				else if (OperatingSystem.IsWindows())
					targetPlatform = PublishTarget.Windows;
				/*else if (OperatingSystem.IsMacOS())
					targetPlatform = PublishTarget.MacOS;*/
				else
				{
					Console.WriteLine("Specify a target platform. Pass --help for more information.");
					Environment.Exit(1);
				}
			}
			/*if (targetPlatform == PublishTarget.MacOS)
			{
				Console.WriteLine(new PublishArgsException(PublishArgs.PUB_TARGET, targetPlatform.ToString(), "Publishing for macOS is not yet implemented.").Message);
				if (GetBoolReply("Publish for Windows instead?"))
					targetPlatform = PublishTarget.Windows;
				else
					Environment.Exit(1);
			}*/

			bool publishForWindowsOnNotWindows = (targetPlatform == PublishTarget.Windows) && !OperatingSystem.IsWindows();
			string targetPlatformName = targetPlatform.ToString();
			
			
			string projectPathArg = $"\"{hubProjDir}\"";
			string publishArgs = projectPathArg;
			publishArgs += PublishArgs.GetPropArg("PublishTarget", targetPlatformName);
			
			string configuration = param.Configuration;
			if (configuration == null)
				configuration = "Release";
			
			publishArgs += $" --configuration {configuration}";
			

			if (param.SelfContained)
			{
				publishArgs += $" --self-contained {param.SelfContained}";
				if (param.EnableTrimming)
				{
					publishArgs +=
						PublishArgs.GetPropArg("PublishTrimmed", true)
						+ PublishArgs.GetPropArg("TrimMode", "link")
						+ PublishArgs.GetPropArg("EnableUnsafeBinaryFormatterSerialization", false)
						+ PublishArgs.GetPropArg("EnableUnsafeUTF7Encoding", false)
						+ PublishArgs.GetPropArg("MetadataUpdaterSupport", false)
					;
					
					if (configuration.Contains("debug", StringComparison.OrdinalIgnoreCase))
					{
						publishArgs +=
							PublishArgs.GetPropArg("TrimmerRemoveSymbols", true)
							+ PublishArgs.GetPropArg("DebuggerSupport", false)
						;
					}
				}
			}
			
			
			if (param.SingleFile)
				publishArgs += PublishArgs.GetPropArg("PublishSingleFile", true);
			
			
			string buildOutputDir = Path.Combine(repoDir, "publish", "demo", targetPlatformName);
			publishArgs += $" -o \"{buildOutputDir}\"";
			string noExtensionExePath = null;
			string exePath = null;
			if (publishForWindowsOnNotWindows)
			{
				noExtensionExePath = Path.Combine(buildOutputDir, "ResurrectionCapsule.Hub");
				exePath = noExtensionExePath + ".exe";
				/*if (File.Exists(exePath))
					File.Delete(exePath);*/
			}

			//Console.WriteLine($"\n\n\n\n==== BEGIN PUBLISH ARGS ====\n\n{publishArgs}\n\n==== END PUBLISH ARGS ====\n\n\n\n");


			/*Process dotnetClean = new Process()
			{
				StartInfo = new ProcessStartInfo("dotnet", "clean " + projectPathArg)
			};
			dotnetClean.Start();
			dotnetClean.WaitForExit();
			if (dotnetClean.ExitCode != 0)
			{
				Console.WriteLine("dotnet clean failed");
				Environment.Exit(dotnetClean.ExitCode);
			}*/

			if (Directory.Exists(buildOutputDir))
				Directory.Delete(buildOutputDir, true);
			Directory.CreateDirectory(buildOutputDir);
			
			/*Process dotnetBuild = new Process()
			{
				StartInfo = new ProcessStartInfo("dotnet", "build " + publishArgs + " --no-incremental")
			};
			dotnetBuild.Start();
			dotnetBuild.WaitForExit();
			if (dotnetBuild.ExitCode != 0)
				Environment.Exit(dotnetBuild.ExitCode);*/
			
			Process dotnetPublish = new Process()
			{
				StartInfo = new ProcessStartInfo("dotnet", "publish " + publishArgs) // + " --no-build")
			};
			dotnetPublish.Start();
			dotnetPublish.WaitForExit();
			if (publishForWindowsOnNotWindows)
			{
				/*if (File.Exists(noExtensionExePath) && !File.Exists(exePath))
				{*/
				try
				{
					File.Move(noExtensionExePath, exePath);
				}
				catch (Exception ex)
				{
					Console.WriteLine("Could not add '.exe' extension to executable name: " + ex.ToString());
				}
				//}
			}
			if (dotnetPublish.ExitCode != 0)
				Console.WriteLine("dotnet publish failed");
			
			Environment.Exit(dotnetPublish.ExitCode);
		}

		/*static bool GetBoolReply(string question)
		{
			bool? answer = null;
			//string response = string.Empty;
			while (answer == null) //(string.IsNullOrEmpty(response) || string.IsNullOrWhiteSpace(response))
			{
				Console.WriteLine(question);
				string response = Console.ReadLine();
				if (string.IsNullOrEmpty(response) || string.IsNullOrWhiteSpace(response))
				{

				}
				else if (bool.TryParse(response, out bool ans))
				{
					answer = ans;
				}
				else if
				(
					response.Equals("yes", StringComparison.OrdinalIgnoreCase)
					|| (response.Equals("y", StringComparison.OrdinalIgnoreCase))
				)
				{
					answer = true;
				}
				else if
				(
					response.Equals("no", StringComparison.OrdinalIgnoreCase)
					|| (response.Equals("n", StringComparison.OrdinalIgnoreCase))
				)
				{
					answer = false;
				}
				else
				{
					Console.WriteLine("Invalid response.");
				}
			}
			return answer.GetValueOrDefault();
		}*/

		static void PrintHelp()
		{
			string boolValues = " <true|false>";
			Console.WriteLine("==== DOCUMENTATION FOR RESURRECTION CAPSULE PUBLISH ASSISTANT ====");
			Console.WriteLine("\n");

			bool isWindows = OperatingSystem.IsWindows();
			string scriptFileName = "publish";
			scriptFileName += isWindows
				? ".bat"
				: ".sh"
			;

			if (!isWindows)
				scriptFileName = $"./{scriptFileName}";
			

			Console.WriteLine($"\tCommand usage:\n\t{scriptFileName} [OPTIONS, IF DESIRED]");
			Console.WriteLine("\n");
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
			Console.WriteLine(PublishArgs.SELF_CONTAINED + boolValues);
			Console.WriteLine("\tIf true, packages the .NET runtime in with the Resurrection Capsule Hub, avoiding the need for it to be installed separately. (larger file size)");
			Console.WriteLine("\tIf false, requires a compatible .NET runtime to be installed separately. (smaller file size)");
			Console.WriteLine("\tDefaults to true if not specified.");
			
			
			Console.WriteLine("\n");
			Console.WriteLine(PublishArgs.SINGLE_FILE + boolValues);
			Console.WriteLine("\tIf true, smooshes the Resurrection Capsule Hub down to as few files as possible (one file, ideally).");
			Console.WriteLine("\tIf false, leaves all files separated from one another.");
			Console.WriteLine("\tDefaults to false if not specified.");
			
			
			Console.WriteLine("\n");
			Console.WriteLine(PublishArgs.TRIM + boolValues);
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
