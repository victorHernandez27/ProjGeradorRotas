using Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Servicos
{
    public class BuscaPessoa
    {
        static readonly HttpClient client = new HttpClient();


        public static async Task<Pessoa> BuscarPessoaPeloNome(string Nome)
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44328/api/Pessoas/Nome?nome=" + Nome);
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var pessoa = JsonConvert.DeserializeObject<Pessoa>(corpoResposta);

                return pessoa;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public static async Task<List<Pessoa>> BuscarTodasPessoas()
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44328/api/Pessoas");
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var pessoa = JsonConvert.DeserializeObject<List<Pessoa>>(corpoResposta);

                return pessoa;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public static async Task<Pessoa> BuscarPessoaPeloId(string Id)
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44328/api/Pessoas/" + Id);
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var pessoa = JsonConvert.DeserializeObject<Pessoa>(corpoResposta);

                return pessoa;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
        public static void CadastrarPessoa(Pessoa pessoa)
        {
            client.PostAsJsonAsync("https://localhost:44328/api/Pessoas/", pessoa);
        }

        public static void UpdatePessoa(string id, Pessoa pessoa)
        {
            client.PutAsJsonAsync("https://localhost:44328/api/Pessoas/" + id, pessoa);
        }

        public static void RemoverPessoa(string id)
        {
            client.DeleteAsync("https://localhost:44328/api/Pessoas/" + id);
        }
    }
}
