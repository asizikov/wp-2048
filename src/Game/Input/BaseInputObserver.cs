using System;
using System.Windows;
using Game.Process;
using GameEngine;

namespace Game.Input
{
    public abstract class BaseInputObserver : IInputObserver
    {
        public event Action<Direction> Move;
        public event Action Restart;
        public event Action KeepPlaying;
        public abstract void Dispose();

        protected void RestartWhithConfirmation()
        {
            var result =
                MessageBox.Show(
                    Resources.AppResources.AreYouShureToRestart,
                    Resources.AppResources.Warning,
                    MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                FireReset();
            }
        }

        protected void FireReset()
        {
            if (Restart != null) Restart();
        }

        protected void FireKeepPlaying()
        {
            if (KeepPlaying != null)
            {
                KeepPlaying();
            }
        }

        protected void FireMove(Direction direction)
        {
            if (Move != null) Move(direction);
        }
    }
}