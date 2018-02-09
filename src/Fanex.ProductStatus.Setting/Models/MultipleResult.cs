namespace Fanex.ProductStatus.Setting.Models
{
    using System.Collections;

    public class MultipleResult : Result
    {
        public MultipleResult(int code, string message, int custId, string custName)
            : base(code, message)
        {
            CustId = custId;
            CustName = custName;
        }

        public MultipleResult(Hashtable outputParams)
            : base(outputParams)
        {
        }

        public int CustId { get; }

        public string CustName { get; }
    }
}
