using vkapi;

namespace example
{
    class Example
    {
        public static void Main(string[] args)
        {
            // Creating api.
            VkApiBotLongpoll api = new VkApiBotLongpoll("TOKEN", 1); // TOKEN + ID.

            // Starting listening.
            api.ListenLongpoll();
        }
    }
}