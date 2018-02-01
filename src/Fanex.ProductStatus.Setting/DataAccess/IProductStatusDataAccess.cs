using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.DataAccess
{
    internal interface IProductStatusDataAccess
    {
        IEnumerable<ProductStatusModel> GetProductStatus(ICriteria criteria);

        Result ExecuteNonQuery(ICriteria criteria);
    }
}
