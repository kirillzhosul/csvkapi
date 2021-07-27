#region Usings.

// System.
using System.IO;
using System.Net;

#endregion

namespace vkapi
{
    public class VkApi
    {
        /// Default VK API class,
        /// Implements base for VK, used as parent for Longpoll.
        /// You may use this as default VK API wrapper.

        #region Fields, Constants.

        // Version for API requests.
        // THIS IS DEFAULT VALUE.
        // You may change this via -> ChangeVersion(version) method!
        protected string _version = "5.131";

        // Access token.
        // Should be not changed except only  in constructor.
        protected readonly string _accessToken;

        // Default GET parameters for method url.
        // Generated in method -> GenerateDefaultParameters(),
        // Which is virtual, so this generationg is overriden in subclasses.
        protected string _urlDefaultParameters;

        // Constants.

        // URL to API method link.
        // IDK why you want to change this, but... don`t.
        public const string _urlMethod = "https://api.vk.com/method";

        #endregion

        #region Methods.

        #region Constructor.

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="accessToken">Access token for VK API</param>
        public VkApi(string accessToken)
        {
            // Setting access token.
            _accessToken = accessToken;

            // Generating new default parameters.
            APIGenerateDefaultParameters();
        }

        #endregion

        #region API.

        #region APIMethod, oveloads.

        /// <summary>
        /// Calls API method.
        /// </summary>
        /// <param name="name">Method name from VK docs</param>
        /// <param name="arguments">Arguments as string separated by & (Example: message=test&peer_id=1125)</param>
        /// <returns>Response</returns>
        public string APIMethod(string name, string arguments)
        {
            // Returning response.
            return UrlGet($"{_urlMethod}/{name}?{_urlDefaultParameters}&{arguments}");
        }

        /// <summary>
        /// APIMethod() overload, Calls API method with no arguments.
        /// </summary>
        /// <param name="name">Method name from VK docs</param>
        /// <returns>Response</returns>
        public string APIMethod(string name)
        {
            // Returning response.
            return APIMethod(name, "");
        }

        /// <summary>
        /// APIMethod() overload, Calls API method with string array arguments.
        /// </summary>
        /// <param name="name">Method name from VK docs</param>
        /// <returns>Response</returns>
        public string APIMethod(string name, string[] arguments)
        {
            // Returning response.
            return APIMethod(name, string.Join("&", arguments));
        }

        #endregion

        /// <summary>
        /// Changes API version. Also call default parameters generation.
        /// </summary>
        /// <param name="version">Version to change</param>
        public void APIChangeVersion(string version)
        {
            // Change version.
            _version = version;

            // Generating new default params.
            APIGenerateDefaultParameters();
        }

        /// <summary>
        /// Generates default GET requests for API calls.
        /// </summary>
        protected virtual void APIGenerateDefaultParameters()
        {
            // Generating default parameters.
            _urlDefaultParameters = $"v={_version}&access_token={_accessToken}";
        }

        #endregion

        #region Other.

        /// <summary>
        /// Gets URL boy and returns it as string.
        /// </summary>
        /// <param name="url">URL for getting it</param>
        /// <returns>Response</returns>
        protected static string UrlGet(string url)
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

        #endregion

        #endregion
    }
}
