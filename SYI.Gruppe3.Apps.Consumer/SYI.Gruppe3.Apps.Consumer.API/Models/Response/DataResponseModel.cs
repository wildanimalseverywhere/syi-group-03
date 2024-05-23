

namespace SYI.Gruppe3.Apps.Consumer.API.Models.Response
{
    public class DataResponseModel
    {
        public string RenderedSQLQuery { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !String.IsNullOrEmpty(ErrorMessage);
        public List<DataResponseItem> Items { get; set; }
    }

    public class DataResponseItem
    {
        public string Borough { get; set; }
        public int NumberOfPersonsInjured { get; set; }
        public int NumberOfPersonsKilled { get; set; }
        public int Year { get; set; }
    }

}
