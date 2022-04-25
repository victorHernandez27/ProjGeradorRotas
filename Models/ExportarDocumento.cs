using Aspose.Words;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
   public class ExportarDocumento
    {
        public static List<List<string>> rotaSelecionada = new();
        public static async Task Write(List<List<string>> rotas, List<string> cabecalhoSelecionado, List<Equipe> equipesSelecionadas, string nomeDoServico, Cidade cidadeSelecionada)
        {

            var indexCep = 0;
            var indexContrato = 0;
            var indexAssinante = 0;
            var indexOS = 0;
            var indexSevico = 0;
            var indexEndereco = 0;
            var indexNumero = 0;
            var indexComplemento = 0;
            var indexBairro = 0;
            var indexCidade = 0;

            for (int i = 0; i < rotas[0].Count; i++)
            {
                var aux = rotas[0][i].ToString().ToUpper();

                if (aux == "SERVIÇO")
                    indexSevico = i;
                if (aux == "CEP")
                    indexCep = i;
                if (aux == "CONTRATO")
                    indexContrato = i;
                if (aux == "ASSINANTE")
                    indexAssinante = i;
                if (aux == "OS")
                    indexOS = i;
                if (aux == "ENDEREÇO")
                    indexEndereco = i;
                if (aux == "NUMERO")
                    indexNumero = i;
                if (aux == "COMPLEMENTO")
                    indexComplemento = i;
                if (aux == "BAIRRO")
                    indexBairro = i;
                if (aux == "CIDADE")
                    indexCidade = i;
            }

            rotaSelecionada = new();
            for (int linha = 0; linha < rotas.Count; linha++)
            {
                var auxServico = rotas[linha][indexSevico].ToString().ToUpper();
                var auxCidade = rotas[linha][indexCidade].ToString().ToUpper();

                if (auxServico == nomeDoServico.ToString().ToUpper() && auxCidade == cidadeSelecionada.Nome.ToString().ToUpper())
                    rotaSelecionada.Add(rotas[linha]);
            }

            int k = 0;
            for (int j = 0; j < equipesSelecionadas.Count; j++)
            {

                Document word = new Document();
                DocumentBuilder builder = new DocumentBuilder(word);

                Font font = builder.Font;
                font.Size = 14;
                font.Bold = true;
                font.Color = System.Drawing.Color.Black;
                font.Name = "Segoe UI";
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Center;
                builder.Writeln("ROTA DE TRABALHO - " + DateTime.Now.ToString("dd/MM/yyyy") + "\n\n");
                builder.ParagraphFormat.Alignment = ParagraphAlignment.Left;
                font.Size = 11;
                builder.Writeln($"Nome da Equipe: {equipesSelecionadas[j].Codigo}  \n");
                builder.Writeln($"Tipo de Serviço: {nomeDoServico}  \n");

                for (k = 0; k < rotaSelecionada.Count; k++)
                {
                    var enderecoCompleto = $"{rotaSelecionada[k][indexEndereco]}, Numero {rotaSelecionada[k][indexNumero]} {rotaSelecionada[k][indexComplemento]} - {rotaSelecionada[k][indexBairro]}";
                    font.Bold = true;
                    font.Underline = Underline.Single;
                    font.Size = 9;
                    builder.Writeln($"Contrato: {rotaSelecionada[k][indexContrato]} - Assinante: {rotaSelecionada[k][indexAssinante]}");

                    font.Underline = Underline.None;
                    font.Bold = false;
                    builder.Writeln($"Endereço:{enderecoCompleto} - CEP: {rotaSelecionada[k][indexCep]}");
                    builder.Writeln($"O.S: {rotaSelecionada[k][indexOS]} ");
                    builder.Writeln("\n\n\n");

                }
                word.Save("GeradorRotas.docx");
            }
        }
    }
}
