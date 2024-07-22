using System.ComponentModel.DataAnnotations;

namespace Blazor_ApiStatusCheck.Connection
{
    public class ApiHealthCheck
    {
        [Key]
        public string Api__Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public DateTime? LastChecked { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

