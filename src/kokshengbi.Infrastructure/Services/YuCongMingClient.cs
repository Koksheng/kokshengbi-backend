using kokshengbi.Application.Common.Interfaces.Services;
using kokshengbi.Contracts.Chart;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace kokshengbi.Infrastructure.Services
{
    public class YuCongMingClient : IYuCongMingClient
    {
        private const string Host = "https://api.yucongming.com/api/dev";
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly HttpClient _httpClient;

        public YuCongMingClient(string accessKey, string secretKey, HttpClient httpClient)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _httpClient = httpClient;
        }

        public async Task<string> DoChatAsync(GenChartByAiDevChatRequest devChatRequest)
        {
            var url = $"{Host}/chat";
            var jsonData = JsonConvert.SerializeObject(devChatRequest);
            var headers = GetHeaders(jsonData);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            foreach (var header in headers)
            {
                content.Headers.Add(header.Key, header.Value);
            }

            var response = await _httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private Dictionary<string, string> GetHeaders(string jsonData)
        {
            var encodeBody = EncodeBody(jsonData);
            var headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "accessKey", _accessKey },
            { "nonce", GenerateNonce() },
            { "body", encodeBody },
            { "timestamp", DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString() },
            { "sign", GenerateSign(encodeBody) }
        };
            return headers;
        }

        private string GenerateNonce()
        {
            return new Random().Next(1000, 9999).ToString();
        }

        private string EncodeBody(string jsonData)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(jsonData));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        private string GenerateSign(string jsonData)
        {
            using var sha256 = SHA256.Create();
            var signStr = jsonData + "." + _secretKey;
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(signStr));
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
