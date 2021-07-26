namespace vkapi_examples
{
    class main
    {
        public static void Main(string[] args)
        {
            // Your connectiong info.
            string accessToken = "";
            int groupIndex = 0;

            // New echo bot.
            new EchoBot(accessToken, groupIndex);
        }
    }
}
