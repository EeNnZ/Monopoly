using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Monopoly.Warehouse.WebHost.Models.Box;
using Monopoly.Warehouse.WebHost.Models.Pallet;
using Newtonsoft.Json;

namespace Monopoly.Warehouse.Client.Common;

public static class ApiClient
{
    public static int Port { get; set; } = 5000;

    public static string      BaseAddress => $"http://localhost:{Port}/api/";
    public static HttpClient? Client      { get; private set; }

    public static void Initialize()
    {
        Client = new HttpClient
        {
            BaseAddress = new Uri(BaseAddress)
        };
        Client.DefaultRequestHeaders.Accept.Clear();
        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    #region Get

    public static async Task<IEnumerable<BoxResponse>?> GetBoxesAsync()
    {
        HttpResponseMessage response = await Client!.GetAsync($"{Client.BaseAddress}boxes");
        response.EnsureSuccessStatusCode();

        string json  = await response.Content.ReadAsStringAsync();
        var    boxes = JsonConvert.DeserializeObject<IEnumerable<BoxResponse>>(json);

        return boxes;
    }

    public static async Task<IEnumerable<PalletResponse>?> GetPalletsAsync()
    {
        HttpResponseMessage response = await Client!.GetAsync($"{Client.BaseAddress}pallets");
        response.EnsureSuccessStatusCode();

        string json    = await response.Content.ReadAsStringAsync();
        var    pallets = JsonConvert.DeserializeObject<IEnumerable<PalletResponse>>(json);

        return pallets;
    }

    #endregion

    #region Get{id}

    public static async Task<BoxResponse?> GetBoxAsync(long id)
    {
        HttpResponseMessage response = await Client!.GetAsync($"{Client.BaseAddress}boxes/{id}");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();
        var    box  = JsonConvert.DeserializeObject<BoxResponse>(json);

        return box;
    }

    public static async Task<PalletResponse?> GetPalletAsync(long id)
    {
        HttpResponseMessage response = await Client!.GetAsync($"{Client.BaseAddress}pallets/{id}");
        response.EnsureSuccessStatusCode();

        string json = await response.Content.ReadAsStringAsync();

        var pallet = JsonConvert.DeserializeObject<PalletResponse>(json);

        return pallet;
    }

    #endregion

    #region Post

    public static async Task<HttpStatusCode> PostBoxAsync(BoxResponse box)
    {
        HttpResponseMessage response = await Client!.PostAsJsonAsync($"{Client!.BaseAddress}boxes", box);
        response.EnsureSuccessStatusCode();
        return response.StatusCode;
    }

    public static async Task<HttpStatusCode> PostPalletAsync(PalletResponse pallet)
    {
        HttpResponseMessage response = await Client!.PostAsJsonAsync($"{Client!.BaseAddress}pallets", pallet);
        response.EnsureSuccessStatusCode();
        return response.StatusCode;
    }

    #endregion

    #region Delete

    public static async Task<HttpStatusCode> DeleteBoxAsync(long id)
    {
        HttpResponseMessage response = await Client!.DeleteAsync($"{Client.BaseAddress}boxes/{id}");
        response.EnsureSuccessStatusCode();
        return response.StatusCode;
    }

    public static async Task<HttpStatusCode> DeletePalletAsync(long id)
    {
        HttpResponseMessage response = await Client!.DeleteAsync($"{Client.BaseAddress}pallets/{id}");
        response.EnsureSuccessStatusCode();
        return response.StatusCode;
    }

    #endregion
}