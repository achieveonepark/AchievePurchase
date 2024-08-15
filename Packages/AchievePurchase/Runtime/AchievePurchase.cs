using UnityEngine.Purchasing;

public class AchievePurchase
{
    private static AchievePurchaseReceiver _receiver;

    public static void Initialize(InitializeDto[] dtos)
    {
        _receiver = new AchievePurchaseReceiver();
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        for (int i = 0; i < dtos.Length; i++)
        {
            var dto = dtos[i];
            builder.AddProduct(dto.ProductId, dto.ProductType);
        }
        UnityPurchasing.Initialize(_receiver, builder);
    }

    public static void Purchase(string productId)
    {
        
    }

    public static PurchaseProcessingResult GetPendingResult()
    {
        return new PurchaseProcessingResult();
    }
}
