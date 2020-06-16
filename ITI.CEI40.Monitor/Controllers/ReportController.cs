using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using Microsoft.AspNetCore.Mvc;

namespace ITI.CEI40.Monitor.Controllers
{
    public class ReportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public void CreatePdf()
        {
            string fileName = @"G:\test3.pdf";
            //FileStream fs = new FileStream(FileMode.Create, FileAccess.Write);
            using (var pdfWriter = new PdfWriter(fileName))
            {
                using (var pdfDocument = new PdfDocument(pdfWriter))
                {
                    Document doc = new Document(pdfDocument, PageSize.LETTER);
                    doc.Add(new Paragraph("PDF Third Trial"));
                    doc.Flush();
                    doc.Close();
                }
            }
        }

    }
}