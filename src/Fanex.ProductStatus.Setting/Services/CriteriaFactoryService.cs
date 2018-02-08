namespace Fanex.ProductStatus.Setting.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Models;
    using NAP.Framework.Data;
    using Shared;

    public interface CriteriaFactoryService
    {
        ICriteria CreateSelectCriteria(int productId, int custId);

        ICriteria CreateUpdateCriteria(int productId, IEnumerable<ProductStatusCommand> commands);

        IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(IEnumerable<ProductStatusCommand> commands);
    }

    public class CriteriaFactoryServiceImpl : CriteriaFactoryService
    {
        public ICriteria CreateSelectCriteria(int productId, int custId)
        {
            switch (productId)
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

        public ICriteria CreateUpdateCriteria(int productId, IEnumerable<ProductStatusCommand> commands)
        {
            if (commands == null || !commands.Any())
            {
                throw new ArgumentNullException(nameof(commands));
            }

            switch (productId)
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