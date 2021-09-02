using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ZiptoCity.Models;
using System.Data.SQLite;
using System.Web.Http;
using System.Net;

namespace ZiptoCity
{
    public static class ZipCodeLookup
    {
        [FunctionName("ZipCodeLookup")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "city/{id}")] HttpRequest req,
            ILogger log, int id)
        {
            log.LogInformation("Requested city for " + id);

            var sqlite_conn = new SQLiteConnection(@"Data Source=C:\home\site\wwwroot\Data\zipcode.db; Version = 3; New = True; Compress = True; ");

            try
            {
                sqlite_conn.Open();
            }
            catch (Exception ex)
            {
                log.LogInformation("Error: " + ex.Message);
            }

            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = "SELECT zip, primaryCity, state, county, timezone FROM zip_code_database WHERE zip = '" + id + "'";

            sqlite_datareader = sqlite_cmd.ExecuteReader();

            var resultCity = new City { };
            var goodResult = false;

            if (sqlite_datareader.HasRows)
            {
                goodResult = true;

                while (sqlite_datareader.Read())
                {
                    resultCity.ZipCode = sqlite_datareader.GetInt32(0);
                    resultCity.CityName = sqlite_datareader.GetString(1);
                    resultCity.State = sqlite_datareader.GetString(2);
                    resultCity.County = sqlite_datareader.GetString(3);
                    resultCity.TimeZone = sqlite_datareader.GetString(4);
                }
            }
            sqlite_conn.Close();

            if (goodResult)
            {                
                return new OkObjectResult(resultCity);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
