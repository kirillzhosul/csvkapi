#region Usings.

// System.
using System.IO;
using System.Net;
using System.Text;

#endregion

namespace vkapi
{
    namespace utils
    {
        /// Utils namespace. Includes working with some utils.

        class Url
        {
            /// Class that works with URLs.

            /// <summary>
            /// Gets URL boy and returns it as string.
            /// </summary>
            /// <param name="url">URL for getting it</param>
            /// <returns>Response</returns>
            public static string Get(string url)
            {
                // Getting request / response.
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();

                // Downloading and returning.
                using (StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream()))
                {
                    // Returning.
                    return streamReader.ReadToEnd();
                }
            }

            /// <summary>
            /// Uploads file using post.
            /// </summary>
            /// <param name="url">URL for uploading</param>
            /// <param name="file">File for uploading.</param>
            /// <returns></returns>
            public static string PostUploadFile(string url, string file)
            {
                // Returning.
                return Encoding.UTF8.GetString(new WebClient().UploadFile(url, file));
            }
        }
    }
}
