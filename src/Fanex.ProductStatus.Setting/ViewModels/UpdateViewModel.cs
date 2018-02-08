namespace Fanex.ProductStatus.Setting.ViewModels
{
    using Configuration;
    using Models;

    public class UpdateViewModel
    {
        public UpdateViewModel()
        {
            var userService = ProductStatusSettingsHandler.CreateUserService();

            ActorId = userService.ActorId;
            IP = userService.IP;
        }

        public int ProductId { get; set; }

        public int CustId { get; set; }

        public ProductStatusCommand[] ProductStatus { get; set; }

        public int ActorId { get; }

        public string IP { get; }

        public bool IsValid()
            => ProductId > 0
            && CustId > 0
            && ProductStatus?.Length > 0
            && ActorId > 0
            && !string.IsNullOrEmpty(IP);
    }
}
