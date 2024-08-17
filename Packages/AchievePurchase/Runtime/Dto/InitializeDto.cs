
using UnityEngine.Purchasing;

namespace com.achieve.purchase
{
    public readonly struct InitializeDto
    {
        public string ProductId { get; init; }
        public ProductType ProductType { get; init; }
    }
}