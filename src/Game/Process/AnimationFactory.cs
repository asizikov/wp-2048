using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Game.Process
{
    internal static class AnimationFactory
    {
        private const int TotalDuration = 200;
        private const int MiddleDuration = 100;

        public static void AnimateCell(Border cellView)
        {
            var popUpAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(TotalDuration)
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
                    Value = 11,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(MiddleDuration))
                }
                );
            popUpAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame
                {
                    Value = 12,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(TotalDuration))
                }
                );


            Storyboard.SetTarget(popUpAnimation, cellView);
            Storyboard.SetTargetProperty(popUpAnimation, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTargetProperty(popUpAnimation, new PropertyPath("(Canvas.Top)"));

            var expandAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(TotalDuration)
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
                    Value = 94, 
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(MiddleDuration))
                }
                );
            expandAnimation.KeyFrames.Add(
                new DiscreteDoubleKeyFrame
                {
                    Value = 90, // Target value (KeyValue)
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(TotalDuration))
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