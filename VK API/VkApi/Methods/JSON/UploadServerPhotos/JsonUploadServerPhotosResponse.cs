#region Usings.

// JSON Parsing.
using Newtonsoft.Json;

#endregion

namespace vkapi.methods.json
{
    public class JsonUploadServerPhotosResponse
    {
        /// Contains photos upload server response container.
        /// Holds response object.

        // Server response.
        [JsonProperty("response")]
        public JsonUploadServerPhotos Response { get; set; }
    }
}