using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Based.ExternalManagers
{

    public class BibleVerseGetter
    {
        public async Task<string> GetRandomVerse()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://iq-bible.p.rapidapi.com/GetRandomVerse?versionId=kjv&limitToBookId=nt"),
                Headers =
                {
                    { "X-RapidAPI-Key", Environment.GetEnvironmentVariable("XRAPIDKEY") },
                    { "X-RapidAPI-Host", "iq-bible.p.rapidapi.com" }
                },
            };

            using(var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }
}
