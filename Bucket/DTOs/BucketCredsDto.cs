using Amazon;
using System.ComponentModel.DataAnnotations;

namespace Bucket.DTOs
{
    public class BucketCredsDto
    {
        [Required]
        public string SecretKey { get; set; }
        [Required]
        public string AccessKey { get; set; }

        public string Region { get; set; }
        public string BucketName { get; set; }
    }
}
