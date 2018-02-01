using System.Collections.Generic;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    internal interface IProductStatusService
    {
        IEnumerable<ProductStatusModel> GetProductStatus(int productId, int custId);

        Result UpdateProductStatus(UpdateProductStatusCommand updateCommand);
    }
}
