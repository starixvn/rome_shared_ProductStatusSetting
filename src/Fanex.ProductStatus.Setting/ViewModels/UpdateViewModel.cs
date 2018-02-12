namespace Fanex.ProductStatus.Setting.ViewModels
{
    public class UpdateViewModel : UpdateViewModelBase
    {
        public int CustId { get; set; }

        public override bool IsValid()
            => base.IsValid() && CustId > 0;
    }
}
