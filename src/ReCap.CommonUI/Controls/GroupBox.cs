using Avalonia.Controls.Primitives;

namespace ReCap.CommonUI
{
    public class GroupBox
        : HeaderedContentControl
    {
        static GroupBox()
        {
            Extensions.MakeControlTypeNonInteractive<GroupBox>();
        }
    }
}
