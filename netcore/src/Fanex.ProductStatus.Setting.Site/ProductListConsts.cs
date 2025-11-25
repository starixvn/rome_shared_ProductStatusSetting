// Extracted from Global.asax.cs for ASP.NET Core compatibility
namespace Fanex.ProductStatus.Setting.Site
{
    using System.Collections.Generic;
    using Configuration;

    public static class ProductListConsts
    {
        public static IEnumerable<Product> Products
        {
            get
            {
                return new List<Product>
                {
                    new Product(2, "RNG Games", true),
                    new Product(3, "Live & RNG Casino", true)
                };
            }
        }
    }
}
