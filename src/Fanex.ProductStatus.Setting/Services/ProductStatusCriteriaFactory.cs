using System.Collections.Generic;
using System.Linq;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Criteria;
using Fanex.ProductStatus.Setting.Enumerations;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    public class ProductStatusCriteriaFactory : IProductStatusCriteriaFactory
    {
        public ICriteria CreateSelectCriteria(int productId, int custId)
        {
            var id = (ProductIdentity)productId;

            switch (id)
            {
                case ProductIdentity.Casino:
                case ProductIdentity.Keno:
                case ProductIdentity.ColossusBets:
                    return new SelectProductStatusCriteria(productId, custId);

                case ProductIdentity.GoldDeluxe:
                    return new SelectGoldDeluxeStatusCriteria(productId, custId);

                case ProductIdentity.NG2:
                    return new SelectNG2StatusCriteria(productId, custId);

                default:
                    return new SelectDCSProductStatusCriteria(productId, custId);
            }
        }

        public ICriteria CreateUpdateCriteria(int productId, IEnumerable<UpdateProductStatusParam> commands)
        {
            if (commands == null || !commands.Any())
            {
                return null;
            }

            var id = (ProductIdentity)productId;

            switch (id)
            {
                case ProductIdentity.Casino:
                    return new UpdateCasinoStatusCriteria(commands.First());

                case ProductIdentity.GoldDeluxe:
                    return new UpdateGoldDeluxeStatusCriteria(commands.First());

                case ProductIdentity.ColossusBets:
                    return new UpdateColossusBetsStatusCriteria(commands.First());

                case ProductIdentity.NG2:
                    return new UpdateNG2StatusCriteria(commands);

                default:
                    return new UpdateDCSProductStatusCriteria(commands);
            }
        }

        public IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(IEnumerable<UpdateProductStatusParam> commands)
        {
            if (commands == null || !commands.Any())
            {
                yield break;
            }

            foreach (var item in commands)
            {
                yield return new UpdateKenoStatusCriteria(item);
            }
        }
    }
}
