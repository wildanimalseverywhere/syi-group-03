using SYI.Gruppe3.Apps.Consumer.API.Models.Data;

namespace SYI.Gruppe3.Apps.Consumer.API.Models.Response
{
    public class DataResponseModel
    {
        public string RenderedSQLQuery { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !String.IsNullOrEmpty(ErrorMessage);
        public List<DataItem> Items { get; set; }
    }

}
