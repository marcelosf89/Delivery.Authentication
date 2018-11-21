using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class ScopeViewModel
    {
        private static readonly ScopeViewModel _empty = new ScopeViewModel();
        public static ref readonly ScopeViewModel Empty => ref _empty;

        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public bool Emphasize { get; set; }
        public bool Required { get; set; }
        public bool Checked { get; set; }

    }
}
