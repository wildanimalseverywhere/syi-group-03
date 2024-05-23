using Microsoft.AspNetCore.Mvc;
using SYI.Gruppe3.Apps.Consumer.API.Models.Response;
using SYI.Gruppe3.Apps.Shared.Library.Models;
using System.Data.SqlClient;

namespace SYI.Gruppe3.Apps.Consumer.API.Controllers
{
    [ApiController]
    [Route("api/data")]
    public class DataController : ControllerBase
    {

        private string SQL_CONNECTION = Environment.GetEnvironmentVariable("SQL_CONNECTION");
        private readonly ILogger<DataController> _logger;

        public DataController(ILogger<DataController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("query")]
        public async Task<object> Get([FromQuery] string borough = "", [FromQuery] int? yearFrom = null, [FromQuery] int? yearTo = null)
        {
            string query = $@"
 SELECT 
                YEAR(CrashDate) AS Year,
                Borough,
                SUM(NumberOfPersonsInjured) AS NumberOfPersonsInjured,
                SUM(NumberOfPersonsKilled) AS NumberOfPersonsKilled
            FROM DATA_SOURCE";

            bool hasWhere = false;
            if (!String.IsNullOrEmpty(borough))
            {
                query = query + " WHERE Borough = @Borough ";
                hasWhere = true;
            }
            if (yearFrom != null || yearTo != null)
            {
                if (!hasWhere)
                {
                    query = query + " WHERE ";
                }
                else
                {
                    query = query + " AND ";
                }
                if (yearFrom != null && yearTo != null)
                    query = query + "  YEAR(CrashDate) BETWEEN @YearFrom AND @YearTo ";
                else if (yearFrom != null && yearTo == null)
                    query = query + "  YEAR(CrashDate) >= @YearFrom ";
                else if (yearTo != null && yearFrom == null)
                    query = query + "  YEAR(CrashDate) <= @YearTo ";
            }

            query = query + "    GROUP BY YEAR(CrashDate), Borough ORDER BY Year, Borough ";



            DataResponseModel result = new DataResponseModel()
            {
                Items = new List<DataResponseItem>(),
                RenderedSQLQuery = query,
            };


            //Load Data from SQL Server
            using (SqlConnection connection = new SqlConnection(SQL_CONNECTION))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    if (!string.IsNullOrEmpty(borough))
                        command.Parameters.AddWithValue("@Borough", borough);
                    if (yearFrom != null)
                        command.Parameters.AddWithValue("@YearFrom", yearFrom.Value);
                    if (yearTo != null)
                        command.Parameters.AddWithValue("@YearTo", yearTo.Value);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var dataSourceDto = new DataResponseItem
                            {
                                Year = reader["Year"] as int? ?? default,
                                Borough = reader["Borough"] as string,
                                NumberOfPersonsInjured = reader["NumberOfPersonsInjured"] as int? ?? default,
                                NumberOfPersonsKilled = reader["NumberOfPersonsKilled"] as int? ?? default
                            };
                            result.Items.Add(dataSourceDto);
                        }
                    }
                }
            }

            return result;
        }
    }
}
