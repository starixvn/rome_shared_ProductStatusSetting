namespace Fanex.ProductStatus.Setting.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Criteria;
    using Models;
    using NAP.Framework.Data;
    using Shared;

    public interface CriteriaFactoryService
    {
        ICriteria CreateSelectCriteria(int productId, int custId, int sportType, byte settingStatus);

        ICriteria CreateUpdateCriteria(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands);
    }

    internal class CriteriaFactoryServiceImpl : CriteriaFactoryService
    {
        public ICriteria CreateSelectCriteria(int productId, int custId, int sportType, byte settingStatus)
        {
            switch (productId)
            {
                case ProductIdentity.NumberGame:
                case ProductIdentity.Casino:
                case ProductIdentity.Keno:
                case ProductIdentity.GoldDeluxe:
                    return new SelectProductStatusCriteria(productId, custId, settingStatus);

                case ProductIdentity.NG2:
                    return new SelectNG2StatusCriteria(productId, custId, settingStatus);

                case ProductIdentity.Sportbook2:
                    if (sportType == SportTypeIndentity.Esports)
                    {
                        return new SelectEsportProductStatusCriteria(productId, custId, settingStatus);
                    }
                    return new SelectDCSProductStatusCriteria(productId, custId, settingStatus);

                default:
                    return new SelectDCSProductStatusCriteria(productId, custId, settingStatus);
            }
        }

        public ICriteria CreateUpdateCriteria(
            int productId,
            int custId,
            int actorId,
            string ip,
            IEnumerable<ProductStatusCommand> commands)
        {
            switch (productId)
            {
                case ProductIdentity.NumberGame:
                    return new UpdateNumberGameStatusCriteria(custId, actorId, ip, commands.First());

                case ProductIdentity.GoldDeluxe:
                    return new UpdateGoldDeluxeStatusCriteria(custId, actorId, ip, commands.First());

                case ProductIdentity.NG2:
                    return new UpdateNG2StatusCriteria(custId, actorId, ip, commands);

                default:
                    return new UpdateDCSProductStatusCriteria(custId, actorId, ip, commands);
            }
        }
    }
}