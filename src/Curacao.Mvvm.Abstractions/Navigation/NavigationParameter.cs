using JetBrains.Annotations;

namespace Curacao.Mvvm.Abstractions.Navigation
{
    [PublicAPI]
    public sealed class NavigationParameter
    {
        public string Parameter { get; set; }
        public string Value { get; set; }
    }
}