using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SfcApplication.Models.Common
{
    internal class NavMenuItem
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public Type PageType { get; set; }
        public NavMenuItem() { }
        public NavMenuItem(string name, Type pageType, int id = 0)
        {
            Name = name;
            PageType = pageType;
            Id = id;
        }

        public NavMenuItem(string name)
        {
            Name = name;
        }
    }
}
