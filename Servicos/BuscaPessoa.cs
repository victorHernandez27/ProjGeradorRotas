using Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace Servicos
{
    public class BuscaPessoa
    {
        static readonly HttpClient client = new HttpClient();


        public static async Task<Pessoa> GetPessoa(string Nome)
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

    }
}
