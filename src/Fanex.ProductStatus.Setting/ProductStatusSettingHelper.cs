using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;
using Fanex.ProductStatus.Setting.Services;

namespace Fanex.ProductStatus.Setting
{
    public static class ProductStatusSettingHelper
    {
        public static IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId)
        {
            IProductStatusService statusService = new ProductStatusService();

            return statusService.GetProductStatus(productId, custId);
        }
    }
}
