using Models;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Servicos
{
    public class BuscaCidade
    {
        static readonly HttpClient client = new HttpClient();


        public static async Task<Cidade> GetCidade(string Nome)
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
                throw;
            }


        }
    }
}
