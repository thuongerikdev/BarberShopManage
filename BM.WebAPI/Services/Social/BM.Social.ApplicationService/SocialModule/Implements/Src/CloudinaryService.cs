using BM.Social.ApplicationService.SocialModule.Abtracts;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BM.Social.ApplicationService.SocialModule.Implements.Src
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;
        private readonly ILogger<CloudinaryService> _logger;

        public CloudinaryService(Cloudinary cloudinary, ILogger<CloudinaryService> logger)
        {
            _cloudinary = cloudinary ?? throw new ArgumentNullException(nameof(cloudinary));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> UploadImageAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("File is null or empty");
                return null;
            }

            try
            {
                _logger.LogInformation("Uploading file {FileName} with size {FileSize}", file.FileName, file.Length);
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fit")
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                if (uploadResult == null)
                {
                    _logger.LogError("Cloudinary returned null result");
                    return null;
                }

                if (uploadResult.SecureUrl == null)
                {
                    _logger.LogError("Cloudinary upload failed. Status: {Status}, Error: {Error}", uploadResult.StatusCode, uploadResult.Error?.Message);
                    return null;
                }

                _logger.LogInformation("Image uploaded successfully. URL: {Url}", uploadResult.SecureUrl);
                return uploadResult.SecureUrl.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload image to Cloudinary");
                throw new InvalidOperationException("Failed to upload image to Cloudinary", ex);
            }
        }
    }
}
