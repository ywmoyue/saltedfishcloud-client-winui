
namespace SfcApplication.Models.Configs
{
    public class ClientConfig
    {
        public string AppName { get; set; }
        public string BaseUrl { get; set; }
        public OpenApiConfig OpenApi { get; set; }
        public string ConfigPath { get; set; } = "";
    }
}
