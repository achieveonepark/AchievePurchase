# AchievePurchase 
Unity IAP를 래핑하여 사용자가 보다 편리하게 접근할 수 있도록 인터페이스를 제공합니다.



## 🖥️ API
이 패키지는 아래의 기능을 제공합니다.
```
AchievePurchase.Initialize      | Unity IAP Initialize
AchievePurchase.Purchase        | 결제 시도
AchievePurchase.Confirm         | 결제 성공 후 완료 처리
AchievePurchase.GetPendingList  | 결제 완료 처리가 되지 않은 상품 리스트 획득
AchievePurchase.GetRestoreList  | Android, iOS 별 구매 복원 할 상품 리스트 획득
```

### 🪄 Initialize
Unity IAP의 초기화 과정을 비동기 방식으로 처리하여 완료 시점을 알 수 있도록 구현하였습니다.
초기화 이전에 스토어에 등록된 ProductId를 런타임에서 획득할 수 있는지 확인 부탁드립니다.

```
using com.Achieve.Purchase;
using Cysharp.Threading.Tasks;

private async void Start()
{
    // Array, List 모두 가능
    List<InitializeDto> dtoList = new List<InitializeDto>();

    // 스토어에 등록 된 Product의 Id와 상품 타입을 Dto에 등록하여 List에 추가합니다.    
    dtoList.Add(new InitializeDto
    {
        ProductId = "Consumable",
        ProductType = ProductType.Consumable
    });

    await AchievePurchase.InitializeAsync(dtoList);
}
```
### 💵 Purchase, Confirm
결제 시도부터 성공 및 실패 시점까지 사용자가 명확하게 판단할 수 있도록 인터페이스와 에러 메시지를 제공합니다.

```
public async void PurchaseAsync(string productId)
{
    PurchaseResult result = await AchievePurchase.PurchaseAsync(productId);

    // 성공 시
    if(result.IsSuccess)
    {
        // 서버를 통한 영수증 검증을 진행하신다면 이 시점에 추가해주세요. 

        // PurchaseResult, Product 모두 가능
        AchievePurchase.Confirm(result);

        // 아이템 지급

        return;
    }

    Debug.Log($"[{result.Product.definition.id}] 상품 구매에 실패했습니다. 이유 : {result.ErrorMessage}");
}
```

# 🔗 Dependencies
[In App Purchasing](https://docs.unity3d.com/Packages/com.unity.purchasing@4.12/manual/index.html) (4.12.0)
