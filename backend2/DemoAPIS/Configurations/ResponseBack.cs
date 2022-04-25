namespace DemoAPIS.Configurations
{
    
    public class ResponseBack<T>
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

    }

}
