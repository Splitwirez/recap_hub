using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace ReCap.Builder
{
	public static class PublishArgs
	{
		public static readonly IReadOnlyList<string> GET_HELP
			= new List<string>()
		{
			"--help",
			"-h",
			"/?"
		}.AsReadOnly();
		
		public const string EXTRA_OPTIONS = "--";
		public const string PUB_TARGET = "--platform";
		public const string SELF_CONTAINED = "--self-contained";
		public const string SINGLE_FILE = "--single-file";
		public const string TRIM = "--trim";
		public const string CONFIGURATION = "--configuration";
		
		public static readonly IReadOnlyList<PublishTarget> VALID_PUB_TARGETS
			= new List<PublishTarget>()
			{
				PublishTarget.Windows
				, PublishTarget.Linux
				//, PublishTarget.MacOS
			}.AsReadOnly();
		public static readonly Dictionary<string, char> ARG_ABBREVIATIONS = new Dictionary<string, char>()
		{
			{ PUB_TARGET, 'p' }
			,{ CONFIGURATION, 'c' }
		};

		public static readonly string PLATFORM_VALUES = new Func<string>(() =>
		{
			string validArgs = string.Empty;
			foreach (var opt in PublishArgs.VALID_PUB_TARGETS)
			{
				validArgs += $" '{opt.ToString()}',";
			}
			return validArgs.TrimEnd(',');
		})();
		
		public static bool IsArg(string str, string isItThisArg)
		{
			string unescaped = UnEscape(str);
			if (unescaped == isItThisArg)
				return true;
			else if (ARG_ABBREVIATIONS.TryGetValue(isItThisArg, out char abbrev) && unescaped == $"-{abbrev}")
				return true;
			else
				return false;
		}
		
		
		public static string UnEscape(string raw)
		{
			return raw.Trim('"', '\'');
		}

		public static string GetPropArg(string name, object value)
		{
			string val = value.ToString();
			if (val.Contains(" ") || val.Contains(@"\"))
				val = $"\"{val}\"";
			return $" -p:{name}={val}";
		}
	}


    public class PublishArgsException : Exception
	{
		public PublishArgsException(string opt, string message)
			: base($"Invalid option '{opt}'.\n{message}")
		{
			
		}
		public PublishArgsException(string opt, string optParams, string message)
			: base($"Invalid input for option '{opt}': '{optParams}'.\n{message}")
		{
			
		}
	}
}
