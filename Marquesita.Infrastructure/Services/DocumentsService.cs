using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using Marquesita.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Marquesita.Infrastructure.Services
{
    public class DocumentsService: IDocumentsService
    {
        private readonly ISaleService _saleService;
        private readonly IConverter _converter;
        public DocumentsService(ISaleService saleService, IConverter converter)
        {
            _saleService = saleService;
            _converter = converter;
        }

        public byte[] GenerateExcelReport()
        {
            var sales = _saleService.GetSaleList();
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Ventas");
            var currentRow = 1;
            worksheet.Cell(currentRow, 1).Value = "Fecha y hora";
            worksheet.Cell(currentRow, 2).Value = "Metodo de Pago";
            worksheet.Cell(currentRow, 3).Value = "Tipo de venta";
            worksheet.Cell(currentRow, 4).Value = "Estado de venta";
            worksheet.Cell(currentRow, 5).Value = "Monto de venta";

            foreach (var sale in sales)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = sale.Date;
                worksheet.Cell(currentRow, 2).Value = sale.PaymentType;
                worksheet.Cell(currentRow, 3).Value = sale.TypeOfSale;
                worksheet.Cell(currentRow, 4).Value = sale.SaleStatus;
                worksheet.Cell(currentRow, 5).Value = "S/. " + sale.TotalAmount;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }

        public byte[] GeneratePdfSale()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Boleta N°"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = GetHTMLString(),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "PDFUtility", "CssPdf", "bootstrap.min.css") },
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Página [page] de [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "La Marquesita" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            return _converter.Convert(pdf);
        }

        public string GetHTMLString()
        {
            var sales = _saleService.GetSaleList();
            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                        <img src='data:image/png;base64,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' 
                             style='width:30%' class='img-responsive center-block'>
                                <table class='table table-striped'>
                                    <thead>
                                        <tr>
                                            <th class='text-center'>Fecha</th>
                                            <th class='text-center'>Tipo de venta</th>
                                            <th class='text-center'>Estado</th>
                                            <th class='text-center'>Monto Total</th>
                                            <th></th>
                                        </tr>
                                    </thead>
                                    <tbody>");
            foreach (var sale in sales)
            {
                sb.AppendFormat(@"<tr>
                                    <td class='text-center'>{0}</td>
                                    <td class='text-center'>{1}</td>
                                    <td class='text-center'>{2}</td>
                                    <td class='text-center'>{3}</td>
                                  </tr>", sale.Date.ToShortDateString(), sale.TypeOfSale, sale.SaleStatus, sale.TotalAmount);
            }
            sb.Append(@"
</tbody>
                                </table>
                            </body>
                        </html>");
            return sb.ToString();
        }
    }
}
