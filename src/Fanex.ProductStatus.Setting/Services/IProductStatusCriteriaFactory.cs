using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    internal interface IProductStatusCriteriaFactory
    {
        ICriteria CreateSelectCriteria(int productId, int custId);

        ICriteria CreateUpdateCriteria(UpdateProductStatusCommand updateCommand);

        IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(UpdateProductStatusCommand updateCommand);
    }
}
