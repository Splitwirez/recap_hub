using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub
{
	public enum NMessageBoxResult
	{
		Negative,
		Affirmative,
		Repetitive,
		Neutral
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
		static NMessageBox()
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

		const NMessageBoxButtons DEFAULT_BUTTONS = NMessageBoxButtons.OK;
		const NMessageBoxResult FALLBACK_RESULT = NMessageBoxResult.Affirmative;
		public static NMessageBoxResult Show(IntPtr hWnd, string content, string title, NMessageBoxButtons buttons = DEFAULT_BUTTONS, NMessageBoxResult fallbackResult = FALLBACK_RESULT)
		{
			if (OperatingSystem.IsWindows())
				return _winMap[(WinNMessageBoxResult)WindowsPInvoke.MessageBox(hWnd, content, title, (uint)buttons)];
			else //TODO: implement on non-Windows platforms
				return FALLBACK_RESULT;
		}

		public static NMessageBoxResult DebugShow(IntPtr hWnd, string content, string title, NMessageBoxButtons buttons = DEFAULT_BUTTONS, NMessageBoxResult fallbackResult = FALLBACK_RESULT)
			=> DebugShow(hWnd, content, title, buttons, fallbackResult, fallbackResult);
		public static NMessageBoxResult DebugShow(IntPtr hWnd, string content, string title, NMessageBoxResult noDebugResult, NMessageBoxResult fallbackResult)
			=> DebugShow(hWnd, content, title, DEFAULT_BUTTONS, noDebugResult, fallbackResult);
		public static NMessageBoxResult DebugShow(IntPtr hWnd, string content, string title, NMessageBoxButtons buttons, NMessageBoxResult noDebugResult, NMessageBoxResult fallbackResult)
			=>
#if DEBUG
				true
#else
				CommandLine.Instance.ShowDebugInfo
#endif
				? Show(hWnd, content, title, buttons, fallbackResult)
				: noDebugResult
			;
	}
}
