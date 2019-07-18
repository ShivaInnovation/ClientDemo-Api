using System.Net.Http.Formatting;

namespace Getix_Admin_Api
{
    internal class JsonpMediaTypeFormatter
    {
        private JsonMediaTypeFormatter jsonFormatter;

        public JsonpMediaTypeFormatter(JsonMediaTypeFormatter jsonFormatter)
        {
            this.jsonFormatter = jsonFormatter;
        }
    }
}