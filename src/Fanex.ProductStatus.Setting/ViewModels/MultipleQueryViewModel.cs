namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Services;
    using Shared;

    public class MultipleQueryViewModel
    {
        public MultipleQueryViewModel()
            : this(null)
        {
        }

        public MultipleQueryViewModel(IProductStatusUser user)
        {
            Products = (user ?? ProductStatusConfigurationHandler.User).GetProductList();

            if (CustRole != RoleIdentity.Super)
            {
                var includedProducts = ToProductDictionary();

                Products = Products.Where(p => includedProducts.ContainsKey(p.Key));
            }
        }

        public int[] ProductIds { get; set; }

        public int[] CustIds { get; set; }

        public string[] CustNames { get; set; }

        public byte CustRole { get; set; }

        public IEnumerable<KeyValuePair<int, string>> Products { get; }

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
