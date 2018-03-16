namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Services;
    using Shared;

    public class MultipleQueryViewModel
    {
        private readonly IProductStatusUser _user;

        public MultipleQueryViewModel()
            : this(null)
        {
        }

        public MultipleQueryViewModel(IProductStatusUser user)
        {
            _user = user ?? ProductStatusConfigurationHandler.User;
        }

        public int[] ProductIds { get; set; }

        public int[] CustIds { get; set; }

        public string[] CustNames { get; set; }

        public byte CustRole { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Products
        {
            get
            {
                var products = _user.GetProductList();

                if (CustRole != RoleIdentity.Super)
                {
                    var includedProducts = ToProductDictionary();

                    products = products.Where(p => includedProducts.ContainsKey(p.Key));
                }

                return products;
            }
        }

        private IDictionary<int, bool> ToProductDictionary()
        {
            IDictionary<int, bool> productDictionary = new Dictionary<int, bool>();

            if (ProductIds?.Length > 0)
            {
                foreach (var item in ProductIds)
                {
                    productDictionary[item] = true;
                }
            }

            return productDictionary;
        }
    }
}
