using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace com.achieve.scripting.purchase
{
    public class AndroidReceiptLocalVerification : IReceiptLocalVerification
    {
        public void Verify(PurchaseEventArgs args)
        {
            //var validator = new CrossPlatformValidator
        }
    }
}