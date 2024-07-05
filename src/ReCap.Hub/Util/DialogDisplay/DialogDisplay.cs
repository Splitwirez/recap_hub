using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
//using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub
{
	public static partial class DialogDisplay
	{
		static List<DialogShownEventArgs> _dialogs = new List<DialogShownEventArgs>();

		public static async Task<T> ShowDialog<T>(IDialogViewModel<T> vm)
		{
			Console.WriteLine("DIALOG OF TYPE \'" + vm.GetType().FullName + "\' SHOWN: " + vm.ToString());
			//int count = 0;//_dialogs.Count;
			var task = vm.CompletionSource.Task;
			AddToQueue(new DialogShownEventArgs(vm, task));
			return await task;
		}




		static void AddToQueue(DialogShownEventArgs args)
		{
			_dialogs.Add(args);
			StartRollingDialogs();
		}

		static bool _rolling = false;
		static async void StartRollingDialogs()
		{
			if (!_rolling)
			{
				_rolling = true;
				while (_dialogs.Count > 0)
				{
					var args = _dialogs[0];

					_dialogShown?.Invoke(null, args);
					await args.Task;
					_dialogs.Remove(args);
					_dialogShown?.Invoke(null, null);
				}
				_rolling = false;
			}
		}


		static int _totalDialogShownHandlers = 0;
		internal static int TotalDialogShownHandlers
		{
			get => _totalDialogShownHandlers;
			set => _totalDialogShownHandlers = Math.Max(0, value);
		}


		static event EventHandler<DialogShownEventArgs> _dialogShown;
		public static event EventHandler<DialogShownEventArgs> DialogShown
		{
			add
			{
				_dialogShown += value;
				if (_rolling && (_dialogs.Count > 0))
					_dialogShown?.Invoke(null, _dialogs[0]);

				TotalDialogShownHandlers++;
			}
			remove
			{
				_dialogShown -= value;
				if (TotalDialogShownHandlers > 0)
					TotalDialogShownHandlers--;
			}
		}
	}

	public class DialogShownEventArgs
	{
		Task _task = null;
		public Task Task
			=> _task;

		IDialogViewModel _vm = null;
		public IDialogViewModel ViewModel
			=> _vm;

		public DialogShownEventArgs(IDialogViewModel vm, Task task)
		{
			_vm = vm;
			_task = task;
		}

		private DialogShownEventArgs()
		{ }
	}


}