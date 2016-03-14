using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Famoser.FrameworkEssentials.Logging;

namespace Famoser.ETHZMensa.Data.Services
{
    public class DataService : IDataService
    {
        public async Task<string> GetHtml(Uri url)
        {
            try
            {
                using (var client = new HttpClient(
                    new HttpClientHandler
                    {
                        AutomaticDecompression = DecompressionMethods.GZip
                                                 | DecompressionMethods.Deflate
                    }))
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    var resp = await client.GetAsync(url);
                    var res = await resp.Content.ReadAsStringAsync();
                    if (resp.IsSuccessStatusCode)
                        return res;
                    LogHelper.Instance.Log(LogLevel.Error, this, "Request not successfull: Status Code " + resp.StatusCode + " returned.");
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.Instance.LogException(ex);
            }
            return null;
        }
    }
}
