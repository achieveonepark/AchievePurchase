using UnityEngine.Purchasing;

namespace com.achieve.scripting.purchase
{
    public interface IReceiptLocalVerification
    {
        void Verify(PurchaseEventArgs args);
    }

}