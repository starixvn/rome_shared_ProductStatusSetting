namespace Fanex.ProductStatus.Setting.ViewModels
{
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Models;
    using Services;

    public class UpdateViewModel
    {
        private readonly IProductStatusActor productStatusActor;

        public UpdateViewModel()
            : this(null)
        {
        }

        public UpdateViewModel(IProductStatusActor productStatusActor)
        {
            this.productStatusActor = productStatusActor ?? ProductStatusConfigurationHandler.Actor;
        }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public byte CustRole { get; set; }

        public IEnumerable<ProductStatusCommand> ProductStatus { get; set; }

        public string IP
            => productStatusActor.ClientIP;

        public int ActorId
            => productStatusActor.Id;

        public string ActorName
           => productStatusActor.Name;

        public virtual bool IsValid()
            => ProductId > 0
            && CustId > 0
            && ProductStatus.Any()
            && ProductStatus.Any(productStatus => !productStatus.Valid())
            && ActorId > 0;
    }
}
