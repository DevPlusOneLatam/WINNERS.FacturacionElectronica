namespace Model.Responses
{
    public class OperacionResponse
    {
        public bool IsSuccess { get; set; }
        public string Contenido { get; set; }
        public string JsonString { get; set; }
    }

    public class OperacionDataResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Contenido { get; set; }
        public string JsonString { get; set; }
    }
}
