namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Shared;

    public class MultipleQueryViewModel : BaseQueryViewModel
    {
        private readonly IEnumerable<KeyValuePair<int, string>> customerProductList;

        private IEnumerable<KeyValuePair<int, string>> products;

        public MultipleQueryViewModel()
            : this(null)
        {
        }

        public MultipleQueryViewModel(
            IEnumerable<KeyValuePair<int, string>> customerProductList)
        {
            this.customerProductList = customerProductList
                ?? ProductStatusConfigurationHandler.CustomerProductListGetter();
        }

        public string ProductIds { get; set; }

        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Products
        {
            get
            {
                if (products == null)
                {
                    products = customerProductList;

                    if (CustRole != RoleIdentity.Super)
                    {
                        var includedProducts = ProductIds.ToDictionary();

                        products = products.Where(p => includedProducts.ContainsKey(p.Key));
                    }
                }

                return products;
            }
        }
    }
}
