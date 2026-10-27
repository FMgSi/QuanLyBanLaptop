using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace QuanLyBanLaptop.Classes
{
    public static class GridExcelExporter
    {
        public static DataTable ReadGrid(DataGridView grid)
        {
            var columns = grid.Columns.Cast<DataGridViewColumn>()
                .Where(c => c.Visible && !(c is DataGridViewButtonColumn)).OrderBy(c => c.DisplayIndex).ToArray();
            var table = new DataTable();
            foreach (var column in columns)
                table.Columns.Add("C" + column.Index, typeof(string)).Caption = column.HeaderText;
            foreach (DataGridViewRow row in grid.Rows)
                if (!row.IsNewRow && row.Visible)
                    table.Rows.Add(columns.Select(c => (object)Convert.ToString(row.Cells[c.Index].FormattedValue)).ToArray());
            return table;
        }

        public static void Export(IWin32Window owner, DataGridView grid, string title, string filename)
        {
            var data = ReadGrid(grid);
            if (data.Rows.Count == 0 || data.Columns.Count == 0)
            {
                Function.ThongBaoCanhBao("Không có dữ liệu trong bảng để xuất.");
                return;
            }
            if (OpenInExcel(data, title)) return;
            // Giống màn bán hàng: xuất bảng HTML mà Excel đọc được khi không có Excel COM.
            using (var dialog = new SaveFileDialog
            {
                Title = "Lưu bảng Excel", Filter = "Tệp Excel (*.xls)|*.xls", DefaultExt = "xls",
                FileName = filename, AddExtension = true, CheckFileExists = false,
                CheckPathExists = true, OverwritePrompt = true, AutoUpgradeEnabled = false
            })
            {
                if (dialog.ShowDialog(owner) != DialogResult.OK) return;
                try { File.WriteAllText(dialog.FileName, BuildHtml(data, title), new UTF8Encoding(true)); }
                catch (Exception ex) { Function.ThongBaoLoi("Không lưu được file Excel: " + ex.Message); return; }
                if (Function.XacNhan("Đã lưu file:\n" + dialog.FileName + "\n\nMở file ngay?") == DialogResult.Yes)
                    try { Process.Start(new ProcessStartInfo(dialog.FileName) { UseShellExecute = true }); }
                    catch (Exception ex) { Function.ThongBaoCanhBao("File đã lưu, nhưng không mở được: " + ex.Message); }
            }
        }

        private static bool OpenInExcel(DataTable data, string title)
        {
            dynamic app = null, books = null, book = null, sheets = null, sheet = null, range = null;
            bool opened = false;
            try
            {
                var type = Type.GetTypeFromProgID("Excel.Application");
                if (type == null) return false;
                app = Activator.CreateInstance(type);
                books = app.Workbooks;
                book = books.Add();
                sheets = book.Worksheets;
                sheet = sheets[1];
                sheet.Name = "Danh sách";
                var values = new object[data.Rows.Count + 2, data.Columns.Count];
                values[0, 0] = title;
                for (int c = 0; c < data.Columns.Count; c++)
                {
                    values[1, c] = data.Columns[c].Caption;
                    for (int r = 0; r < data.Rows.Count; r++) values[r + 2, c] = Convert.ToString(data.Rows[r][c]);
                }
                // Các bảng này là thông tin văn bản: giữ số 0 đầu, không thực thi công thức nhập từ dữ liệu.
                string endColumn = "";
                for (int n = data.Columns.Count; n > 0; n = (n - 1) / 26)
                    endColumn = (char)('A' + (n - 1) % 26) + endColumn;
                range = sheet.Range["A1", endColumn + (data.Rows.Count + 2)];
                range.NumberFormat = "@";
                range.Value2 = values;
                range.ColumnWidth = 24;
                range.WrapText = true;
                app.Visible = true;
                app.UserControl = true;
                opened = true;
                return true;
            }
            catch (Exception ex) { Debug.WriteLine("Excel export: " + ex.Message); return false; }
            finally
            {
                if (!opened && app != null)
                    try { app.DisplayAlerts = false; app.Quit(); } catch { }
                foreach (object item in new object[] { range, sheet, sheets, book, books, app })
                    if (item != null && Marshal.IsComObject(item)) Marshal.FinalReleaseComObject(item);
            }
        }

        public static string BuildHtml(DataTable data, string title)
        {
            var html = new StringBuilder("<html><head><meta charset='utf-8'></head><body><h2>");
            html.Append(WebUtility.HtmlEncode(title)).Append("</h2><table border='1'><tr>");
            foreach (DataColumn column in data.Columns)
                html.Append("<th>").Append(WebUtility.HtmlEncode(column.Caption)).Append("</th>");
            html.Append("</tr>");
            foreach (DataRow row in data.Rows)
            {
                html.Append("<tr>");
                foreach (var value in row.ItemArray)
                    html.Append("<td style='mso-number-format:\"\\@\"'>")
                        .Append(WebUtility.HtmlEncode(Convert.ToString(value))).Append("</td>");
                html.Append("</tr>");
            }
            return html.Append("</table></body></html>").ToString();
        }
    }
}
