namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;

    public class UpdateViewModelBase
    {
        protected UpdateViewModelBase()
        {
            var userService = ProductStatusConfigurationHandler.CreateUserService();

            ActorId = userService.ActorId;
            IP = userService.IP;
        }

        public int ProductId { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public int ActorId { get; }

        public string IP { get; }

        public virtual bool IsValid()
            => ProductId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0;
    }
}
