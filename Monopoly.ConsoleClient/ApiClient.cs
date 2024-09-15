using System.Net;
using System.Net.Http.Json;
using Monopoly.API.Data.DTOs;
using Newtonsoft.Json;

namespace Monopoly.ConsoleClient
{
    public static class ApiClient
    {
        public static int Port { get; set; } = 5000;

        public static string BaseAddress => $"http://localhost:{Port}/api/";
        public static HttpClient? Client { get; private set; }

        public static void Initialize()
        {
            Client = new HttpClient
            {
                BaseAddress = new Uri(BaseAddress)
            };
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        #region Get
        public static async Task<IEnumerable<BoxDto>?> GetBoxesAsync()
        {
            var response = await Client!.GetAsync($"{Client.BaseAddress}boxes");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var boxes = JsonConvert.DeserializeObject<IEnumerable<BoxDto>>(json);

            return boxes;
        }
        public static async Task<IEnumerable<PalletDto>?> GetPalletsAsync()
        {
            var response = await Client!.GetAsync($"{Client.BaseAddress}pallets");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var pallets = JsonConvert.DeserializeObject<IEnumerable<PalletDto>>(json);

            return pallets;
        }
        #endregion

        #region Get{id}
        public static async Task<BoxDto?> GetBoxAsync(long id)
        {
            var response = await Client!.GetAsync($"{Client.BaseAddress}boxes/{id}");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();
            var box = JsonConvert.DeserializeObject<BoxDto>(json);

            return box;
        }
        public static async Task<PalletDto?> GetPalletAsync(long id)
        {
            var response = await Client!.GetAsync($"{Client.BaseAddress}pallets/{id}");
            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            var pallet = JsonConvert.DeserializeObject<PalletDto>(json);

            return pallet;
        }
        #endregion

        #region Post
        public static async Task<HttpStatusCode> PostBoxAsync(BoxDto box)
        {
            var response = await Client!.PostAsJsonAsync($"{Client!.BaseAddress}boxes", box);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> PostPalletAsync(PalletDto pallet)
        {
            var response = await Client!.PostAsJsonAsync($"{Client!.BaseAddress}pallets", pallet);
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        #endregion

        #region Delete
        public static async Task<HttpStatusCode> DeleteBoxAsync(long id)
        {
            var response = await Client!.DeleteAsync($"{Client.BaseAddress}boxes/{id}");
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        public static async Task<HttpStatusCode> DeletePalletAsync(long id)
        {
            var response = await Client!.DeleteAsync($"{Client.BaseAddress}pallets/{id}");
            response.EnsureSuccessStatusCode();
            return response.StatusCode;
        }
        #endregion
    }
}
