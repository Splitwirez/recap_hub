using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using ReCap.Hub.ViewModels;

namespace ReCap.Hub
{
    public class ViewLocator : IDataTemplate
    {
        public bool SupportsRecycling => false;

        public Control Build(object data)
        {
            Control ret = null;
            
            if ((data != null) && (data is RxObjectBase rxData))
            {
                var type = rxData.GetViewType();
            
                if ((type != null) && type.IsAssignableTo(typeof(Control)))
                {
                    var inst = Activator.CreateInstance(type);
                    ret = (Control)inst;
                }
            }
            
            return (ret != null)
                ? ret
                : new TextBlock
                    {
                        Text = $"Couldn't find view for type '{data.GetType().FullName}'"
                    };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}