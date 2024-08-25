using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

namespace com.achieve.scripting.purchase
{
    internal class AchievePurchaseReceiver : IDetailedStoreListener
    {
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            AchievePurchase.controller = controller;
            AchievePurchase.extensionProvider = extensions;
            AchievePurchase.initializeCompletionSource.TrySetResult(InitializeResult.Success());
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            AchievePurchase.initializeCompletionSource.TrySetResult(InitializeResult.Error(error.ToString()));
        }

        public void OnInitializeFailed(InitializationFailureReason error, string message)
        {
            AchievePurchase.initializeCompletionSource.TrySetResult(InitializeResult.Error(error.ToString()));
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureDescription failureDescription)
        {
            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = PurchaseType.Purchase,
                Product = product,
                ErrorMessage = failureDescription.reason.ToString()
            };

            AchievePurchase.purchaseCompletionSource.TrySetResult(purchaseResult);
        }

        public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = PurchaseType.Purchase,
                Product = product,
                ErrorMessage = failureReason.ToString()
            };

            AchievePurchase.purchaseCompletionSource.TrySetResult(purchaseResult);
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            PurchaseType type = AchievePurchase.isCheckingPendingList ? PurchaseType.Pending : PurchaseType.Purchase;

            PurchaseResult purchaseResult = new PurchaseResult
            {
                Type = type,
                Product = purchaseEvent.purchasedProduct,
                ErrorMessage = string.Empty
            };

            AchievePurchase.AddPendingList(purchaseResult);
            AchievePurchase.purchaseCompletionSource.TrySetResult(purchaseResult);

            return PurchaseProcessingResult.Pending;
        }
    }
}