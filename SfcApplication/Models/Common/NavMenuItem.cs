
namespace SfcApplication.Models.Common
{
    public class NavMenuItem
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; }
        public string Path { get; set; }

        public bool Hidden { get; set; } = false;
        public NavMenuItem() { }
        public NavMenuItem(string name, string path, int id = 0,bool hidden=false)
        {
            Name = name;
            Path = path;
            Id = id;
            Hidden = hidden;
        }

        public NavMenuItem(string name)
        {
            Name = name;
        }
    }
}
