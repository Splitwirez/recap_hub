using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using ReactiveUI;
using ReCap.Hub;

namespace ReCap.Hub.Models
{
    public class ModelBase : RxObjectBase
    {
        public override Type GetViewType()
            => Type.GetType(GetType().FullName.Replace("Model", "View"));
    }
}
