using Azure.Storage.Blobs;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using RMotownFestival.Api.Common;

using System;
using System.Linq;

namespace RMotownFestival.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PicturesController : ControllerBase
    {
        public BlobUtility BlobUtility { get; }
        public PicturesController(BlobUtility blobUtility)
        {
            BlobUtility = blobUtility;
        }
        [HttpGet]
        public string[] GetAllPictureUrls()
        {

            var container = BlobUtility.GetPicturesContainer();

            return container.GetBlobs()
                .Select(blob => BlobUtility.GetSasUri(container,blob.Name))
                .ToArray();
           // return Array.Empty<string>();
        }

        [HttpPost]
        public void PostPicture(IFormFile file)
        {
            BlobContainerClient container = BlobUtility.GetPicturesContainer();
            container.UploadBlob (file.FileName ,file.OpenReadStream( ));
        }
    }
}
