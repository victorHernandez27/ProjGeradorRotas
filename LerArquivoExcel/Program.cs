﻿using OfficeOpenXml;
using System;
using System.IO;
using System.Linq;

namespace LerArquivoExcel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            FileInfo arquivoBase = new FileInfo(@"Dados\Planilhas\GeradorRotas.xlsx");
            var saida = new StreamWriter(@"Dados\Documentos\word.docx");

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage package = new ExcelPackage(arquivoBase))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();

                int colCount = worksheet.Dimension.End.Column;

                for (int coluna = 1; coluna < colCount; coluna++)
                {
                    saida.WriteLine(worksheet.Cells[1, coluna].Value.ToString());
                }
                saida.Close();
            }
        }
    }
}
