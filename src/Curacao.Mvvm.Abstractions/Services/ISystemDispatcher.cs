using System;

namespace Curacao.Mvvm.Abstractions.Services
{
    public interface ISystemDispatcher
    {
        void InvokeOnUIifNeeded(Action action);
    }
}