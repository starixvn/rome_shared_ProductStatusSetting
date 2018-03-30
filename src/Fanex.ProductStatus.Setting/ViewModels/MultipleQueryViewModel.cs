namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Services;
    using Shared;

    public class MultipleQueryViewModel : BaseQueryViewModel
    {
        private readonly IProductStatusUserService _userService;

        public MultipleQueryViewModel()
            : this(null)
        {
        }

        public MultipleQueryViewModel(IProductStatusUserService userService)
        {
            _userService = userService ?? ProductStatusConfigurationHandler.UserService;
        }

        public string ProductIds { get; set; }

        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Products
        {
            get
            {
                var products = _userService.GetProductList();

                if (CustRole != RoleIdentity.Super)
                {
                    var includedProducts = ProductIds.ToDictionary();

                    products = products.Where(p => includedProducts.ContainsKey(p.Key));
                }

                return products;
            }
        }
    }
}
