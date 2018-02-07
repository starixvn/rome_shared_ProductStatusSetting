using System.Collections.Generic;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    public interface IProductStatusCriteriaFactory
    {
        ICriteria CreateSelectCriteria(int productId, int custId);

        ICriteria CreateUpdateCriteria(int productId, IEnumerable<UpdateProductStatusParam> commands);

        IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(IEnumerable<UpdateProductStatusParam> commands);
    }
}
