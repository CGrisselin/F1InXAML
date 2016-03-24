using System;

namespace F1InXAML
{
    internal class SlideNavigatorFrame
    {
        public SlideNavigatorFrame(int slideIndex, Action setupSlide)
        {
            SlideIndex = slideIndex;
            SetupSlide = setupSlide;            
        }

        public int SlideIndex { get; }

        public Action SetupSlide { get; }
    }
}