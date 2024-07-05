using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace ReCap.Builder
{
	public enum ExitReason : int
	{
		Cancelled = -1,
		Success = 0,
		Failure = 1,
		StillWorkingOnThisCodePath = 9001
	}


	public static class Program
    {
		//public static readonly char S = PublishArgs.DIR_SEPARATOR;
		public static void Exit(ExitReason reason)
			=> Environment.Exit((int)reason);

        static void Main(string[] args)
        {
			/*var helpArg = args.FirstOrDefault(arg => PublishArgs.GET_HELP.Any(help => arg == help));
			if (helpArg != null)*/
			int argCount = args.Length;
			if (argCount == 0)
			{
				Console.WriteLine("In future, do");
				CLIHelp.PrintTopLevelHelp();

				if (GetBoolReply("Produce a build for this machine now?"))
				{
					PublishForMe();
				}
				else
				{
					Exit(ExitReason.Cancelled);
				}
			}
			else if (NeedsHelp(args))
			{
				
			}
			else if (PublishFromMode(args))
			{
				
			}
			else if 
			(
				(argCount == 2)
			 	&& PublishArgs.UnEscape(args[0]).Equals("for", StringComparison.OrdinalIgnoreCase)
				&& PublishArgs.UnEscape(args[1]).Equals("me", StringComparison.OrdinalIgnoreCase)
			)
			{
				PublishForMe();
			}
			else if (PublishParams.TryGetFromCLI(args, out PublishParams param, out Exception exception))
			{
				PublishHub(param);
			}
			else if (exception is PublishArgsException ex)
			{
				Console.WriteLine(ex.Message);
				Exit(ExitReason.Failure);
			}
			
			Exit(ExitReason.StillWorkingOnThisCodePath);
        }

		public static bool PublishFromMode(string[] args)
		{
			int argCount = args.Length;
			if (argCount < 2)
				return false;
			else if (!args.FirstOrDefault().Equals(PublishArgs.PUB_MODE))
				return false;
			
			
			string mode = args[1];

			if (string.IsNullOrEmpty(mode) || string.IsNullOrWhiteSpace(mode))
				return false;

			string[] extraArgs = new string[0];

			PublishTarget? target = null;
			int extraArgsStart = 2;
			if (argCount > 2)
			{
				string thirdArg = args[3];
				if (thirdArg == PublishArgs.PUB_PLATFORM)
				{
					if (Enum.TryParse<PublishTarget>(args[4], out PublishTarget publishTarget))
					{
						target = publishTarget;
					}
					extraArgsStart = 4;
				}
				else if (thirdArg != PublishArgs.EXTRA_OPTIONS)
				{
					return false;
				}
			}

			if (argCount > extraArgsStart)
				extraArgs = args.Skip(extraArgsStart).ToArray();


			Console.WriteLine($"would publish as {mode} for ");
			Exit(ExitReason.StillWorkingOnThisCodePath);
			//return false;
			if (PublishParams.TryGetParamsForMode(mode, target, out PublishParams param))
			{
				PublishHub(param);
				return true;
			}
			else
				return false;
		}

		static void PublishForMe()
		{
			if (PublishParams.TryGetParamsForMode("DEV__DEMO", null, out PublishParams forMeParam))
			{
				PublishHub(forMeParam);
			}
			else
				Exit(ExitReason.Failure);
		}

		public static bool NeedsHelp(string[] args)
		{
			int argCount = args.Length;
			
			if (!((argCount > 0) && (argCount <= 2) && args.Any(arg => PublishArgs.GET_HELP.Any(help => arg == help))))
				return false;


			bool helpRequestUnderstood = false;
				
			if (argCount == 1)
			{
				helpRequestUnderstood = true;
				CLIHelp.PrintHelp(true, true);
			}
			else if (argCount == 2)
			{
				string helpType = args.LastOrDefault();
				
				if (string.IsNullOrEmpty(helpType) || string.IsNullOrWhiteSpace(helpType))
				{

				}
				else if (helpType == PublishArgs.HELP_TYPE_MODES)
				{
					helpRequestUnderstood = true;
					CLIHelp.PrintHelp
					(
						showModes: true,
						showOptions: false
					);
				}
				else if (helpType == PublishArgs.HELP_TYPE_OPTIONS)
				{
					helpRequestUnderstood = true;
					CLIHelp.PrintHelp
					(
						showModes: false,
						showOptions: true
					);
				}
			}

			if (helpRequestUnderstood)
			{
				Exit(ExitReason.Success);
				return true;
			}
			else
			{
				Console.WriteLine("Help request not understood");
				Exit(ExitReason.Failure);
				return false;
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
					Exit(ExitReason.Failure);
				}
			}
			/*if (targetPlatform == PublishTarget.MacOS)
			{
				Console.WriteLine(new PublishArgsException(PublishArgs.PUB_TARGET, targetPlatform.ToString(), "Publishing for macOS is not yet implemented.").Message);
				if (GetBoolReply("Publish for Windows instead?"))
					targetPlatform = PublishTarget.Windows;
				else
					Exit(ExitReason.Failure);
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

		static bool GetBoolReply(string question)
		{
			bool? answer = null;
			//string response = string.Empty;
			while (!answer.HasValue) //(string.IsNullOrEmpty(response) || string.IsNullOrWhiteSpace(response))
			{
				Console.WriteLine(question);
				string response = Console.ReadLine();

				if (string.IsNullOrEmpty(response) || string.IsNullOrWhiteSpace(response))
				{
				}
				else if (bool.TryParse(response, out bool ans))
				{
					answer = ans;
					continue;
				}
				else if
				(
					response.Equals("yes", StringComparison.OrdinalIgnoreCase)
					|| (response.Equals("y", StringComparison.OrdinalIgnoreCase))
				)
				{
					answer = true;
					continue;
				}
				else if
				(
					response.Equals("no", StringComparison.OrdinalIgnoreCase)
					|| (response.Equals("n", StringComparison.OrdinalIgnoreCase))
				)
				{
					answer = false;
					continue;
				}
				
				Console.WriteLine($"\n'{response}' is not a valid response - this is a yes or no question, so answer 'yes' or 'no'.\n");
			}
			return answer.GetValueOrDefault();
		}
    }
}
