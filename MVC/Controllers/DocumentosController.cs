using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using OfficeOpenXml;
using Servicos;

namespace MVC.Controllers
{
    public class DocumentoController : Controller
    {
        public static List<List<string>> rotas = new();
        public static List<string> cabecalho = new();
        public static List<string> servicos = new();
        public static string nomeDoServico;
        public static string cidade;

        public IActionResult Upload()
        {
            return View();
        }
        public IActionResult UploadDocumento()
        {
            var documento = HttpContext.Request.Form.Files; 

            if (documento.Count > 0)
            {
                List<List<string>> rotaServico = new();
                List<string> cabecalhoDoArquivo = new();

                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using ExcelPackage lerDocumento = new(documento[0].OpenReadStream()); 
                ExcelWorksheet planilha = lerDocumento.Workbook.Worksheets.FirstOrDefault(); 

                var linhaCount = planilha.Dimension.End.Row; 
                var colunaCount = planilha.Dimension.End.Column; 

                int colunaDoCep = 0;
                int colunaDoServico = 0;

                for (var coluna = 1; coluna < colunaCount; coluna++)
                {
                    var aux = planilha.Cells[1, coluna].Value.ToString(); 

                    cabecalhoDoArquivo.Add(aux); 

                    if (aux.ToUpper() == "CEP")
                        colunaDoCep = coluna - 1; 

                    if (aux.ToUpper() == "SERVIÇO") 
                        colunaDoServico = coluna;
                }

                cabecalho = cabecalhoDoArquivo;


                planilha.Cells[2, 1, linhaCount, colunaCount] 
                        .Sort(colunaDoCep, false); 


                List<string> servico = new();

                for (var linha = 1; linha < linhaCount; linha++)
                {
                    List<string> conteudoDaLinha = new();

                    for (var column = 1; column < colunaCount; column++)
                    {
                        servico.Add(planilha.Cells[linha, colunaDoServico].Value.ToString().ToUpper());

                        var conteudo = planilha.Cells[linha, column].Value?.ToString() ?? "";
                        conteudoDaLinha.Add(conteudo.ToString());
                    }

                    rotaServico.Add(conteudoDaLinha);
                }

                servicos = servico.Distinct().ToList();
                servicos.RemoveAt(0);

                rotas = rotaServico;

                return RedirectToAction(nameof(Filtros));
            }

            return RedirectToAction(nameof(Upload));
        }

        public async Task<IActionResult> Filtros()
        {
            IEnumerable<Cidade> cidades = await BuscaCidade.BuscarTodasCidades();

            ViewBag.Cidades = cidades;
            ViewBag.Servicos = servicos;

            return View();
        }

        [HttpPost]
        public IActionResult BuscarEquipePelaCidadeAtual()
        {
            nomeDoServico = Request.Form["nomeDoServico"].ToString();
            cidade = Request.Form["Filtros"].ToString();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Equipe> equipe = await BuscaEquipe.BuscarEquipePelaCidadeId(cidade);

            ViewBag.Cabecalho = cabecalho;
            ViewBag.Equipes = equipe;

            return View();
        }

        public async Task<IActionResult> Create()
        {
            var equipeSelecionada = Request.Form["equipe"].ToList();
            var cabecalhoSelecionado = Request.Form["cabecalho"].ToList();

            if (equipeSelecionada.Count == 0 || cabecalhoSelecionado.Count == 0)
                return RedirectToAction(nameof(Index));

            List<Equipe> equipesSelecionadas = new();

            foreach (var equipeId in equipeSelecionada)
            {
                var equipe = await BuscaEquipe.BuscarEquipePeloId(equipeId);
                equipesSelecionadas.Add(equipe);
            }

            var cidadeSelecionada = await BuscaCidade.BuscarCidadePeloId(cidade);

            await ExportarDocumento.Write(rotas, cabecalhoSelecionado, equipesSelecionadas, nomeDoServico, cidadeSelecionada);

            return View();
        }

    }
}