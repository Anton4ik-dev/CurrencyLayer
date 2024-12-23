using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;

namespace CurrencyLayer
{
    public class CurrencyLayerAPI
    {
        private static string API_KEY = "167f9923e3fefc38f3241cfa4a9acb91";
        private static string URL = "http://api.currencylayer.com/live";

        public async Task<float> GetExchangeRate()
        {
            string requestUrl = $"{URL}?access_key={API_KEY}&currencies=EUR&source=USD&format=1";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl))
            {
                var operation = webRequest.SendWebRequest();

                while (!operation.isDone)
                {
                    await Task.Yield();
                }

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"{webRequest.error} {webRequest.responseCode}");
                    return 0;
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    jsonResponse = jsonResponse.Trim();
                    jsonResponse = System.Text.Encoding.UTF8.GetString(System.Text.Encoding.Default.GetBytes(jsonResponse));
                    return ProcessResponse(jsonResponse);
                }
            }
        }

        private float ProcessResponse(string jsonResponse)
        {
            try
            {
                var serializer = new JsonSerializer();
                using (var reader = new JsonTextReader(new StringReader(jsonResponse)))
                {
                    CurrencyResponse response = serializer.Deserialize<CurrencyResponse>(reader);

                    if (response == null)
                        Debug.LogError("Response null");
                    else if (!response.Success)
                        Debug.LogError(jsonResponse);
                    else if (response.Quotes != null)
                    {
                        if (response.Quotes.TryGetValue("USDEUR", out float exchangeRate))
                            return exchangeRate;
                    }
                }
            }
            catch (JsonReaderException ex)
            {
                Debug.LogError($"{ex.Message}");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"{ex.Message}");
            }
            return 0;
        }
    }
}