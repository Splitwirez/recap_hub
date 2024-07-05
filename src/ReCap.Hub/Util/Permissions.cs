using System;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using Microsoft.Win32;

namespace ReCap.Hub
{
	public static partial class Permissions
	{
		static WindowsIdentity _identityCached = null;
		static WindowsIdentity _identity
		{
			get
			{
				if (OperatingSystem.IsWindows())
				{
					if (_identityCached == null)
						_identityCached = WindowsIdentity.GetCurrent();
				}

				return _identityCached;
			}
		}

		static WindowsPrincipal _principalCached = null;
		static WindowsPrincipal _principal
		{
			get
			{
				if (OperatingSystem.IsWindows())
				{
					if (_principalCached == null)
						_principalCached = new WindowsPrincipal(_identity);
				}
				return _principalCached;
			}
		}

		public static bool IsAdministrator()
		{
			if (OperatingSystem.IsWindows())
				return _principal.IsInRole(WindowsBuiltInRole.Administrator);
			else
				return false; //TODO: do we even care if the user runs the hub as root?
		}

		//https://stackoverflow.com/questions/1220213/detect-if-running-as-administrator-with-or-without-elevated-privileges
		//https://github.com/falahati/UACHelper
		/*public static bool IsExplicitlyElevated()
			=> IsAdministrator() && _identity.Owner.IsWellKnown(WellKnownSidType.BuiltinAdministratorsSid);*/

		private const string REGISTRY_ADDRESS = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System";

		public static bool IsUACEnabled
		{
			get
			{
				if (OperatingSystem.IsWindows())
				{
					using (var baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
					using (var key = baseKey.OpenSubKey(REGISTRY_ADDRESS, false))
					{
						return (key?.GetValue("EnableLUA", 0) as int? ?? 0) > 0;
					}
				}
				else
					return false;
			}
		}

		/*public static string GetProcessCommandLineArgs()
		{
			var args = Environment.GetCommandLineArgs().ToList();
			if (args.Count > 0)
				args.RemoveAt(0);

			string returnVal = string.Empty;
			foreach (string s in args)
			{
				if (s.Contains(' ') && (!s.StartsWith('"')) && (!s.EndsWith('"')))
					returnVal = returnVal + "\"" + s + "\" ";
				else
					returnVal = returnVal + s + " ";
			}

			return returnVal;
		}*/

		public static Process RerunAsAdministrator(string args)
		{
			//https://stackoverflow.com/questions/133379/elevating-process-privilege-programmatically/10905713
			var exeName = Process.GetCurrentProcess().MainModule.FileName;
			Process process = null;
			ProcessStartInfo startInfo = new ProcessStartInfo(exeName, args)
			{
				UseShellExecute = true,
				Verb = "runas"
			};
			/*if (args != null)
				startInfo.Arguments = args;*/

			process = Process.Start(startInfo);

			return process;
		}
	}
}