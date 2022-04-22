using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Servicos
{
    public class BuscaUsuario
    {
        static readonly HttpClient client = new HttpClient();


        public static async Task<Usuario> BuscarUsuarioPeloNome(string NomeUsuario)
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44357/api/Usuarios/Nome?nome=" + NomeUsuario);
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<Usuario>(corpoResposta);

                return usuario;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public static async Task<List<Usuario>> BuscarTodosUsuarios()
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44357/api/Usuarios");
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<List<Usuario>>(corpoResposta);

                return usuario;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        public static async Task<Usuario> BuscarUsuarioPeloId(string Id)
        {
            try
            {
                HttpResponseMessage respostaAPI = await client.GetAsync("https://localhost:44357/api/Usuarios/" + Id);
                respostaAPI.EnsureSuccessStatusCode();
                string corpoResposta = await respostaAPI.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<Usuario>(corpoResposta);

                return usuario;

            }
            catch (HttpRequestException)
            {
                throw;
            }
        }
        public static void CadastrarUsuario(Usuario usuario)
        {
            client.PostAsJsonAsync("https://localhost:44357/api/Usuarios/", usuario);
        }

        public static void UpdateUsuario(string id, Usuario usuario)
        {
            client.PutAsJsonAsync("https://localhost:44357/api/Usuarios/" + id, usuario);
        }

        public static void RemoverUsuario(string id)
        {
            client.DeleteAsync("https://localhost:44357/api/Usuarios/" + id);
        }
    }
}
