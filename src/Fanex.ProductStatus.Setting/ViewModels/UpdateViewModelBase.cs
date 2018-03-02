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

        public string IP
            => user.GetClientIP().Invoke();

        public int ActorId
            => user.GetActorId().Invoke();

        public virtual bool IsValid()
            => ProductId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0;
    }
}
