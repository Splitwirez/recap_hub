using System;

namespace ReCap.Hub.ViewModels
{
    public abstract class ViewModelBase
        : RxObjectBase
    {
        public override Type GetViewType()
            => Type.GetType(GetType().FullName.Replace("ViewModel", "View"));
    }
}
