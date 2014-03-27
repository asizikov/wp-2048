using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Game.Process
{
    internal static class AnimationFactory
    {
        private const int TotalDuration = 200;
        private const int MiddleDuration = 100;

        private static readonly Dictionary<int, int> Positions = new Dictionary<int, int>
        {
            {0, 0},
            {1, 113},
            {2, 226},
            {3, 339},
        };

        private static Storyboard AnimateAppear(DependencyObject cellView, AnimationType type)
        {
            var totalDuration = type == AnimationType.Appear ? TotalDuration : TotalDuration*2;
            var start = type == AnimationType.Appear ? 0 : TotalDuration;
            var middleDuration = type == AnimationType.Appear ? MiddleDuration : MiddleDuration*2;

            var expandAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(totalDuration)
                };


            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 100,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(start))
                });

            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 110,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(middleDuration))
                }
                );
            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 100,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(totalDuration))
                }
                );
            Storyboard.SetTarget(expandAnimation, cellView);
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Height)"));
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Width)"));

            var popUpStoryBoard = new Storyboard();
            popUpStoryBoard.Children.Add(expandAnimation);
            return popUpStoryBoard;
        }

        private static void FaidIn(DependencyObject cellView)
        {
            var expandAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(TotalDuration)
                };


            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 0,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
                });

            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 1,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(TotalDuration))
                }
                );
            // Create a storyboard to apply the animation.
            Storyboard.SetTarget(expandAnimation, cellView);
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Opacity)"));

            var popUpStoryBoard = new Storyboard();
            popUpStoryBoard.Children.Add(expandAnimation);
            popUpStoryBoard.Begin();
        }

        public static void ApplyAnimation(AnimationType type, Border cellView, Position previousPosition, int x, int y)
        {
            if (type == AnimationType.Appear)
            {
                FaidIn(cellView);
                return;
            }

            bool isHorisontal = false;
            int start, end;
            if (previousPosition.X == x)
            {
                start = previousPosition.Y;
                end = y;
            }
            else
            {
                isHorisontal = true;
                start = previousPosition.X;
                end = x;
            }
            var moveAnimationX = new DoubleAnimationUsingKeyFrames
            {
                Duration = TimeSpan.FromMilliseconds(200)
            };

            moveAnimationX.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = Positions[start],
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
                });

            moveAnimationX.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = Positions[end],
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(200))
                }
                );

            Storyboard.SetTarget(moveAnimationX, cellView);
            Storyboard.SetTargetProperty(moveAnimationX, new PropertyPath(string.Format("(Canvas.{0})",
                (isHorisontal ? "Left" : "Top"))));

            var moveStoryBoard = new Storyboard();
            moveStoryBoard.Children.Add(moveAnimationX);
            if (type == AnimationType.MoveAndMerge)
            {
                var app = AnimateAppear(cellView, type);
                moveStoryBoard.Children.Add(app);
            }
            
            moveStoryBoard.Begin();
        }
    }
}