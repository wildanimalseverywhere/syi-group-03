

namespace SYI.Gruppe3.Apps.Consumer.API.Models.Response
{
    public class DataResponseModel
    {
        public string RenderedSQLQuery { get; set; }
        public string ErrorMessage { get; set; }
        public bool HasError => !String.IsNullOrEmpty(ErrorMessage);
        public List<DataResponseItem> Items { get; set; }
        public DataResponseModel FinalizeResult()
        {
            if (Items == null || Items.Count == 0)
                return this;
            var lowestYear = Items.Min(x => x.Year);
            var highestYear = Items.Max(x => x.Year);
            var list = Enumerable.Range(lowestYear, highestYear - lowestYear).ToList();
            var affectedBoroughs = Items.GroupBy(t => t.Borough)
                .Select(f => new
                {
                    borough = f.Key,
                    years = f.GroupBy(o => o.Year).Select(v => new
                    {
                        year = v.Key,
                        amount = v.Count()
                    })
                });
            foreach (var borrough in affectedBoroughs)
            {
                var availableYears = borrough.years.Select(t => t.year).ToList();
                var missingYears = list.Where(t => availableYears.Contains(t) == false).ToList();
                missingYears.ForEach(g =>
                {
                    Items.Add(new DataResponseItem()
                    {
                        Borough = borrough.borough,
                        NumberOfPersonsInjured = 0,
                        NumberOfPersonsKilled = 0,
                        Year = g
                    });
                });
            }
            Items = Items.OrderBy(t => t.Year).ThenBy(t => t.Borough).ToList();
            return this;
        }
    }

    public class DataResponseItem
    {
        public string Borough { get; set; }
        public int NumberOfPersonsInjured { get; set; }
        public int NumberOfPersonsKilled { get; set; }
        public int Year { get; set; }
        public int Count => NumberOfPersonsInjured + NumberOfPersonsKilled;
    }

}
