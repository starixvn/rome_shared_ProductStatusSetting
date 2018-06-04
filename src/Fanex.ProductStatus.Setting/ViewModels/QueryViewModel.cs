namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Services;

    public class QueryViewModel : BaseQueryViewModel
    {
        private readonly IProductStatusActor productStatusActor;

        public QueryViewModel()
            : this(null)
        {
        }

        public QueryViewModel(IProductStatusActor productStatusActor)
        {
            this.productStatusActor = productStatusActor ?? ProductStatusConfigurationHandler.Actor;
        }

        public string CustName { get; set; }

        public byte SystemId
            => productStatusActor.SystemId;
    }
}
