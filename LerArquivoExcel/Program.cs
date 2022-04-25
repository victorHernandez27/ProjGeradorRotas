using Aspose.Words;
using OfficeOpenXml;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace LerArquivoExcel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileInfo arquivoBase = new FileInfo(@"Dados\Planilhas\GeradorRotas.xlsx"); 
            var saida = new StreamWriter(@"Dados\Documentos\word.docx"); 

            DataTable tabela = new DataTable(); 

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; 

            using (ExcelPackage package = new ExcelPackage(arquivoBase))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault(); 

                int colCount = worksheet.Dimension.End.Column; 
                int rowCount = worksheet.Dimension.End.Row; 

                string[,] matriz = new string[rowCount - 1, colCount - 1]; 
                
                for (int i = 0; i < rowCount - 1; i++)
                {

                    for (int j = 0; j < colCount - 1; j++)
                    {
                        matriz[i, j] = worksheet.Cells[i + 1, j + 1].Value == null ? "" : worksheet.Cells[i + 1, j + 1].Value.ToString();
                    }
                }
                
                for (int i = 1; i <= colCount; i++)
                {
                    var col = worksheet.Cells[1, i].Value.ToString();
                    tabela.Columns.Add(col);
                }
                
                for (int l = 2; l <= rowCount; l++)
                {
                    var d = new string[colCount];
                    int index = 0;
                    for (int i = 1; i <= colCount; i++)
                    {
                        d[index] = worksheet.Cells[l, i].Value == null ? "" : worksheet.Cells[l, i].Value.ToString();
                        index++;
                    }
                    tabela.Rows.Add(d);
                }
                
                tabela.DefaultView.Sort = "CEP";
                tabela = tabela.DefaultView.ToTable();

                StringBuilder sbArquivo = new StringBuilder(); 

                for (int i = 0; i < tabela.Rows.Count; i++) 
                {
                    var j = 0;
                    foreach (var item in matriz) 
                    {
                        if (j < colCount)
                        {
                            sbArquivo.Append(item + ">> " + tabela.Rows[i][j] + Environment.NewLine);
                            j++;
                        }
                        else
                            break;
                    }
                }
                saida.WriteLine(sbArquivo); 
                saida.Close();
            }

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
            builder.Writeln("Nome da Equipe: EQP-0001 \n");

            for (int i = 0; i < tabela.Rows.Count; i++)
            {

                font.Underline = Underline.Single;
                font.Size = 9;
                builder.Writeln($"Contrato:    {i * 2}      - Assinante:   {i * 3}        ");

                font.Underline = Underline.None;
                font.Bold = false;
                builder.Writeln($"Endereço:     - CEP:  {i}   -000");
                builder.Writeln($"O.S:   {i * 5}         -   TIPO O.S:     {i}                  ");
                builder.Writeln("\n\n\n");

            }
            word.Save("Teste.docx");
        }
    }
}
