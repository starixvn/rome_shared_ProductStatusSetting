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

        public ICriteria CreateUpdateCriteria(IEnumerable<ProductStatusCommand> commands)
        {
            if (commands == null || !commands.Any())
            {
                return null;
            }

            var firstCommand = commands.First();
            var id = (ProductIdentity)firstCommand.ProductId;

            switch (id)
            {
                case ProductIdentity.Casino:
                    return new UpdateCasinoStatusCriteria(firstCommand);

                case ProductIdentity.GoldDeluxe:
                    return new UpdateGoldDeluxeStatusCriteria(firstCommand);

                case ProductIdentity.ColossusBets:
                    return new UpdateColossusBetsStatusCriteria(firstCommand);

                case ProductIdentity.NG2:
                    return new UpdateNG2StatusCriteria(commands);

                default:
                    return new UpdateDCSProductStatusCriteria(commands);
            }
        }

        public IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(IEnumerable<ProductStatusCommand> commands)
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
