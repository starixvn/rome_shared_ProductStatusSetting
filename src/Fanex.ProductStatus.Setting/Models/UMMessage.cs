namespace Fanex.ProductStatus.Setting.Models
{
    using System;

    public class UMMessage
    {
        public UMMessage(
            int productId,
            int sportType,
            DateTime beginTime,
            DateTime endTime)
        {
            ProductId = productId;
            SportType = sportType;
            BeginTime = beginTime;
            EndTime = endTime;
        }

        public int ProductId { get; }

        public int SportType { get; }

        public DateTime BeginTime { get; }

        public DateTime EndTime { get; }
    }
}
