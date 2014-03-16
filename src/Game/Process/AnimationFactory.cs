using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Game.Process
{
    internal static class AnimationFactory
    {
        private static int totalDuration = 400;
        private static int _middleDuration =250;

        public static void AnimateCell(Border cellView)
        {
            var popUpAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(totalDuration)
                };


            popUpAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 12,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
                });

            popUpAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame
                {
                    Value = 6,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_middleDuration))
                }
                );
            popUpAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame
                {
                    Value = 12, // Target value (KeyValue)
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration))
                }
                );


            Storyboard.SetTarget(popUpAnimation, cellView);
            Storyboard.SetTargetProperty(popUpAnimation, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(popUpAnimation, new PropertyPath("(Canvas.Top)"));

            var expandAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(totalDuration)
                };


            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 90,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
                });

            expandAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame
                {
                    Value = 102, // Target value (KeyValue)
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(_middleDuration))
                }
                );
            expandAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame
                {
                    Value = 90, // Target value (KeyValue)
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration))
                }
                );
            // Create a storyboard to apply the animation.
            Storyboard.SetTarget(expandAnimation, cellView);
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Height)"));
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Width)"));

            var popUpStoryBoard = new Storyboard();
            popUpStoryBoard.Children.Add(popUpAnimation);
            popUpStoryBoard.Children.Add(expandAnimation);
            popUpStoryBoard.Begin();
        }
    }
}