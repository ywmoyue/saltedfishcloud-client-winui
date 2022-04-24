using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Beans
{
    internal class BaseBean
    {
        public string Msg { get; set; }
        public int Code { get; set; }
    }

    internal class BaseBean<T> : BaseBean
    {
        public T Data { get; set; }
    }
}
