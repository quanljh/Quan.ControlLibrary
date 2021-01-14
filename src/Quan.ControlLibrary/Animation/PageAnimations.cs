using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Quan.ControlLibrary
{
    public static class PageAnimations
    {
        /// <summary>
        /// Slides a page in from right
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeInFromRight(this ContentControl page, float seconds)
        {
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideFromRight(seconds, page.ActualWidth);

            //Add fade in animation
            sb.AddFadeIn(seconds);

            //Start animating
            sb.Begin(page);

            //Make page visible
            page.Visibility = Visibility.Visible;

            //Wait for it finish
            await Task.Delay((int)(seconds * 1000));
        }


        /// <summary>
        /// Slides a page out to the left
        /// </summary>
        /// <param name="page">The page to animate</param>
        /// <param name="seconds">The time the animation will take</param>
        /// <returns></returns>
        public static async Task SlideAndFadeOutToLeft(this ContentControl page, float seconds)
        {
            var sb = new Storyboard();

            //Add slide from right animation
            sb.AddSlideToLeft(seconds, page.ActualWidth);

            //Add fade in animation
            sb.AddFadeOut(seconds);

            //Start animating
            sb.Begin(page);

            //Make page visible
            page.Visibility = Visibility.Visible;

            //Wait for it finish
            await Task.Delay((int)(seconds * 1000));
        }
    }
}
