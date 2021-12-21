using ClosedXML.Excel;
using DinkToPdf;
using DinkToPdf.Contracts;
using Marquesita.Infrastructure.Interfaces;
using Marquesita.Models.Business;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Marquesita.Infrastructure.Services
{
    public class DocumentsService : IDocumentsService
    {
        private readonly ISaleService _saleService;
        private readonly IUserManagerService _userManager;
        private readonly IConverter _converter;
        private readonly IAddressService _addressService;
        public DocumentsService(ISaleService saleService, IConverter converter, IUserManagerService userManager, IAddressService addressService)
        {
            _saleService = saleService;
            _converter = converter;
            _userManager = userManager;
            _addressService = addressService;
        }

        public async Task<byte[]> GenerateExcelReport()
        {
            var sales = _saleService.GetSaleList();
            using var workbook = new XLWorkbook();

            var worksheet = workbook.Worksheets.Add("Ventas");
            var currentRow = 1;
            var counter = 1;
            worksheet.Cell(currentRow, 1).Value = "Orden";
            worksheet.Cell(currentRow, 2).Value = "Cliente";
            worksheet.Cell(currentRow, 3).Value = "Fecha y hora";
            worksheet.Cell(currentRow, 4).Value = "Metodo de Pago";
            worksheet.Cell(currentRow, 5).Value = "Tipo de venta";
            worksheet.Cell(currentRow, 6).Value = "Estado de venta";
            worksheet.Cell(currentRow, 7).Value = "Empleado";
            worksheet.Cell(currentRow, 8).Value = "Monto de venta";

            foreach (var sale in sales)
            {
                currentRow++;
                counter++;

                var client = await _userManager.GetUserByIdAsync(sale.UserId);
                var employee = await _userManager.GetUserByIdAsync(sale.EmployeeId);
                worksheet.Cell(currentRow, 1).Value = counter;
                worksheet.Cell(currentRow, 2).Value = client.FirstName + " " + client.LastName;
                worksheet.Cell(currentRow, 3).Value = sale.Date;
                worksheet.Cell(currentRow, 4).Value = sale.PaymentType;
                worksheet.Cell(currentRow, 5).Value = sale.TypeOfSale;
                worksheet.Cell(currentRow, 6).Value = sale.SaleStatus;
                if (employee != null)
                {
                    worksheet.Cell(currentRow, 7).Value = employee.FirstName + " " + employee.LastName;
                }
                else
                {
                    worksheet.Cell(currentRow, 7).Value = "-";
                }
                worksheet.Cell(currentRow, 8).Value = sale.TotalAmount;

            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            var content = stream.ToArray();

            return content;
        }

        public async Task<byte[]> GeneratePdfSaleReportAsync()
        {
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Estado de Ventas"
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await PdfHtmlSaleReportAsync(),
                WebSettings = { DefaultEncoding = "utf-8" },
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

        public async Task<string> PdfHtmlSaleReportAsync()
        {
            var sales = _saleService.GetSaleList();
            int count = 0;
            var actualDate = DateTime.Now;
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<!DOCTYPE html><head> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css'> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/admin-lte@3.0.5/dist/css/adminlte.min.css'> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/admin-lte@3.0.5/dist/css/adminlte.min.css.map'> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.1/css/all.min.css'> <link rel='stylesheet' href='https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css'> <link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700'></head><body> <br/> <div class='wrapper'> <section class='invoice p-3 mb-3'> <div class='row'> <div class='col-12'> <h2 class='page-header'> <img src='https://i.ibb.co/VNmddvj/Los-Alpes-Logo.png' alt='LosAlpes-Logo' style='width: 150px; height: 150px; border: 0;'>");
            stringBuilder.AppendFormat(@"<small class='float-right'>Fecha: {0}</small> </h2> </div></div>", actualDate);
            stringBuilder.Append(@"<table class='table table-striped'> <thead class='thead-dark'> <tr> <th class='text-center'>Orden</th> <th class='text-center'>Cliente</th> <th class='text-center'>Fecha</th> <th class='text-center'>Tipo de venta</th> <th class='text-center'>Estado</th> <th class='text-center'>Vendedor</th> <th class='text-center'>Monto Total</th> </tr></thead> <tbody>");
            foreach (var sale in sales) 
            { 
                var client = await _userManager.GetUserByIdAsync(sale.UserId);
                var employee = await _userManager.GetUserByIdAsync(sale.EmployeeId);
                count++;
                if (employee != null) 
                { 
                    stringBuilder.AppendFormat(@"<tr> <td class='text-center'>{0}</td><td class='text-center'>{1} {2}</td><td class='text-center'>{3}</td><td class='text-center'>{4}</td><td class='text-center'>{5}</td><td class='text-center'>{6} {7}</td><td class='text-center'>S/.{8}</td></tr>", count, client.FirstName, client.LastName, sale.Date.ToShortDateString(), sale.TypeOfSale, sale.SaleStatus, employee.FirstName, employee.LastName, sale.TotalAmount);
                } 
                else 
                { 
                    stringBuilder.AppendFormat(@"<tr> <td class='text-center'>{0}</td><td class='text-center'>{1} {2}</td><td class='text-center'>{3}</td><td class='text-center'>{4}</td><td class='text-center'>{5}</td><td class='text-center'> - </td><td class='text-center'>S/.{6}</td></tr>", count, client.FirstName, client.LastName, sale.Date.ToShortDateString(), sale.TypeOfSale, sale.SaleStatus, sale.TotalAmount); 
                } 
            }
            stringBuilder.Append(@"</tbody> </table> </section> </div></body></html>");
            return stringBuilder.ToString();
        }

        public async Task<byte[]> GenerateSalePDF(Sale sale)
        {
            var saleNumber = _saleService.GetSaleListCount() + 1;
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "Los Alpes Boleta N° " + saleNumber
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await InvoiceHtml(sale, saleNumber),
                WebSettings = { DefaultEncoding = "utf-8" },
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

        public async Task<string> InvoiceHtml(Sale sale, int saleNumber)
        {
            var client = await _userManager.GetUserByIdAsync(sale.UserId);
            var clientAddress = _addressService.GetAddressFullText(sale.AddressId);
            int count = 0;
            var saleDetail = _saleService.GetDetailSaleList(sale.Id);
            var igv = Math.Round((sale.TotalAmount * Convert.ToDecimal(0.18)), 2);
            var subtotal = Math.Round((sale.TotalAmount - igv), 2);
            var actualDate = DateTime.Now;

            var stringBuilder = new StringBuilder();
            stringBuilder.Append(@"<!DOCTYPE html><head> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/bootstrap@4.5.3/dist/css/bootstrap.min.css'> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/admin-lte@3.0.5/dist/css/adminlte.min.css'> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/admin-lte@3.0.5/dist/css/adminlte.min.css.map'> <link rel='stylesheet' href='https://cdn.jsdelivr.net/npm/@fortawesome/fontawesome-free@5.15.1/css/all.min.css'> <link rel='stylesheet' href='https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css'> <link rel='stylesheet' href='https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700'></head><body> <br/> <div class='wrapper'> <section class='invoice p-3 mb-3'> <div class='row'> <div class='col-12'> <h2 class='page-header'> <img src='https://i.ibb.co/VNmddvj/Los-Alpes-Logo.png' alt='LosAlpes-Logo' style='width: 150px; height: 150px; border: 0;'>");
            stringBuilder.AppendFormat(@"<small class='float-right'>Fecha: {0}</small> </h2> </div></div><div> <table style='width: 100%;'> <tbody> <td style='width: 33.3333333333%;'> De <address> <strong>Los Alpes E.I.R.L.</strong><br>Jr. Junín Nro. 965 (Barrio San Pedro)<br>Telefono: (076)362287 <br>Celulares: 941342093 <br>Correo: losalpes.cajamarca@gmail.com</address> </td><td style='width: 33.3333333333%;'> Para <address> <strong>{1} {2}</strong><br>{3}<br>Celular: {4}<br>Correo: {5}</address> </td><td style='width: 33.3333333333%;'> <b>Boleta #{6}</b><br><br><b>ID de pedido:</b> {7}<br><b>Día de pago:</b> {8}<br><b>Tipo de pago:</b> {9}</td></tbody> </table> </div>", actualDate, client.FirstName, client.LastName, clientAddress, client.Phone, client.Email, saleNumber, sale.Id, sale.Date, sale.PaymentType); 
            stringBuilder.Append(@"<table class='table table-striped'> <thead> <tr> <th class='text-center'>Orden</th> <th class='text-center'>Producto</th> <th class='text-center'>Precio</th> <th class='text-center'>Cantidad</th> <th class='text-center'>Subtotal</th> </tr></thead> <tbody>");
            foreach (var detail in saleDetail) { 
                count++; 
                stringBuilder.AppendFormat(@" <tr> <td class='text-center'>{0}</td><td class='text-center'>{1}</td><td class='text-center'>S./{2}</td><td class='text-center'>{3}</td><td class='text-center'>S./{4}</td></tr>", count, detail.Product.Name, detail.UnitPrice, detail.Quantity, detail.Subtotal);
            }
            stringBuilder.AppendFormat(@" </tbody> </table> <div> <table style='width: 100%;'> <tbody> <td style='width: 50%;'> </td><td style='width: 50%;'> <table class='table' style='width: 100%;'> <tr> <th style='width:50%'>Subtotal:</th> <td>S/.{0}</td></tr><tr> <th>IGV (18%)</th> <td>S/.{1}</td></tr><tr> <th>Total:</th> <td>S/.{2}</td></tr></table> </td></tbody> </table> </div></section> </div></body></html>", subtotal, igv, sale.TotalAmount);
            return stringBuilder.ToString();
        }
    }
}
