using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;
using ReCap.Hub;

namespace ReCap.Hub.ViewModels
{
    public class ViewModelBase : RxObjectBase
    {
        public override Type GetViewType()
            => Type.GetType(GetType().FullName.Replace("ViewModel", "View"));
    }
}
