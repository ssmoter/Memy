namespace Memy.Shared.Model
{
    public class FileUploadStatus
    {
        public string? ObjName { get; set; }
        public int ObjTyp { get; set; }
        public int ObjOrder { get; set; }
        public byte[]? Data { get; set; }
        public string? ImgUrl { get; set; }
    }
    public class FileUploadModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string[]? Tag { get; set; }
        public string? Categories { get; set; }
        public FileUploadStatus[]? FileUploadStatuses { get; set; }
    }

}
