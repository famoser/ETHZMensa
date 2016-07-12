using System;
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


                    if (url.AbsoluteUri.Contains("http://www.mensa.uzh.ch/menueplaene"))
                    {
                        var response = await client.GetByteArrayAsync(url);

                        var responseString = Encoding.GetEncoding("iso-8859-1").GetString(response, 0, response.Length - 1);
                        return responseString;
                    }

                    return await client.GetStringAsync(url);
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
