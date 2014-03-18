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

        private static Storyboard AnimateAppear(Border cellView)
        {
//            var popUpAnimation
//                = new DoubleAnimationUsingKeyFrames
//                {
//                    Duration = TimeSpan.FromMilliseconds(TotalDuration)
//                };
//
//
//            popUpAnimation.KeyFrames.Add(
//                new LinearDoubleKeyFrame
//                {
//                    Value = 12,
//                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
//                });
//
//            popUpAnimation.KeyFrames.Add(
//                new LinearDoubleKeyFrame
//                {
//                    Value = 11,
//                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(MiddleDuration))
//                }
//                );
//            popUpAnimation.KeyFrames.Add(
//                new LinearDoubleKeyFrame
//                {
//                    Value = 12,
//                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(TotalDuration))
//                }
//                );
//
//
//            Storyboard.SetTarget(popUpAnimation, cellView);
//            Storyboard.SetTargetProperty(popUpAnimation, new PropertyPath("(Canvas.Left)"));
//            Storyboard.SetTargetProperty(popUpAnimation, new PropertyPath("(Canvas.Top)"));

            var expandAnimation
                = new DoubleAnimationUsingKeyFrames
                {
                    Duration = TimeSpan.FromMilliseconds(TotalDuration)
                };


            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 100,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(0))
                });

            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 110,
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(MiddleDuration))
                }
                );
            expandAnimation.KeyFrames.Add(
                new LinearDoubleKeyFrame
                {
                    Value = 100, // Target value (KeyValue)
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(TotalDuration))
                }
                );
            // Create a storyboard to apply the animation.
            Storyboard.SetTarget(expandAnimation, cellView);
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Height)"));
            Storyboard.SetTargetProperty(expandAnimation, new PropertyPath("(Width)"));

            var popUpStoryBoard = new Storyboard();
//            popUpStoryBoard.Children.Add(popUpAnimation);
            popUpStoryBoard.Children.Add(expandAnimation);
            return popUpStoryBoard;
        }

        private static void FaidIn(Border cellView)
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
                Duration = TimeSpan.FromMilliseconds(150)
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
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(150))
                }
                );

            Storyboard.SetTarget(moveAnimationX, cellView);
            Storyboard.SetTargetProperty(moveAnimationX, new PropertyPath(string.Format("(Canvas.{0})",
                (isHorisontal ? "Left" : "Top"))));

            var moveStoryBoard = new Storyboard();
            moveStoryBoard.Children.Add(moveAnimationX);
            if (type == AnimationType.MoveAndMerge)
            {
                var app = AnimateAppear(cellView);
                moveStoryBoard.Children.Add(app);
            }
            
            moveStoryBoard.Begin();
        }
    }
}