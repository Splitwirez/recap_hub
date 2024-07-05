using System;
using Avalonia.Animation.Easings;

namespace ReCap.CommonUI
{
    public class HybridEaseInOut
        : Easing
    {
        static readonly Easing _ease1 = new SineEaseInOut();
        static readonly Easing _ease2 = new CubicEaseInOut();
        /// <inheritdoc/>
        public override double Ease(double progress)
        {
            if (progress > 0.5)
                return _ease2.Ease(progress);
            else
                return _ease1.Ease(progress);
        }
    }
}
