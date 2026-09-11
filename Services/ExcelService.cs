using ClosedXML.Excel;
using System.Data;

namespace SetupReportGenerator.Services
{
    public class ExcelService
    {
        public void Export(DataTable dataTable, string filePath)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Setup Report");

                // Report Title
                worksheet.Cell(1, 1).Value = "SIPLACE SETUP REPORT";
                worksheet.Range(1, 1, 1, 7).Merge();

                worksheet.Cell(1, 1).Style.Font.Bold = true;
                worksheet.Cell(1, 1).Style.Font.FontSize = 18;
                worksheet.Cell(1, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                // Column Headers
                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    worksheet.Cell(3, i + 1).Value = dataTable.Columns[i].ColumnName;

                    worksheet.Cell(3, i + 1).Style.Font.Bold = true;
                    worksheet.Cell(3, i + 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                }

                // Data
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cell(row + 4, col + 1).Value =
                            dataTable.Rows[row][col]?.ToString();
                    }
                }

                worksheet.Columns().AdjustToContents();

                workbook.SaveAs(filePath);
            }
        }
    }
}