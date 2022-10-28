using System.Runtime.Serialization;

namespace SfcApplication.Models.Requests
{
    [DataContract]
    public class RenameFileRequest
    {
        [DataMember(Name="oldName")]
        public string OldName { get; set; }
        [DataMember(Name = "newName")]
        public string NewName { get; set; }
    }
}
