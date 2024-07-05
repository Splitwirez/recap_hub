using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.IO.Pipes;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;

namespace ReCap.Hub
{
	public enum NMessageBoxResult
	{
		Negative = 0,
		Affirmative = 1,
		Neutral = 2,
		Repetitive = 3,
	}


	public enum NMessageBoxButtons : uint
	{
		//AbortRetryIgnore = 0x00000002,
		CancelTryagainContinue = 0x00000006,
		//Help = 0x00004000,
		OK = 0x00000000,
		OKCancel = 0x00000001,
		RetryCancel = 0x00000005,
		YesNo = 0x00000004,
		YesNoCancel = 0x00000003,
	}

	public enum NMessageBoxPurpose : uint
	{
		Neutral = 0x00000000,
		Info = 0x00000040,
		Warning = 0x00000030,
		Error = 0x00000010,
	}


	public static class NMessageBox
	{
		private enum WinNMessageBoxResult
		{
			Abort = 3,
			Cancel = 2,
			Continue = 11,
			Ignore = 5,
			No = 7,
			OK = 1,
			Retry = 4,
			TryAgain = 10,
			Yes = 6
		}

		static readonly WinNMessageBoxResult[] _winNegative =
		{
			WinNMessageBoxResult.Abort,
			WinNMessageBoxResult.No,
		};
		static readonly WinNMessageBoxResult[] _winAffirmative =
		{
			WinNMessageBoxResult.Continue,
			WinNMessageBoxResult.OK,
			WinNMessageBoxResult.Yes,
		};
		static readonly WinNMessageBoxResult[] _winRepetitive =
		{
			WinNMessageBoxResult.Retry,
			WinNMessageBoxResult.TryAgain,
		};
		static readonly WinNMessageBoxResult[] _winNeutral =
		{
			WinNMessageBoxResult.Ignore,
			WinNMessageBoxResult.Cancel,
		};

		static readonly ReadOnlyDictionary<WinNMessageBoxResult, NMessageBoxResult> _winMap = null;
		
		const string ZENITY_CMD = "zenity";
		static readonly string _zenityPath = $"/usr/bin/{ZENITY_CMD}";
		const string PURPOSE_ARG_PRE="--";
		const string ICON_ARG_PRE="--icon-name=";
		static readonly ReadOnlyDictionary<NMessageBoxPurpose, string> _zenPurposeMap
			= new(new Dictionary<NMessageBoxPurpose, string>()
		{
			{ NMessageBoxPurpose.Neutral, "info" },
			{ NMessageBoxPurpose.Info, "info" },
			{ NMessageBoxPurpose.Warning, "warning" },
			{ NMessageBoxPurpose.Error, "error" },
		});

		static readonly ReadOnlyDictionary<NMessageBoxButtons, string[]> _zenButtonTextMap = new(new Dictionary<NMessageBoxButtons, string[]>()
		{
			/*{
				NMessageBoxButtons.AbortRetryIgnore,
				new string[]
				{
					// ?????
				}
			},*/
			{
				NMessageBoxButtons.CancelTryagainContinue,
				new string[]
				{
					$"{CANCEL_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}",
					$"{OK_TEXT_ARG_PRE}{TryagainBtnText}{TEXT_ARG_POST}",
					$"{THIRD_BTN_TEXT_ARG_PRE}{ContinueBtnText}{TEXT_ARG_POST}",
				}
			},
			/*{
				NMessageBoxButtons.Help,
				new string[]
				{
					// ?????
				}
			},*/
			{
				NMessageBoxButtons.OK,
				new string[0]
			},
			{
				NMessageBoxButtons.OKCancel,
				new string[]
				{
					$"{OK_TEXT_ARG_PRE}{OKBtnText}{TEXT_ARG_POST}",
					$"{CANCEL_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}",
				}
			},
			{
				NMessageBoxButtons.RetryCancel,
				new string[]
				{
					$"{CANCEL_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}",
					$"{OK_TEXT_ARG_PRE}{RetryBtnText}{TEXT_ARG_POST}",
				}
			},
			{
				NMessageBoxButtons.YesNo,
				new string[0]
			},
			{
				NMessageBoxButtons.YesNoCancel,
				new string[]
				{
					$"{THIRD_BTN_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}"
				}
			}
		});


		static readonly ReadOnlyDictionary<NMessageBoxButtons, WinNMessageBoxResult[]> _zenButtonResultsMap = new(new Dictionary<NMessageBoxButtons, WinNMessageBoxResult[]>()
		{
			/*{
				NMessageBoxButtons.AbortRetryIgnore,
				new WinNMessageBoxResult[]
				{
					// ?????
				}
			},*/
			{
				NMessageBoxButtons.CancelTryagainContinue,
				new WinNMessageBoxResult[]
				{
					WinNMessageBoxResult.Cancel,
					WinNMessageBoxResult.TryAgain,
					WinNMessageBoxResult.Continue,
				}
			},
			/*{
				NMessageBoxButtons.Help,
				new WinNMessageBoxResult[]
				{
					// ?????
				}
			},*/
			{
				NMessageBoxButtons.OK,
				new WinNMessageBoxResult[]
				{
					WinNMessageBoxResult.OK,
				}
			},
			{
				NMessageBoxButtons.OKCancel,
				new WinNMessageBoxResult[]
				{
					WinNMessageBoxResult.OK,
					WinNMessageBoxResult.Cancel,
				}
			},
			{
				NMessageBoxButtons.RetryCancel,
				new WinNMessageBoxResult[]
				{
					WinNMessageBoxResult.Retry,
					WinNMessageBoxResult.Cancel,
				}
			},
			{
				NMessageBoxButtons.YesNo,
				new WinNMessageBoxResult[]
				{
					WinNMessageBoxResult.Yes,
					WinNMessageBoxResult.No,
				}
			},
			{
				NMessageBoxButtons.YesNoCancel,
				new WinNMessageBoxResult[]
				{
					WinNMessageBoxResult.Yes,
					WinNMessageBoxResult.No,
					WinNMessageBoxResult.Cancel,
				}
			}
		});
		

		/*static readonly ReadOnlyDictionary<NMessageBoxPurpose, string> _zenPurposeMapMultipleButtons
			= new ReadOnlyDictionary<NMessageBoxPurpose, string>(new Dictionary<NMessageBoxPurpose, string>()
		{
			//https://unix.stackexchange.com/questions/486768/how-can-i-change-zenity-dialog-icon
			{ NMessageBoxPurpose.Neutral, "--info" },
			{ NMessageBoxPurpose.Info, "--info" },
			{ NMessageBoxPurpose.Warning, "--warning" },
			{ NMessageBoxPurpose.Error, "--error" },
		});*/


		static NMessageBox()
		{
			if (OperatingSystem.IsWindows() || OperatingSystem.IsLinux())
			{
				var winMap = new Dictionary<WinNMessageBoxResult, NMessageBoxResult>();


				foreach (WinNMessageBoxResult r in _winNegative)
					winMap.Add(r, NMessageBoxResult.Negative);

				foreach (WinNMessageBoxResult r in _winAffirmative)
					winMap.Add(r, NMessageBoxResult.Affirmative);

				foreach (WinNMessageBoxResult r in _winRepetitive)
					winMap.Add(r, NMessageBoxResult.Repetitive);

				foreach (WinNMessageBoxResult r in _winNeutral)
					winMap.Add(r, NMessageBoxResult.Neutral);


				_winMap = new ReadOnlyDictionary<WinNMessageBoxResult, NMessageBoxResult>(winMap);
			}

			if (OperatingSystem.IsLinux())
			{
				//_zenityPath
				//new Process("which", ZENITY_CMD)
			}
		}

		static string CancelBtnText
		{
			get => "Cancel";
		}

		static string OKBtnText
		{
			get => "OK";
		}

		static string TryagainBtnText
		{
			get => "Try again";
		}

		static string RetryBtnText
		{
			get => "Retry";
		}

		static string ContinueBtnText
		{
			get => "Continue";
		}
			 
		const string OK_TEXT_ARG_PRE = "--ok-label=\"";
		const string CANCEL_TEXT_ARG_PRE = "--cancel-label=\"";
		const string THIRD_BTN_TEXT_ARG_PRE = "--extra-button=\"";
		const string TEXT_ARG_POST = "\"";
		static string CreateZenityArgs(string content, string title, NMessageBoxButtons buttons, NMessageBoxPurpose purpose)
		{
			bool singleButton = buttons == NMessageBoxButtons.OK;
			List<string> args = new List<string>();
			
			string purposeArg = _zenPurposeMap[purpose];
			if (singleButton)
			{
				//args.Add(_zenPurposeMapSingleButton[purpose]);
				purposeArg = $"{PURPOSE_ARG_PRE}{purposeArg}";
			}
			else
			{
				purposeArg = $"{ICON_ARG_PRE}{purposeArg}";
				args.Add("--question");
			}
			args.Add(purposeArg);
			args.Add($"--text=\"{content}\"");
			args.Add($"--title=\"{title}\"");

			if ((!singleButton) && (buttons != NMessageBoxButtons.YesNo))
			{
				/*if (buttons == NMessageBoxButtons.RetryCancel)
				{
					args.Add($"{CANCEL_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}");
					args.Add($"{OK_TEXT_ARG_PRE}{RetryBtnText}{TEXT_ARG_POST}");
				}
				else if (buttons == NMessageBoxButtons.CancelTryagainContinue)
				{
					args.Add($"{CANCEL_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}");
					args.Add($"{OK_TEXT_ARG_PRE}{TryagainBtnText}{TEXT_ARG_POST}");
					args.Add($"{THIRD_BTN_TEXT_ARG_PRE}{ContinueBtnText}{TEXT_ARG_POST}");
				}
				else if (buttons == NMessageBoxButtons.OKCancel)
				{
					args.Add($"{OK_TEXT_ARG_PRE}{OKBtnText}{TEXT_ARG_POST}");
					args.Add($"{CANCEL_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}");
				}
				else if (buttons == NMessageBoxButtons.YesNoCancel)
				{
					args.Add($"{THIRD_BTN_TEXT_ARG_PRE}{CancelBtnText}{TEXT_ARG_POST}");
				}*/
				var buttonArgs = _zenButtonTextMap[buttons];
				foreach (var btnArg in buttonArgs)
				{
					args.Add(btnArg);
				}
			}
			return CommandLine.PrepareArgsForCLI(args, false);
		}

		const NMessageBoxButtons DEFAULT_BUTTONS = NMessageBoxButtons.OK;
		const NMessageBoxPurpose DEFAULT_PURPOSE = NMessageBoxPurpose.Neutral;
		const NMessageBoxResult FALLBACK_RESULT = NMessageBoxResult.Affirmative;
		public static NMessageBoxResult Show(IntPtr hWnd, string content, string title, NMessageBoxButtons buttons = DEFAULT_BUTTONS, NMessageBoxPurpose purpose = DEFAULT_PURPOSE, NMessageBoxResult fallbackResult = FALLBACK_RESULT)
		{
			if (OperatingSystem.IsWindows())
				return _winMap[(WinNMessageBoxResult)WindowsPInvoke.MessageBox(hWnd, content, title, (uint)buttons | (uint)purpose)];
#if ZENITY
			else if (OperatingSystem.IsLinux())
			{
				//https://hackaday.io/project/184015-using-zenity-for-gui-command-line-programs-in-c
				string args = CreateZenityArgs(content, title, buttons, purpose);
				string command = $"{_zenityPath} {args}";
				IntPtr fp = LinuxPInvoke.POpen(command, "r");
				if (fp == IntPtr.Zero)
				{
					//something went wrong
				}
				else
				{
					//https://www.tutorialspoint.com/c_standard_library/c_function_fgets.htm
					//while (LinuxPInvoke.FGets(out string str, 256, fp) > 0)
					string output = string.Empty;
					int exitCode;
					unsafe
					{
        				//byte* 
						var memBytePtr = (byte*)fp.ToPointer();
						
						string str = string.Empty;
						while (str != null)
						{
							using (var stream = new UnmanagedMemoryStream(memBytePtr, args.Length)) // new AnonymousPipeClientStream(PipeDirection.Out, new SafePipeHandle(fp, true)))
							{
								using (var reader = new StreamReader(stream))
								{
									output += str;
									str = reader.ReadLine();
								}
							}
						}
						var closed = LinuxPInvoke.PClose(fp);
						exitCode = closed; //LinuxPInvoke.WEXITSTATUS();
					}
					Debug.WriteLine($"output: \"{output}\"");
					Debug.WriteLine($"exitCode: \"{exitCode}\"");
				}
				return fallbackResult;
			}
			else if (false)
			{
				var zenity = new Process();
				zenity.StartInfo.FileName = _zenityPath;
				string zenityArgs = CreateZenityArgs(content, title, buttons, purpose);
				zenity.StartInfo.Arguments = zenityArgs;
				Debug.WriteLine($"ZENITY ARGS:\n{zenityArgs}");
				zenity.StartInfo.UseShellExecute = false;
				zenity.StartInfo.RedirectStandardOutput = true;
				zenity.StartInfo.RedirectStandardError = true;
				zenity.StartInfo.RedirectStandardInput = true;
				
				
				/*string outputData = "\n\n\n\n\n\n\n\n\n\n";
				zenity.OutputDataReceived += (s, e) =>
				{
					outputData += "\n" + e.Data + "\n";
				};
				zenity.ErrorDataReceived += (s, e) =>
				{
					outputData += "\n<ERROR>\n\t" + e.Data + "\n</ERROR>\n";
				};*/
				zenity.Start();
				//outputData += "\n\n\n\n\n\n\n\n\n\n<FINAL>\n\t" + zenity.StandardOutput.ReadToEnd() + "\n</FINAL>\n\n\n\n\n\n\n\n\n\n";
				string outputData = zenity.StandardOutput.ReadToEnd();
				zenity.WaitForExit();
				int exitCode = zenity.ExitCode;
				Debug.WriteLine("==== ZENITY ====\n"
					+ $"\tExitCode: \"{exitCode}\"\n"
					+ $"\toutputData: \"{outputData}\"");
				return fallbackResult; //TODO: map return values
			}
#endif
			else //TODO: implement on non-Windows platforms
			{
				string titleLine = $"======== {title} ========";
				string output = titleLine + "\n";
				int lineMax = titleLine.Length - 10;
				string lineStart = "|    ";
				/*if (lineMax > 16)
				{
					lineMax -= 8;
					lineStart = "    ";
				}*/
				string lineEnd = "    |\n";

				int lineIndex = 0;

				output += lineStart;
				for (int i = 0; i < lineMax; i++)
				{
					output += " ";
				}
				output += lineEnd + lineStart;
				for (int i = 0; i < content.Length; i++)
				{
					string append = content[i].ToString();
					if (append.Contains('\r') || append.Contains('\n')) // || append.Contains('\r\n'))
					{
						lineIndex = 0;
					}
					else if (lineIndex >= lineMax)
					{
						output += lineEnd + lineStart;
						lineIndex = 0;
					}

					output += append;
					// ) || append.Contains(
					
					lineIndex++;
				}
				for (int i = lineIndex; i < lineMax; i++)
				{
					output += " ";
				}
				output += lineEnd + lineStart;
				for (int i = 0; i < lineMax; i++)
				{
					output += " ";
				}
				output += lineEnd;
				for (int i = 0; i < titleLine.Length; i++)
				{
					output += "=";
				}
				Console.WriteLine(output);
				return fallbackResult;
			}
		}

		public static NMessageBoxResult DebugShow(IntPtr hWnd, string content, string title, NMessageBoxButtons buttons = DEFAULT_BUTTONS, NMessageBoxPurpose purpose = DEFAULT_PURPOSE, NMessageBoxResult fallbackResult = FALLBACK_RESULT)
			=> DebugShow(hWnd, content, title, buttons, purpose, fallbackResult, fallbackResult);
		public static NMessageBoxResult DebugShow(IntPtr hWnd, string content, string title, NMessageBoxResult noDebugResult, NMessageBoxResult fallbackResult)
			=> DebugShow(hWnd, content, title, DEFAULT_BUTTONS, DEFAULT_PURPOSE, noDebugResult, fallbackResult);
		public static NMessageBoxResult DebugShow(IntPtr hWnd, string content, string title, NMessageBoxButtons buttons, NMessageBoxPurpose purpose, NMessageBoxResult noDebugResult, NMessageBoxResult fallbackResult)
			=>
#if DEBUG
				true
#else
				CommandLine.Instance.ShowDebugInfo
#endif
				? Show(hWnd, content, title, buttons, purpose, fallbackResult)
				: noDebugResult
			;

        /*public class ZenityPipeStream : PipeStream
        {
            public ZenityPipeStream(PipeDirection direction, int bufferSize)
				: base(direction, bufferSize)
            {
            }

            public ZenityPipeStream(PipeDirection direction, PipeTransmissionMode transmissionMode, int outBufferSize)
				: base(direction, transmissionMode, outBufferSize)
            {
            }

			override Initia
        }*/
    }
}
