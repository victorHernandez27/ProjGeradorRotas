using Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http.Json;

namespace Servicos
{
    public class BuscaCidade
    {
        static readonly HttpClient client = new HttpClient();


        public static async Task<Cidade> BuscarCidadePeloNome(string Nome)
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44326/api/Cidades/Nome?nome=" + Nome);
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var cidade = JsonConvert.DeserializeObject<Cidade>(corpoResposta);

                return cidade;

            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public static async Task<List<Cidade>> BuscarTodasCidades()
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44326/api/Cidades");
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var cidade = JsonConvert.DeserializeObject<List<Cidade>>(corpoResposta);

                return cidade;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public static async Task<Cidade> BuscarCidadePeloId(string Id)
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44326/api/Cidades/" + Id);
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var cidade = JsonConvert.DeserializeObject<Cidade>(corpoResposta);

                return cidade;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
        public static void CadastrarCidade(Cidade cidade)
        {
            client.PostAsJsonAsync("https://localhost:44326/api/Cidades/", cidade);
        }

        public static void UpdateCidade(string id, Cidade cidade)
        {
            client.PutAsJsonAsync("https://localhost:44326/api/Cidades/" + id, cidade);
        }

        public static void RemoverCidade(string id)
        {
            client.DeleteAsync("https://localhost:44326/api/Cidades/" + id);
        }
    }
}
