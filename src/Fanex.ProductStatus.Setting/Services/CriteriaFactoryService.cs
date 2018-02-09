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

        ICriteria CreateUpdateCriteria(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands);

        IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands);
    }

    public class CriteriaFactoryServiceImpl : CriteriaFactoryService
    {
        public ICriteria CreateSelectCriteria(int productId, int custId)
        {
            switch (productId)
            {
                case ProductIdentity.NumberGame:
                case ProductIdentity.Casino:
                case ProductIdentity.Keno:
                case ProductIdentity.GoldDeluxe:
                case ProductIdentity.ColossusBets:
                    return new SelectProductStatusCriteria(productId, custId);

                case ProductIdentity.NG2:
                    return new SelectNG2StatusCriteria(productId, custId);

                default:
                    return new SelectDCSProductStatusCriteria(productId, custId);
            }
        }

        public ICriteria CreateUpdateCriteria(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            if (commands == null || !commands.Any())
            {
                throw new ArgumentNullException(nameof(commands));
            }

            switch (productId)
            {
                case ProductIdentity.NumberGame:
                    return new UpdateNumberGameStatusCriteria(custId, actorId, ip, commands.First());

                case ProductIdentity.Casino:
                    return new UpdateCasinoStatusCriteria(custId, actorId, ip, commands.First());

                case ProductIdentity.GoldDeluxe:
                    return new UpdateGoldDeluxeStatusCriteria(custId, actorId, ip, commands.First());

                case ProductIdentity.ColossusBets:
                    return new UpdateColossusBetsStatusCriteria(custId, actorId, ip, commands.First());

                case ProductIdentity.NG2:
                    return new UpdateNG2StatusCriteria(custId, actorId, ip, commands);

                default:
                    return new UpdateDCSProductStatusCriteria(custId, actorId, ip, commands);
            }
        }

        public IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            if (commands == null || !commands.Any())
            {
                yield break;
            }

            foreach (var status in commands)
            {
                yield return new UpdateKenoStatusCriteria(custId, actorId, ip, status);
            }
        }
    }
}