using System.Data;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.IO.Font.Constants;

namespace lab;

public static class Exporter
{
    public static void ExportToPdf(DataTable dt, string path)
    {
        var writer = new PdfWriter(path);
        var pdf = new PdfDocument(writer);
        var document = new Document(pdf);
        var font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
        var bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

        var table = new Table(dt.Columns.Count);

        foreach (DataColumn c in dt.Columns)
            table.AddCell(
                new Paragraph(c.ColumnName)
                    .SetFont(bold)
                    .SetBackgroundColor(ColorConstants.GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
            );

        foreach (DataRow r in dt.Rows)
        {
            if (dt.Rows.Count <= 0) continue;
            for (var i = 0; i < dt.Columns.Count; i++)
                table.AddCell(
                    new Paragraph(r[i].ToString())
                        .SetFont(font)
                        .SetBackgroundColor(ColorConstants.WHITE)
                        .SetTextAlignment(TextAlignment.CENTER));
        }

        document.Add(table);
        document.Close();
    }
}