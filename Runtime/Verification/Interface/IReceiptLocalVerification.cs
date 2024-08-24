using UnityEngine.Purchasing;

public interface IReceiptLocalVerification
{
    void Verify(PurchaseEventArgs args);
}
