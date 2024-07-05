using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ReCap.Hub
{
	public interface IDialogViewModel
	{
		bool IsCloseable
        {
			get;
        }

		void OnClosed();

		//TaskCompletionSource<T> GetCompletionSource<T>();
	}

	public interface IDialogViewModel<T>
		: IDialogViewModel
	{
		TaskCompletionSource<T> CompletionSource
		{
			get;
		}

		/*
		T GetReturnValueForWhenClosed()
			=> default;
		*/
		void IDialogViewModel.OnClosed()
        {
			CompletionSource.SetResult(default);
		}
	}
}