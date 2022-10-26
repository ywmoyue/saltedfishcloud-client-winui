using System.Collections.Generic;

namespace SfcApplication.Models.Requests
{
    public class MoveFileRequest
    {
        public int TargetUid { get; set; }
        public int SourceUid { get; set; }
        public List<MoveFileRequestItem> Files { get; set; }
    }

    public class MoveFileRequestItem
    {
        public string Source { get; set; }
        public string Target { get; set; } = "/";
    }
}
