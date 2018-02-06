using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    internal interface IProductStatusCriteriaFactory
    {
        ICriteria CreateSelectCriteria(int productId, int custId);

        ICriteria CreateUpdateCriteria(IEnumerable<ProductStatusCommand> commands);

        IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(IEnumerable<ProductStatusCommand> commands);
    }
}
