#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.methods.json
{
    public class JsonUploadServerPhotos
    {
        /// Contains main photos upload server response.
        /// Holds server parameters: URL, AID, UID.

        // Key for request.
        [JsonProperty("upload_url")]
        public string UploadUrl { get; set; }

        // Key for request.
        [JsonProperty("album_id")]
        public long AlbumId { get; set; }

        // Key for request.
        [JsonProperty("user_id")]
        public long UserId { get; set; }
    }
}