using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace APIConsumer
{
    public static class Crud<T>
    {
        public static async Task<T> Create(string url, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(
                    System.Net.Http.Headers.MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request); // Esperar la tarea

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to create user. Status code: {response.StatusCode}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(jsonResponse);

                return result;
            }
        }


        public static async Task<T[]> Read(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<T[]>(response);
                return result;
            }
        }

        public static async Task<T> Read_ById(string url, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                url = $"{url}/{id}";
                var response = await client.GetStringAsync(url);
                var result = JsonConvert.DeserializeObject<T>(response);
                return result;
            }
        }

        public static async Task Update(string url, int id, T data)
        {
            using (HttpClient client = new HttpClient())
            {
                url = $"{url}/{id}";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(
                    MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var json = JsonConvert.SerializeObject(data);
                var request = new HttpRequestMessage(HttpMethod.Put, url);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.SendAsync(request);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to update user. Status code: {response.StatusCode}");
                }
            }
        }

        public static async Task Delete(string url, int id)
        {
            using (HttpClient client = new HttpClient())
            {
                url = $"{url}/{id}";
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(
                    MediaTypeWithQualityHeaderValue.Parse("application/json")
                );

                var response = await client.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to delete user. Status code: {response.StatusCode}");
                }
            }
        }

        public static async Task<T> Read_byName(string url, string username)
        {
            using (HttpClient client = new HttpClient())
            {
                url = $"{url}/username/{username}"; // Asegúrate de incluir el slash y formar la URL correctamente
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );

                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Failed to retrieve user. Status code: {response.StatusCode}");
                }

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<T>(jsonResponse);

                return result;
            }
        }

    }
}
