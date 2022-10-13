
namespace SfcApplication.Models.Beans
{
    public class BaseBean
    {
        public string Msg { get; set; }
        public int Code { get; set; }
    }

    public class BaseBean<T> : BaseBean
    {
        public T Data { get; set; }
    }
}
