using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Encryption;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Bucket.DTOs;
using Amazon;

namespace Bucket.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : Controller
    {
        [HttpGet("Allfiles")]
        public async Task<IActionResult> GetAll([FromBody]BucketCredsDto bucketCredsDto)
        {
            try
            {
                var credentials = new BasicAWSCredentials(bucketCredsDto.AccessKey, bucketCredsDto.SecretKey);

                var regionEndPoint = RegionEndpoint.EnumerableAllRegions.Where(x => x.OriginalSystemName == bucketCredsDto.Region).FirstOrDefault();

                var config = new AmazonS3Config
                {
                    RegionEndpoint = regionEndPoint
                };

                AmazonS3Client client = new AmazonS3Client(credentials, config);

                ListObjectsRequest listRequest = new ListObjectsRequest
                {
                    BucketName = bucketCredsDto.BucketName
                };

                ListObjectsResponse listResponse;

                listResponse = await client.ListObjectsAsync(listRequest);

                return Ok(listResponse.S3Objects);
            }
            catch
            {
                return BadRequest("Something Was Wrong");
            }
        }
        
        
        [HttpGet]
        public async Task<IActionResult> GetAllEndPointRegion()
        {
            return Ok(RegionEndpoint.EnumerableAllRegions);
        }


        [HttpPost("DownloadFile")]
        public async Task<MemoryStream> DownloadFile([FromBody] BucketCredsDto bucketCredsDto,string fileKey)
        {
            try
            {
                var credentials = new BasicAWSCredentials(bucketCredsDto.AccessKey, bucketCredsDto.SecretKey);

                var regionEndPoint = RegionEndpoint.EnumerableAllRegions.Where(x => x.OriginalSystemName == bucketCredsDto.Region).FirstOrDefault();

                var config = new AmazonS3Config
                {
                    RegionEndpoint = regionEndPoint
                };

                AmazonS3Client client = new AmazonS3Client(credentials, config);

                GetObjectRequest listRequest = new GetObjectRequest
                {
                    BucketName = bucketCredsDto.BucketName,
                    Key = fileKey
                    
                };

                using (GetObjectResponse response = await client.GetObjectAsync(listRequest))

                using (Stream responseStream = response.ResponseStream)

                using (StreamReader reader = new StreamReader(responseStream))
                {
                    var memory = new MemoryStream();

                    await responseStream.CopyToAsync(memory);

                    memory.Position = 0;

                    return memory;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
