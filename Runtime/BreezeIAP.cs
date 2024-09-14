using System.Collections.Generic;
using UnityEngine.Purchasing;
using System.Threading.Tasks;
using System;
using UnityEngine;

namespace Achieve.BreezeIAP
{
    public class BreezeIAP
    {
        internal static IStoreController controller;
        internal static IExtensionProvider extensionProvider;

        internal static List<PurchaseResult> pendingList;
        internal static TaskCompletionSource<PurchaseResult> purchaseCompletionSource;
        internal static TaskCompletionSource<InitializeResult> initializeCompletionSource;

        internal static bool isCheckingPendingList;

        private static BreezeIAPReceiver _receiver;
        private static bool _isInitialized;

        /// <summary>
        /// 스토어에 등록 된 ProductID들을 기반으로 IAP를 Initialize합니다.
        /// </summary>
        /// <param name="dtos">IAP Initialize에 필요한 데이터</param>
        /// <param name="isDebug">Debug.Log를 찍을 것인지?</param>
        public static async Task InitializeAsync(InitializeDto[] dtos, bool isDebug = false)
        {
            if (_isInitialized) return;
            PurchaseLog.CurrentLogLevel = isDebug ? PurchaseLog.LogLevel.Debug : PurchaseLog.LogLevel.Info;

            _receiver = new BreezeIAPReceiver();
            pendingList = new List<PurchaseResult>();

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            for (int i = 0; i < dtos.Length; i++)
            {
                var dto = dtos[i];
                builder.AddProduct(dto.ProductId, dto.ProductType);
            }

            isCheckingPendingList = true;
            UnityPurchasing.Initialize(_receiver, builder);

            // 시간 내에 흐르지 못하면 false 처리
            var result = await initializeCompletionSource.Task.Timeout(TimeSpan.FromSeconds(10));
            _isInitialized = result.IsInitialized;

            if (_isInitialized is false)
            {
                PurchaseLog.Warning("Initialize failed: No response after Initialize");
                return;
            }

            PurchaseLog.Info("Initialize Successful!");
        }

        /// <summary>
        /// 스토어에 등록 된 ProductID들을 기반으로 IAP를 Initialize합니다.
        /// </summary>
        /// <param name="dtos">IAP Initialize에 필요한 데이터</param>
        /// <param name="isDebug">Debug.Log를 찍을 것인지?</param>
        public static async Task InitializeAsync(List<InitializeDto> dtos, bool isDebug = false)
        {
            if (_isInitialized) return;
            PurchaseLog.CurrentLogLevel = isDebug ? PurchaseLog.LogLevel.Debug : PurchaseLog.LogLevel.Info;

            var dtoArray = dtos.ToArray();

            dtos.Add(new InitializeDto
            {
                ProductId = "Consumable",
                ProductType = ProductType.Consumable
            });

            _receiver = new BreezeIAPReceiver();
            pendingList = new List<PurchaseResult>();

            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            for (int i = 0; i < dtoArray.Length; i++)
            {
                var dto = dtoArray[i];
                builder.AddProduct(dto.ProductId, dto.ProductType);
            }

            isCheckingPendingList = true;
            UnityPurchasing.Initialize(_receiver, builder);

            // 시간 내에 흐르지 못하면 false 처리
            var result = await initializeCompletionSource.Task.Timeout(TimeSpan.FromSeconds(10));
            _isInitialized = result.IsInitialized;

            if (_isInitialized is false)
            {
                PurchaseLog.Warning("Initialize failed: No response after Initialize");
                return;
            }

            PurchaseLog.Info("Initialize Successful!");
        }

        /// <summary>
        /// Initialize 후에 해당 메소드를 꼭 호출하여 Pending 상품에 대한 처리를 진행합니다.
        /// </summary>
        /// <returns></returns>
        public static List<PurchaseResult> GetPendingList()
        {
            if (_isInitialized)
            {
                PurchaseLog.Warning($"It is not initialized. Call method : {nameof(GetPendingList)}");
                return null;
            }

            PurchaseLog.Debug("Get PendingList...");
            isCheckingPendingList = false;
            return pendingList;
        }

        /// <summary>
        /// 스토어에 등록 된 ProductId를 입력하여 상품 구매를 시도합니다.
        /// 결제 결과는 AchievePurchaseReceiver에 등록된 event로 호출됩니다.
        /// 성공 : onPurchaseFailed
        /// 실패 : onPurchaseSuccess
        /// </summary>
        /// <param name="productId"></param>
        public static async Task<PurchaseResult> PurchaseAsync(string productId)
        {
            if (_isInitialized)
            {
                PurchaseLog.Warning($"It is not initialized. Call method : {nameof(PurchaseAsync)}");
                var result = PurchaseResult.Error("초기화 실패");
            }
            
            purchaseCompletionSource = new TaskCompletionSource<PurchaseResult>();
            
            PurchaseLog.Info($"Attempt to pay for product [{productId}]...");
            controller.InitiatePurchase(productId);

            // 60초 동안 설정되지 않으면... fail.
            var product = await purchaseCompletionSource.Task.Timeout(TimeSpan.FromSeconds(60));
            PurchaseLog.Info($"The payment for item [{productId}] was successful!");

            purchaseCompletionSource = null;

            return product;
        }

        /// <summary>
        /// Purchase로 결제 시도 후 onPurchaseSuccess로 event가 호출되었을 때
        /// 올바르게 구매가 진행되었다면 상품 구매를 확정합니다.
        /// 이 메소드를 호출한 후에 아이템을 지급해주세요.
        /// </summary>
        /// <param name="PurchaseResult"></param>
        public static void Confirm(PurchaseResult product)
        {
            controller.ConfirmPendingPurchase(product.Product);
            PurchaseLog.Info($"I confirmed product [{product.Product.definition.id}].");
        }
        

        /// <summary>
        /// Purchase로 결제 시도 후 onPurchaseSuccess로 event가 호출되었을 때
        /// 올바르게 구매가 진행되었다면 상품 구매를 확정합니다.
        /// 이 메소드를 호출한 후에 아이템을 지급해주세요.
        /// </summary>
        /// <param name="PurchaseResult"></param>
        public static void Confirm(Product product)
        {
            controller.ConfirmPendingPurchase(product);
            PurchaseLog.Info($"I confirmed product [{product.definition.id}].");
        }

        /// <summary>
        /// 구매한 상품을 복원합니다.
        /// 해당 메소드는 소모성 상품을 제외한 상품들을 불러옵니다.
        /// </summary>
        public static void Restore()
        {
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                var apple = extensionProvider.GetExtension<IAppleExtensions>();

                apple.RestoreTransactions((result, reason) =>
                {
                    PurchaseLog.Info(result ? "Restore successful!" : $"Restore failed...{reason}");
                });
            }
        }

        internal static void AddPendingList(PurchaseResult product)
        {
            if(isCheckingPendingList)
            {
                pendingList.Add(product);
            }
        }
    }
}