using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace Achieve.BreezeIAP
{
    internal class BreezeIAPReceiver : IDetailedStoreListener
    {
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            BreezeIAP.controller = controller;
            BreezeIAP.extensionProvider = extensions;
            BreezeIAP.initializeCompletionSource.TrySetResult(InitializeResult.Success());
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            BreezeIAP.initializeCompletionSource.TrySetResult(InitializeResult.Error(error.ToString()));
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            BreezeIAP.initializeCompletionSource.TrySetResult(InitializeResult.Error(error.ToString()));
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = PurchaseType.Purchase,
                Product = product,
                ErrorMessage = failureDescription.reason.ToString()
            };

            BreezeIAP.purchaseCompletionSource.TrySetResult(purchaseResult);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = PurchaseType.Purchase,
                Product = product,
                ErrorMessage = failureReason.ToString()
            };

            BreezeIAP.purchaseCompletionSource.TrySetResult(purchaseResult);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            PurchaseType type = BreezeIAP.isCheckingPendingList ? PurchaseType.Pending : PurchaseType.Purchase;

            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = type,
                Product = purchaseEvent.purchasedProduct,
                ErrorMessage = string.Empty
            };

            BreezeIAP.AddPendingList(purchaseResult);
            BreezeIAP.purchaseCompletionSource.TrySetResult(purchaseResult);

            return PurchaseProcessingResult.Pending;
        }
    }
}