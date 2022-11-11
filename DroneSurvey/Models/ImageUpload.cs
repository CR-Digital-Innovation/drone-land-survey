namespace DroneSurvey.Models
{
    public class ImageUploadHistory
    {
        public string? BatchUid { get; set; }
        public string? Location { get; set; }
        public string? UploadTime { get; set; }
        public string? UploadStatus { get; set; }
        public string? ContainerUrl { get; set; }
        public int? ImageUploadCount { get; set; }
        public string? OrthomosaicStatus { get; set; }
        public string? QCStatus { get; set; }
        public string? PolygonStatus { get; set; }
        public string? PolygonAcceptanceStatus{ get; set; }
    }

    public class ImageUpload
    {
        public string? DirPath { get; set; }
        public string? Village { get; set; }
        public string? LoggedUser { get; set; }
    }

    public class VieImageModel
    {
        public string? FilePath { get; set; }
        public string? FileName { get; set; }
    }
}
