using UnityEngine.Purchasing;

namespace Achieve.BreezeIAP
{
    public interface IReceiptLocalVerification
    {
        void Verify(PurchaseEventArgs args);
    }

}