namespace Asakabank.Base {
    public class BaseResponse {
        /// <summary>
        /// Код ошибки
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// Текст ошибки
        /// </summary>
        public string Message { get; set; }
    }
}