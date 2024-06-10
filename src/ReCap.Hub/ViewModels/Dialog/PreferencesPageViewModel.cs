using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReCap.Hub.Data;

namespace ReCap.Hub.ViewModels
{
    public class PreferencesPageViewModel
        : ViewModelBase
        , IDialogViewModel<object>
    {
        bool GetUseManagedDecorations()
            => HubData.Instance.UseManagedDecorations;
        public bool UseManagedDecorations
        {
            get => GetUseManagedDecorations();
            set
            {
                if (ForwardRASIC(GetUseManagedDecorations, value))
                    HubData.Instance.UseManagedDecorations = value;
            }
        }


        string GetUserDisplayName()
            => HubData.Instance.UserDisplayName;
        public string UserDisplayName
        {
            get => GetUserDisplayName();
            set
            {
                if (ForwardRASIC(GetUserDisplayName, value))
                    HubData.Instance.UserDisplayName = value;
            }
        }


        bool GetAutoCloseServer()
            => HubData.Instance.AutoCloseServer;
        public bool AutoCloseServer
        {
            get => GetAutoCloseServer();
            set
            {
                if (ForwardRASIC(GetAutoCloseServer, value))
                    HubData.Instance.AutoCloseServer = value;
            }
        }




        bool ForwardRASIC<T>(Func<T> get, T value)
        {
            T oldValue = get();
            T newValue = value;

            T backingField = oldValue;
            RASIC(ref backingField, newValue);
            if (oldValue == null)
                return newValue == null;
            else
                return !oldValue.Equals(newValue);
        }




        public bool IsCloseable
        {
            get => true;
        }

        readonly TaskCompletionSource<object> _tcs = new TaskCompletionSource<object>();
        public TaskCompletionSource<object> CompletionSource
        {
            get => _tcs;
        }


        public void CloseCommand(object _)
            => CompletionSource.TrySetResult(null);
    }
}
