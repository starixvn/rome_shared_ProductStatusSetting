using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    public static class ProductStatusSettingManager
    {
        public static IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId)
        {
            IProductStatusService statusService = new ProductStatusService();

            return statusService.GetProductStatus(productId, custId);
        }
    }
}
