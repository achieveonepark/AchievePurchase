namespace com.achieve.scripting.purchase

{
    public readonly struct InitializeResult
    {
        /// <summary>
        /// 상품의 현재 상태
        /// </summary>
        public bool IsInitialized { get; init; }

        /// <summary>
        /// 에러 상세 메시지
        /// </summary>
        public string ErrorMessage { get; init; }

        /// <summary>
        /// Success == true => 결제 성공 
        /// </summary>
        public bool IsSuccess => string.IsNullOrEmpty(ErrorMessage);

        public static InitializeResult Success()
        {
            return new InitializeResult
            {
                IsInitialized = true,
                ErrorMessage = string.Empty
            };
        }

        public static InitializeResult Error(string message)
        {
            return new InitializeResult
            {
                IsInitialized = false,
                ErrorMessage = message
            };
        }
    }
}