using BM.Social.ApplicationService.SocialModule.Abtracts;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using BM.Shared.ApplicationService;

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

        public async Task DeleteImageAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                _logger.LogWarning("Image URL is null or empty");
                throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));
            }

            try
            {
                // Extract public ID from URL
                var publicId = ExtractPublicIdFromUrl(imageUrl);
                if (string.IsNullOrEmpty(publicId))
                {
                    _logger.LogWarning("Could not extract public ID from URL: {Url}", imageUrl);
                    throw new ArgumentException("Invalid Cloudinary image URL", nameof(imageUrl));
                }

                _logger.LogInformation("Deleting image with public ID: {PublicId}", publicId);

                var deletionParams = new DeletionParams(publicId)
                {
                    ResourceType = ResourceType.Image
                };

                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);
                if (deletionResult == null || deletionResult.Result == null)
                {
                    _logger.LogError("Cloudinary deletion returned null result for public ID: {PublicId}", publicId);
                    throw new InvalidOperationException("Failed to delete image from Cloudinary");
                }

                if (deletionResult.Result.ToLower() != "ok")
                {
                    _logger.LogError("Cloudinary deletion failed for public ID: {PublicId}. Result: {Result}, Error: {Error}",
                        publicId, deletionResult.Result, deletionResult.Error?.Message);
                    throw new InvalidOperationException($"Failed to delete image: {deletionResult.Error?.Message ?? deletionResult.Result}");
                }

                _logger.LogInformation("Image deleted successfully. Public ID: {PublicId}", publicId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete image from Cloudinary for URL: {Url}", imageUrl);
                throw new InvalidOperationException("Failed to delete image from Cloudinary", ex);
            }
        }

        private string ExtractPublicIdFromUrl(string imageUrl)
        {
            try
            {
                // Example URL: https://res.cloudinary.com/{cloud_name}/image/upload/v1234567890/{public_id}.jpg
                var uri = new Uri(imageUrl);
                var path = uri.AbsolutePath;

                // Remove the "/image/upload/" and version prefix (e.g., "v1234567890/")
                var regex = new Regex(@"/image/upload/(v\d+/)?(.+)");
                var match = regex.Match(path);
                if (!match.Success)
                {
                    return null;
                }

                var publicId = match.Groups[2].Value;
                // Remove file extension (e.g., ".jpg")
                publicId = Regex.Replace(publicId, @"\.\w+$", "");
                return publicId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to extract public ID from URL: {Url}", imageUrl);
                return null;
            }
        }
    }
}