namespace CSharp_Json
{
    public class RetornoAPIViewModel<T>
    {
        public bool Success { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public string Content { get; set; }
    }
}
