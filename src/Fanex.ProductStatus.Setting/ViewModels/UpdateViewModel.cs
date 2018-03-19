namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;
    using Services;

    public class UpdateViewModel
    {
        private readonly IProductStatusUser user;

        public UpdateViewModel()
            : this(null)
        {
        }

        public UpdateViewModel(IProductStatusUser user)
        {
            this.user = user ?? ProductStatusConfigurationHandler.User;
        }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public string IP
            => user.GetClientIP();

        public int ActorId
            => user.GetActorId();

        public virtual bool IsValid()
            => ProductId > 0
            && CustId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0;
    }
}
