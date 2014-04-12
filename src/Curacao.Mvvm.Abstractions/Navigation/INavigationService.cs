using System.Collections.Generic;
using JetBrains.Annotations;

namespace Curacao.Mvvm.Abstractions.Navigation
{
    [PublicAPI]
    public interface INavigationService
    {

        [PublicAPI] void GoBack();
        [PublicAPI] bool CanGoBack();
        [PublicAPI] void GoToPage(string page, IEnumerable<NavigationParameter> parameters = null);
        [PublicAPI] void GoToPage(string page, IDictionary<string, string> parameters);
        [PublicAPI] void CleanNavigationStack();
        [PublicAPI] void GoToPage(string page, [CanBeNull] IEnumerable<NavigationParameter> parameters, int numberOfItemsToRemove);
    }
}
