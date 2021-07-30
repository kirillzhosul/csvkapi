#region Usings.

// System.
using System.Collections.Generic;

// JSON.
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

namespace vkapi.methods
{
    public class Uploader : Methods
    {
        /// Uploader class that works with files uploading.

        #region Uploader.

        /// <summary>
        /// Uploads photo and returns it.
        /// </summary>
        /// <param name="api">VkApi Object</param>
        /// <param name="albumId">Album ID for uploading</param>
        /// <param name="file">File to upload</param>
        /// <returns></returns>
        public static Dictionary<string, dynamic> UploadPhoto(VkApi api, long albumId, string file)
        {
            // Server JSON result.
            string serverJSON = GetPhotosUploadServer(api, albumId);

            // JSON Parsing.
            json.JsonUploadServerPhotosResponse serverResponse = JsonConvert.DeserializeObject<json.JsonUploadServerPhotosResponse>(serverJSON);

            // Getting server URL.
            string uploadURL = serverResponse.Response.UploadUrl;

            // Uploading.
            string uploadString = utils.Url.PostUploadFile(uploadURL, file);

            // Parsing upload
            JObject uploadJSON = JsonConvert.DeserializeObject<JObject>(uploadString);

            // Getting save information.
            long saveServer = (long)(uploadJSON.GetValue("server"));
            string saveHash = (string)(uploadJSON.GetValue("hash"));
            string savePhotosList = (string)(uploadJSON.GetValue("photos_list"));

            // Saving photo.
            string uploadedPhoto = SavePhoto(api, albumId, saveServer, savePhotosList, saveHash);

            // Getting response.
            JToken response = JsonConvert.DeserializeObject<JObject>(uploadedPhoto).GetValue("response");

            // Getting object.
            object responseObject = response[0].ToObject(typeof(Dictionary<string, dynamic>));

            // Returning photo.
            return (Dictionary<string, dynamic>)responseObject;
        }

        /// <summary>
        /// Gets photo attachment by photo object.
        /// </summary>
        /// <param name="photo">Photo object from UploadPhoto()</param>
        /// <returns>Attachment</returns>
        public static string GetPhotoAttachment(Dictionary<string, dynamic> photo)
        {
            // Returning photo.
            return $"photo{photo["owner_id"]}_{photo["id"]}";
        }

        #endregion

        #region Other.

        /// <summary>
        /// Saves photo to album
        /// </summary>
        /// <param name="api">API Object</param>
        /// <param name="albumId">Index of the album to save</param>
        /// <param name="server">Server index from upload result</param>
        /// <param name="photosList">Photos list from upload result</param>
        /// <param name="hash">Hash from upload result</param>
        /// <returns></returns>
        private static string SavePhoto(VkApi api, long albumId, long server, string photosList, string hash)
        {
            // Returning.
            return api.APIMethod("photos.save", $"album_id={albumId}&server={server}&photos_list={photosList}&hash={hash}");
        }

        #endregion

        #region Getting upload server.

        /// <summary>
        /// Calls photos get upload server method.
        /// </summary>
        /// <param name="api">API Object</param>
        /// <param name="albumId">Index of the album to upload</param>
        /// <returns></returns>
        private static string GetPhotosUploadServer(VkApi api, long albumId)
        {
            // Returning.
            return api.APIMethod("photos.getUploadServer", $"album_id={albumId}");
        }

        #endregion
    }
}