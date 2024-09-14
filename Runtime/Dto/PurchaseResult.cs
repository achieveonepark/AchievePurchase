using UnityEngine.Purchasing;

namespace Achieve.BreezeIAP
{
    public readonly struct PurchaseResult
    {
        /// <summary>
        /// 상품의 현재 상태
        /// </summary>
        public PurchaseType Type { get; init; }

        /// <summary>
        /// 시도 한 상품
        /// </summary>
        public Product Product { get; init; }

        /// <summary>
        /// 에러 상세 메시지
        /// </summary>
        public string ErrorMessage { get; init; }

        /// <summary>
        /// Success == true => 결제 성공 
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public static PurchaseResult Error(string message)
        {
            return new PurchaseResult
            {
                Type = PurchaseType.Error,
                Product = null,
                ErrorMessage = message
            };
        }
    }
}