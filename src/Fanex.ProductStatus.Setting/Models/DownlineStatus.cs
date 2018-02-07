namespace Fanex.ProductStatus.Setting.Models
{
    public class DownlineStatus
    {
        private readonly bool isDisabledUpline;

        public DownlineStatus(bool isDisabledUpline, int totalDisabled, int totalEnabled)
        {
            this.isDisabledUpline = isDisabledUpline;
            TotalDisabled = totalDisabled;
            TotalEnabled = totalEnabled;
        }

        public int TotalDisabled { get; }

        public int TotalEnabled { get; }

        public int Total => TotalDisabled + TotalEnabled;

        public bool IsDisabled => isDisabledUpline || Total == 0;
    }
}
