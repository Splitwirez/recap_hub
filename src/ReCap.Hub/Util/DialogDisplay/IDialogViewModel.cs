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
	}

	public interface IDialogViewModel<T> : IDialogViewModel
	{
		TaskCompletionSource<T> GetCompletionSource();
	}
}