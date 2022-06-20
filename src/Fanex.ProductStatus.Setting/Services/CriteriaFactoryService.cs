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
        private const int MacauGamesStatusSettingType = 21;

        public ICriteria CreateSelectCriteria(int productId, int custId, int sportType, byte settingStatus)
        {
            switch (productId)
            {
                case ProductIdentity.NumberGame:
                case ProductIdentity.Keno:
                    return new SelectProductStatusCriteria(productId, custId, settingStatus);

                case ProductIdentity.NG2:
                    return new SelectNG2StatusCriteria(productId, custId, settingStatus);

                case ProductIdentity.Sportbook2:
                    if (sportType == SportTypeIndentity.Esports)
                    {
                        return new SelectEsportProductStatusCriteria(productId, custId, settingStatus);
                    }
                    return new SelectDCSProductStatusCriteria(productId, custId, settingStatus);
                case ProductIdentity.MacauGames:
                    return new SelectDCSProductStatusCriteria(productId, custId, settingStatus, MacauGamesStatusSettingType);
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
                case ProductIdentity.NG2:
                    return new UpdateNG2StatusCriteria(custId, actorId, ip, commands);
                case ProductIdentity.MacauGames:
                    return new UpdateDCSProductStatusCriteria(custId, actorId, ip, commands, MacauGamesStatusSettingType);
                default:
                    return new UpdateDCSProductStatusCriteria(custId, actorId, ip, commands);
            }
        }
    }
}