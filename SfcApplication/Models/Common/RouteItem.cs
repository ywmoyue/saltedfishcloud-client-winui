using System;

namespace SfcApplication.Models.Common
{
    public class RouteItem
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public Object Query { get; set; }
        public Type PageType { get; set; }
    }
}
