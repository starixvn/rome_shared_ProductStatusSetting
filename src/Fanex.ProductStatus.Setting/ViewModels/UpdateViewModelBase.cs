namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;
    using Services;

    public class UpdateViewModelBase
    {
        private readonly IProductStatusUser user;

        public UpdateViewModelBase()
            : this(null)
        {
        }

        public UpdateViewModelBase(IProductStatusUser user)
        {
            this.user = user ?? ProductStatusConfigurationHandler.User;
        }

        public int ProductId { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public int ActorId
        {
            get
            {
                return user.ActorId;
            }
        }

        public string IP
        {
            get
            {
                return user.IP;
            }
        }

        public virtual bool IsValid()
            => ProductId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0;
    }
}
