using System.Collections.Generic;

namespace Fanex.ProductStatus.Setting.Models
{
    public class UpdateProductStatusCommand
    {
        public UpdateProductStatusCommand(
            int productId,
            int custId,
            int userId,
            string ip,
            IEnumerable<ProductStatusCommand> productStatuses,
            int uplineId = 0)
        {
            ProductId = productId;
            CustId = custId;
            UserId = userId;
            IP = ip;
            ProductStatuses = productStatuses;
            UplineId = uplineId;
        }

        public int ProductId { get; }

        public int CustId { get; }

        public int UserId { get; }

        public string IP { get; }

        public IEnumerable<ProductStatusCommand> ProductStatuses { get; }

        public int UplineId { get; }
    }
}
