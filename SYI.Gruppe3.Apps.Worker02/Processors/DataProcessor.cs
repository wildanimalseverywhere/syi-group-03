using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CsvHelper;
using CsvHelper.Configuration;
using SYI.Gruppe3.Apps.Shared.Library.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Formats.Asn1;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SYI.Gruppe3.Apps.Worker02.Processors
{
    public class DataProcessor
    {
        private readonly string _containerName;
        private readonly string _fileName;
        private string SQL_CONNECTION = Environment.GetEnvironmentVariable("SQL_CONNECTION");
        private string BLOB_CONNECTION = Environment.GetEnvironmentVariable("BLOB_CONNECTION");
        private string INSERT_STATEMENT = @"
        INSERT INTO DATA_SOURCE (
            Borough, 
            ContributingFactorVehicle1, 
            ContributingFactorVehicle2, 
            CrashDate, 
            CrossStreetName, 
            Location, 
            OnStreetName, 
            VehicleTypeCode1, 
            VehicleTypeCode2, 
            ZipCode, 
            Hour, 
            Latitude, 
            Longitude, 
            Minute, 
            NumberOfPersonsInjured, 
            NumberOfPersonsKilled
        ) VALUES (
            @Borough, 
            @ContributingFactorVehicle1, 
            @ContributingFactorVehicle2, 
            @CrashDate, 
            @CrossStreetName, 
            @Location, 
            @OnStreetName, 
            @VehicleTypeCode1, 
            @VehicleTypeCode2, 
            @ZipCode, 
            @Hour, 
            @Latitude, 
            @Longitude, 
            @Minute, 
            @NumberOfPersonsInjured, 
            @NumberOfPersonsKilled
        )";

        public DataProcessor(string container, string fileName)
        {
            _containerName = container;
            _fileName = fileName;
        }

        private async Task<string> DownloadFile()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(BLOB_CONNECTION);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);
            var blobClient = blobContainerClient.GetBlobClient(_fileName);
            var download = await blobClient.DownloadStreamingAsync();

            var path = Path.GetTempFileName();

            using (var memoryStream = new MemoryStream())
            {
                await blobClient.DownloadToAsync(memoryStream);
                await File.WriteAllBytesAsync(path, memoryStream.ToArray());
            }
            return path;
        }



        public async Task<bool> ProcessBlobStorageUrl(ILogger logger)
        {
            logger.LogInformation("downloading file....");
            var csvPath = await DownloadFile();
            logger.LogInformation($"path: {csvPath}");

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                Delimiter = ",",
                Encoding = System.Text.Encoding.UTF8, 
               
            };

            logger.LogInformation($"opening csv file...");
            using (var reader = new StreamReader(csvPath))
            {
                using (var csv = new CsvReader(reader, config))
                {
                    var sourceList = csv.GetRecords<SourceDataItem>();
                    //Console.WriteLine($"items to insert: {sourceList.Count()}");
                    int index = 0;
                    using (SqlConnection connection = new SqlConnection(SQL_CONNECTION))
                    {

                        logger.LogInformation($"opening sql connection...");
                        await connection.OpenAsync();
                        foreach (var row in sourceList)
                        {
                            using (SqlCommand command = new SqlCommand(INSERT_STATEMENT, connection))
                            {
                                command.Parameters.AddWithValue("@Borough", row.Borough);
                                command.Parameters.AddWithValue("@ContributingFactorVehicle1", row.ContributingFactorVehicle1);
                                command.Parameters.AddWithValue("@ContributingFactorVehicle2", row.ContributingFactorVehicle2);
                                command.Parameters.AddWithValue("@CrashDate", row.CrashDate);
                                command.Parameters.AddWithValue("@CrossStreetName", row.CrossStreetName);
                                command.Parameters.AddWithValue("@Location", row.Location);
                                command.Parameters.AddWithValue("@OnStreetName", row.OnStreetName);
                                command.Parameters.AddWithValue("@VehicleTypeCode1", row.VehicleTypeCode1);
                                command.Parameters.AddWithValue("@VehicleTypeCode2", row.VehicleTypeCode2);
                                command.Parameters.AddWithValue("@ZipCode", row.ZipCode);
                                command.Parameters.AddWithValue("@Hour", row.Hour);
                                command.Parameters.AddWithValue("@Latitude", row.Latitude);
                                command.Parameters.AddWithValue("@Longitude", row.Longitude);
                                command.Parameters.AddWithValue("@Minute", row.Minute);
                                command.Parameters.AddWithValue("@NumberOfPersonsInjured", row.NumberOfPersonsInjured);
                                command.Parameters.AddWithValue("@NumberOfPersonsKilled", row.NumberOfPersonsKilled);
                                await command.ExecuteNonQueryAsync();
                            }
                            index++;
                            logger.LogInformation($"{index} rows totally inserted");
                        }
                    }
                }
            }


            logger.LogInformation($"data insertion completed");

            File.Delete(csvPath);
            return true; 
        }

    }
}
