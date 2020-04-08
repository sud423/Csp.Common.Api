using Csp.EF;

namespace Csp.Upload.Api.Models
{
    public class FileModel : Entity
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Ext { get; set; }

        public double FileSize { get; set; }

        public string FilePath { get; set; }

        public string ContentType { get; set; }

        public int UserId { get; set; }

        public int TenantId { get; set; }


    }
}
