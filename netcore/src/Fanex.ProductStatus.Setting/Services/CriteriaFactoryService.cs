namespace Fanex.ProductStatus.Setting.Services
{
    using System;
    using System.Collections.Generic;
    using Criteria;
    using Models;
    // TODO: Requires Fanex.NAP.Framework.Data migration
    // using NAP.Framework.Data;
    using Shared;

    public interface CriteriaFactoryService
    {
        // TODO: Requires ICriteria from Fanex.NAP.Framework
        // ICriteria CreateSelectCriteria(int productId, int custId, int sportType, byte settingStatus);
        object CreateSelectCriteria(int productId, int custId, int sportType, byte settingStatus);

        // ICriteria CreateUpdateCriteria(
        object CreateUpdateCriteria(
            int productId,
            int custId,
            int actorId,
            string ip,
            byte settingStatus,
            IEnumerable<ProductStatusCommand> commands);
    }

    internal class CriteriaFactoryServiceImpl : CriteriaFactoryService
    {
        private const int MacauGamesStatusSettingType = 21;

        public object CreateSelectCriteria(int productId, int custId, int sportType, byte settingStatus)
        {
            // TODO: Requires ICriteria from Fanex.NAP.Framework
            throw new NotImplementedException("Requires ICriteria from Fanex.NAP.Framework");
            /*
            switch (productId)
            {
                case ProductIdentity.Keno:
                    return new SelectProductStatusCriteria(productId, custId, settingStatus);

                case ProductIdentity.RNGSlot:
                    return new SelectDCSProductStatusCriteria(productId, custId, settingStatus, MacauGamesStatusSettingType);

                default:
                    return new SelectDCSProductStatusCriteria(productId, custId, settingStatus);
            }
            */
        }

        public object CreateUpdateCriteria(
            int productId,
            int custId,
            int actorId,
            string ip,
            byte settingStatus,
            IEnumerable<ProductStatusCommand> commands)
        {
            // TODO: Requires ICriteria from Fanex.NAP.Framework
            throw new NotImplementedException("Requires ICriteria from Fanex.NAP.Framework");
            /*
            switch (productId)
            {
                case ProductIdentity.RNGSlot:
                    return new UpdateDCSProductStatusCriteria(custId, actorId, ip, commands, settingStatus, MacauGamesStatusSettingType);

                default:
                    return new UpdateDCSProductStatusCriteria(custId, actorId, ip, commands, settingStatus);
            }
            */
        }
    }
}