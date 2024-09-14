using UnityEngine.Purchasing;

namespace Achieve.BreezeIAP
{
    public readonly struct InitializeDto
    {
        public string ProductId { get; init; }
        public ProductType ProductType { get; init; }
    }
}