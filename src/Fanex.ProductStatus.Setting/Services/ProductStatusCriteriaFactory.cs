using System.Collections.Generic;
using System.Linq;
using Fanex.NAP.Framework.Data;
using Fanex.ProductStatus.Setting.Criteria;
using Fanex.ProductStatus.Setting.Enumerations;
using Fanex.ProductStatus.Setting.Models;

namespace Fanex.ProductStatus.Setting.Services
{
    internal class ProductStatusCriteriaFactory : IProductStatusCriteriaFactory
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

        public ICriteria CreateUpdateCriteria(UpdateProductStatusCommand updateCommand)
        {
            var isValidCommand = updateCommand?.ProductStatuses?.Any();

            if (isValidCommand != true)
            {
                return null;
            }

            var id = (ProductIdentity)updateCommand.ProductId;

            switch (id)
            {
                case ProductIdentity.Casino:
                    return CreateUpdateCasinoStatusCriteria(updateCommand);

                case ProductIdentity.GoldDeluxe:
                    return CreateUpdateGoldDeluxeStatusCriteria(updateCommand);

                case ProductIdentity.ColossusBets:
                    return CreateUpdateColossusBetsStatusCriteria(updateCommand);

                case ProductIdentity.NG2:
                    return CreateUpdateNG2StatusCriteria(updateCommand);

                default:
                    return CreateUpdateDCSProductStatusCriteria(updateCommand);
            }
        }

        public IEnumerable<ICriteria> CreateUpdateKenoStatusCriteria(UpdateProductStatusCommand updateCommand)
        {
            var productStatuses = updateCommand?.ProductStatuses;

            if (productStatuses == null || !productStatuses.Any())
            {
                yield break;
            }

            foreach (var item in productStatuses)
            {
                yield return new UpdateKenoStatusCriteria(
                    updateCommand.CustId,
                    updateCommand.UserId,
                    updateCommand.UplineId,
                    updateCommand.IP,
                    item);
            }
        }

        private static ICriteria CreateUpdateCasinoStatusCriteria(UpdateProductStatusCommand updateCommand)
        {
            return new UpdateCasinoStatusCriteria(
                updateCommand.CustId,
                updateCommand.UserId,
                updateCommand.IP,
                updateCommand.ProductStatuses.First());
        }

        private static ICriteria CreateUpdateGoldDeluxeStatusCriteria(UpdateProductStatusCommand updateCommand)
        {
            return new UpdateGoldDeluxeStatusCriteria(
                updateCommand.CustId,
                updateCommand.UserId,
                updateCommand.IP,
                updateCommand.ProductStatuses.First());
        }

        private static ICriteria CreateUpdateColossusBetsStatusCriteria(UpdateProductStatusCommand updateCommand)
        {
            return new UpdateColossusBetsStatusCriteria(
                updateCommand.CustId,
                updateCommand.UserId,
                updateCommand.IP,
                updateCommand.ProductStatuses.First());
        }

        private static ICriteria CreateUpdateNG2StatusCriteria(UpdateProductStatusCommand updateCommand)
        {
            return new UpdateNG2StatusCriteria(
                updateCommand.CustId,
                updateCommand.UserId,
                updateCommand.IP,
                updateCommand.ProductStatuses);
        }

        private static ICriteria CreateUpdateDCSProductStatusCriteria(UpdateProductStatusCommand updateCommand)
        {
            return new UpdateDCSProductStatusCriteria(
                updateCommand.CustId,
                updateCommand.UserId,
                updateCommand.IP,
                updateCommand.ProductStatuses);
        }
    }
}
