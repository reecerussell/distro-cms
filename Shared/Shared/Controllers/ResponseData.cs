namespace Shared.Controllers
{
    public class ResponseData
    {
        public int StatusCode { get; set; }
        public object Data { get; set; }
        public string Error { get; set; }
    }

    public class ResponseData<T> where T : class
    {
        public int StatusCode { get; set; }
        public T Data { get; set; }
        public string Error { get; set; }
    }
}
