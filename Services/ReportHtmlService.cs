using SetupReportGenerator.DAL;
using SetupReportGenerator.Models;
using System;
using System.Data;
using System.Net;
using System.Text;

namespace SetupReportGenerator.Services
{
    /// <summary>
    /// Generates a premium, self-contained HTML report (Bootstrap 5 + DataTables)
    /// that is displayed inside a WebView2 control via NavigateToString().
    ///
    /// Features: frozen header, frozen first column, live search, pagination,
    /// row numbers, Excel/PDF export, print, company branding, recipe info,
    /// generated timestamp, footer, responsive layout, dark-blue theme.
    ///
    /// NOTE: This template pulls Bootstrap/DataTables/jsPDF/SheetJS libraries
    /// from public CDNs, so the machine running WebView2 needs internet access.
    /// If your production PC is offline, see the "OFFLINE USE" note at the
    /// bottom of this file for how to host the same libraries locally.
    /// </summary>
    public class ReportHtmlService
    {
        private const string CompanyName = "ASMPT India Private Limited";
        private const string CompanyAddress = "Gurappa Avenue, Bengaluru, Karnataka 560025";
        private const string CompanyInitials = "ASMPT";

        public string GenerateReport(DateTime fromDate, DateTime toDate, int recipeId)
        {
            RecipeDAL recipeDAL = new RecipeDAL();
            SetupDAL setupDAL = new SetupDAL();

            RecipeHeader recipe = null;

            if (recipeId > 0)
                recipe = recipeDAL.GetRecipe(recipeId);

            DataTable dt = setupDAL.GetReport(fromDate, toDate, recipeId);

            StringBuilder html = new StringBuilder();

            html.Append(GetHead());
            html.Append(GetBodyHeader(recipe));
            html.Append(GetTableSection(dt));
            html.Append(GetFooterAndScripts());

            return html.ToString();
        }

        // ---------------------------------------------------------------
        // HEAD (Bootstrap + DataTables CSS + custom dark-blue theme)
        // ---------------------------------------------------------------
        private string GetHead()
        {
            return @"
<!DOCTYPE html>
<html lang='en'>
<head>
<meta charset='utf-8' />
<meta name='viewport' content='width=device-width, initial-scale=1' />
<title>SIPLACE Setup Report</title>

<link href='https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css' rel='stylesheet'>
<link href='https://cdn.datatables.net/1.13.8/css/dataTables.bootstrap5.min.css' rel='stylesheet'>
<link href='https://cdn.datatables.net/fixedheader/3.4.0/css/fixedHeader.bootstrap5.min.css' rel='stylesheet'>
<link href='https://cdn.datatables.net/fixedcolumns/4.3.0/css/fixedColumns.bootstrap5.min.css' rel='stylesheet'>
<link href='https://cdn.datatables.net/buttons/2.4.2/css/buttons.bootstrap5.min.css' rel='stylesheet'>

<style>
    :root{
        --dark-blue:#0A2F52;
        --mid-blue:#0B5394;
        --bright-blue:#1E7FC2;
        --light-blue:#EAF2FA;
        --accent:#F2A900;
        --gold:#E8B84B;
    }
    *{ box-sizing:border-box; }
    body{
        font-family:'Segoe UI', Calibri, Arial, sans-serif;
        background: radial-gradient(circle at top left, #F5F8FC 0%, #E3EAF2 45%, #DCE5EF 100%);
        color:#222;
        min-height:100vh;
    }
    .report-header{
        background: linear-gradient(120deg, var(--dark-blue) 0%, var(--mid-blue) 55%, var(--bright-blue) 100%);
        color:#fff;
        padding:20px 28px;
        border-radius:12px;
        margin-bottom:20px;
        box-shadow:0 10px 28px rgba(10,47,82,0.35), inset 0 1px 0 rgba(255,255,255,0.15);
        position:relative;
        overflow:hidden;
    }
    .report-header::after{
        content:'';
        position:absolute; inset:0;
        background:linear-gradient(180deg, rgba(255,255,255,0.10), rgba(255,255,255,0) 40%);
        pointer-events:none;
    }
    .company-logo-box{
        width:60px;height:60px;border-radius:12px;
        background:linear-gradient(160deg,#ffffff,#dbe9f7);
        display:flex;align-items:center;justify-content:center;
        font-weight:700;color:var(--dark-blue);font-size:13px;letter-spacing:0.5px;
        flex-shrink:0;
        box-shadow:0 4px 10px rgba(0,0,0,0.25);
    }
    .company-name{ font-size:19px; font-weight:600; margin:0; }
    .company-address{ font-size:12px; opacity:0.85; margin:0; }
    .report-title{
        font-size:24px; font-weight:700; letter-spacing:1px; text-align:center;
        text-shadow:0 2px 6px rgba(0,0,0,0.25);
    }
    .generated-date{ font-size:12px; color:#eaf2fa; text-align:right; line-height:1.4; }

    .recipe-info-box{
        background:linear-gradient(160deg,#ffffff,#f4f8fc);
        border:1px solid #d7e2ec; border-radius:12px;
        padding:14px 20px; margin-bottom:18px;
        box-shadow:0 6px 16px rgba(11,61,102,0.08);
    }
    .recipe-info-box td{ padding:4px 10px; font-size:14px; }
    .recipe-info-box td.label{ font-weight:600; color:var(--dark-blue); width:150px; }

    .report-card{
        border:0; border-radius:12px;
        background:linear-gradient(180deg,#ffffff,#fbfcfe);
        box-shadow:0 10px 26px rgba(11,61,102,0.12);
    }

    table.dataTable thead th{
        background: linear-gradient(180deg, var(--mid-blue), var(--dark-blue)) !important;
        color:#fff !important;
        border-color:#0a2f4f !important;
        vertical-align:middle;
        white-space:nowrap;
        text-shadow:0 1px 2px rgba(0,0,0,0.2);
    }
    table.dataTable tbody tr:nth-child(even){ background-color:var(--light-blue); }
    table.dataTable tbody tr:hover{
        background:linear-gradient(90deg,#FFF3CD,#FFEAA6) !important;
        transition:background 0.15s ease;
    }
    table.dataTable tbody td{ vertical-align:middle; }

    .dtfc-fixed-left{ background-color:#fff; box-shadow:3px 0 6px rgba(0,0,0,0.10); }

    .dt-buttons .btn{
        background:linear-gradient(160deg, var(--bright-blue), var(--mid-blue));
        border:0; color:#fff;
        margin-right:6px; font-size:13px; font-weight:600;
        border-radius:7px; padding:7px 14px;
        box-shadow:0 3px 8px rgba(11,83,148,0.35);
        transition:transform 0.12s ease, box-shadow 0.12s ease;
    }
    .dt-buttons .btn:hover{
        background:linear-gradient(160deg, var(--mid-blue), var(--dark-blue));
        color:#fff; transform:translateY(-1px);
        box-shadow:0 6px 14px rgba(11,61,102,0.4);
    }

    div.dataTables_filter input{
        border-radius:7px; border:1px solid #c9d6e3; padding:6px 12px;
        box-shadow:inset 0 1px 3px rgba(0,0,0,0.06);
    }
    div.dataTables_length select{ border-radius:6px; }

    .report-footer{
        margin-top:22px; padding:14px 20px;
        background:linear-gradient(120deg, var(--dark-blue), var(--mid-blue));
        color:#fff; border-radius:12px; font-size:12px; display:flex;
        justify-content:space-between; align-items:center; flex-wrap:wrap; gap:6px;
        box-shadow:0 8px 20px rgba(10,47,82,0.25);
    }

    /* Floating scroll-to-top / scroll-to-bottom buttons */
    .scroll-fab-group{
        position:fixed; right:22px; bottom:26px; z-index:1050;
        display:flex; flex-direction:column; gap:10px;
    }
    .scroll-fab{
        width:44px; height:44px; border-radius:50%;
        background:linear-gradient(160deg, var(--bright-blue), var(--dark-blue));
        color:#fff; border:0; font-size:18px;
        display:flex; align-items:center; justify-content:center;
        box-shadow:0 6px 16px rgba(10,47,82,0.4);
        cursor:pointer; opacity:0; visibility:hidden;
        transform:translateY(10px);
        transition:opacity 0.2s ease, transform 0.2s ease, box-shadow 0.15s ease;
    }
    .scroll-fab.show{ opacity:1; visibility:visible; transform:translateY(0); }
    .scroll-fab:hover{ box-shadow:0 10px 22px rgba(10,47,82,0.55); transform:translateY(-2px); }

    @media print{
        .no-print{ display:none !important; }
        body{ background:#fff; }
        .report-header, .report-footer{
            -webkit-print-color-adjust:exact; print-color-adjust:exact;
        }
    }

    @media (max-width: 768px){
        .report-header{ text-align:center; }
        .report-title{ margin:10px 0; }
        .generated-date{ text-align:center; }
    }
</style>
</head>
<body>
<div class='container-fluid py-3'>
";
        }

        // ---------------------------------------------------------------
        // BODY HEADER (logo, company info, title, recipe info)
        // ---------------------------------------------------------------
        private string GetBodyHeader(RecipeHeader recipe)
        {
            string generated = DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss");

            string recipeName = Encode(recipe?.RecipeName);
            string lineName = Encode(recipe?.LineName);
            string model = Encode(recipe?.Model);
            string boardSide = Encode(recipe?.BoardSide);

            return $@"
<div class='report-header d-flex justify-content-between align-items-center flex-wrap gap-3'>
    <div class='d-flex align-items-center gap-3'>
        <div class='company-logo-box'>{CompanyInitials}</div>
        <div>
            <p class='company-name'>{CompanyName}</p>
            <p class='company-address'>{CompanyAddress}</p>
        </div>
    </div>
    <div class='report-title'>SIPLACE SETUP REPORT</div>
    <div class='generated-date'>
        Generated On<br/>{generated}
    </div>
</div>

<div class='recipe-info-box'>
    <table class='w-100'>
        <tr>
            <td class='label'>Recipe Name</td><td>{recipeName}</td>
            <td class='label'>Line Name</td><td>{lineName}</td>
        </tr>
        <tr>
            <td class='label'>Model</td><td>{model}</td>
            <td class='label'>Board Side</td><td>{boardSide}</td>
        </tr>
    </table>
</div>

<div class='card report-card'>
<div class='card-body'>
";
        }

        // ---------------------------------------------------------------
        // TABLE (row numbers filled in by DataTables via columnDefs)
        // ---------------------------------------------------------------
        private string GetTableSection(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"
<div class='table-responsive'>
<table id='setupTable' class='table table-bordered table-striped align-middle w-100'>
<thead>
<tr>
<th>#</th>
<th>Machine Name</th>
<th>Table</th>
<th>Track</th>
<th>Part Number</th>
<th>Quantity</th>
<th>Reference Designators</th>
<th>Feeder Type</th>
</tr>
</thead>
<tbody>
");
            foreach (DataRow row in dt.Rows)
            {
                sb.Append("<tr>");
                sb.Append("<td></td>"); // filled by DataTables render function
                sb.Append("<td>" + Encode(row["MachineName"]) + "</td>");
                sb.Append("<td>" + Encode(row["Table"]) + "</td>");
                sb.Append("<td>" + Encode(row["Track"]) + "</td>");
                sb.Append("<td>" + Encode(row["PartNumber"]) + "</td>");
                sb.Append("<td>" + Encode(row["Quantity"]) + "</td>");
                sb.Append("<td>" + Encode(row["ReferenceDesignators"]) + "</td>");
                sb.Append("<td>" + Encode(row["FeederType"]) + "</td>");
                sb.Append("</tr>");
            }
            sb.Append(@"
</tbody>
</table>
</div>
");
            return sb.ToString();
        }

        // ---------------------------------------------------------------
        // FOOTER + SCRIPTS (DataTables init: fixed header/column, search,
        // pagination, row numbers, Excel/PDF export, print)
        // ---------------------------------------------------------------
        private string GetFooterAndScripts()
        {
            int year = DateTime.Now.Year;

            return $@"
</div>
</div>

<div class='report-footer no-print'>
    <span>&copy; {year} {CompanyName} &mdash; Confidential</span>
    <span>SIPLACE Setup Report Generator v1.0</span>
</div>

</div>

<div class='scroll-fab-group no-print'>
    <button id='fabScrollTop' class='scroll-fab' type='button' title='Scroll to top'>&#8593;</button>
    <button id='fabScrollBottom' class='scroll-fab' type='button' title='Scroll to bottom'>&#8595;</button>
</div>

<script src='https://code.jquery.com/jquery-3.7.1.min.js'></script>
<script src='https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js'></script>

<script src='https://cdn.datatables.net/1.13.8/js/jquery.dataTables.min.js'></script>
<script src='https://cdn.datatables.net/1.13.8/js/dataTables.bootstrap5.min.js'></script>
<script src='https://cdn.datatables.net/fixedheader/3.4.0/js/dataTables.fixedHeader.min.js'></script>
<script src='https://cdn.datatables.net/fixedcolumns/4.3.0/js/dataTables.fixedColumns.min.js'></script>

<script src='https://cdn.datatables.net/buttons/2.4.2/js/dataTables.buttons.min.js'></script>
<script src='https://cdn.datatables.net/buttons/2.4.2/js/buttons.bootstrap5.min.js'></script>
<script src='https://cdn.datatables.net/buttons/2.4.2/js/buttons.html5.min.js'></script>
<script src='https://cdn.datatables.net/buttons/2.4.2/js/buttons.print.min.js'></script>
<script src='https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js'></script>
<script src='https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/pdfmake.min.js'></script>
<script src='https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.2.7/vfs_fonts.js'></script>

<script>
$(document).ready(function () {{

    $('#setupTable').DataTable({{
        pageLength: 10,
        lengthMenu: [5, 10, 25, 50, 100],
        order: [],
        fixedHeader: true,
        scrollX: true,
        fixedColumns: {{ leftColumns: 2 }},
        columnDefs: [
            {{
                targets: 0,
                orderable: false,
                searchable: false,
                className: 'text-center',
                render: function (data, type, row, meta) {{
                    return meta.row + 1;
                }}
            }}
        ],
        dom: ""<'row mb-2 align-items-center'<'col-sm-6'B><'col-sm-6'f>>"" +
             ""<'row'<'col-sm-12't>>"" +
             ""<'row mt-2 align-items-center'<'col-sm-5'i><'col-sm-7'p>>"",
        buttons: [
            {{ extend: 'excelHtml5', text: 'Export Excel', className: 'btn btn-sm', title: 'SIPLACE_Setup_Report' }},
            {{
                extend: 'pdfHtml5',
                text: 'Export PDF',
                className: 'btn btn-sm',
                title: 'SIPLACE_Setup_Report',
                orientation: 'landscape',
                pageSize: 'A4',
                customize: function (doc) {{
                    // The exported title is always the first element in
                    // doc.content when 'title' is set on the button - force
                    // it to be centered instead of pdfmake's left default.
                    if (doc.content && doc.content.length > 0) {{
                        doc.content[0].alignment = 'center';
                        doc.content[0].fontSize = 16;
                        doc.content[0].bold = true;
                        doc.content[0].margin = [0, 0, 0, 12];
                    }}

                    // Shrink the overall font so more rows fit per page.
                    // (Previously the default ~10-11pt body font combined
                    // with equal-width columns caused long text columns
                    // like Reference Designators / Part Number to wrap
                    // onto 2-3 lines each, which meant only ~4 rows fit on
                    // page 1 - the rest were simply flowing onto page 2+.)
                    doc.defaultStyle = doc.defaultStyle || {{}};
                    doc.defaultStyle.fontSize = 7;
                    doc.pageMargins = [18, 30, 18, 30];

                    // Give narrow, short-content columns an 'auto' width so
                    // they shrink to fit their content, and let the wider
                    // text columns (Part Number, Reference Designators,
                    // Feeder Type) share the remaining page width. This
                    // reduces wrapping/row-height compared to forcing every
                    // column to the same equal width.
                    // Column order: # | Machine Name | Table | Track |
                    //               Part Number | Quantity |
                    //               Reference Designators | Feeder Type
                    var columnWidths = ['auto', 'auto', 'auto', 'auto', '*', 'auto', '*', 'auto'];

                    for (var i = 0; i < doc.content.length; i++) {{
                        var item = doc.content[i];
                        if (item && item.table && item.table.body && item.table.body.length > 0) {{
                            var colCount = item.table.body[0].length;
                            item.table.widths = (colCount === columnWidths.length)
                                ? columnWidths
                                : new Array(colCount).fill('*');

                            // Tight cell padding so more rows fit per page.
                            item.table.body.forEach(function (rowArr) {{
                                rowArr.forEach(function (cell) {{
                                    cell.margin = [2, 1, 2, 1];
                                }});
                            }});
                        }}
                    }}

                    // Add a page-number footer so it's easy to confirm all
                    // rows made it in when the table spans multiple pages.
                    doc.footer = function (currentPage, pageCount) {{
                        return {{
                            text: 'Page ' + currentPage + ' of ' + pageCount,
                            alignment: 'center',
                            fontSize: 8,
                            margin: [0, 6, 0, 0]
                        }};
                    }};
                }}
            }},
            {{ extend: 'print', text: 'Print', className: 'btn btn-sm' }}
        ],
        language: {{
            searchPlaceholder: 'Search report...',
            search: ''
        }}
    }});

    // Floating scroll-to-top / scroll-to-bottom buttons
    var fabTop = document.getElementById('fabScrollTop');
    var fabBottom = document.getElementById('fabScrollBottom');

    function toggleFabs() {{
        var scrollTop = window.scrollY || document.documentElement.scrollTop;
        var atBottom = (window.innerHeight + scrollTop) >= (document.body.scrollHeight - 30);

        if (scrollTop > 150) {{ fabTop.classList.add('show'); }} else {{ fabTop.classList.remove('show'); }}
        if (!atBottom && document.body.scrollHeight > window.innerHeight + 150) {{
            fabBottom.classList.add('show');
        }} else {{
            fabBottom.classList.remove('show');
        }}
    }}

    window.addEventListener('scroll', toggleFabs);
    window.addEventListener('resize', toggleFabs);
    setTimeout(toggleFabs, 300);

    fabTop.addEventListener('click', function () {{
        window.scrollTo({{ top: 0, behavior: 'smooth' }});
    }});
    fabBottom.addEventListener('click', function () {{
        window.scrollTo({{ top: document.body.scrollHeight, behavior: 'smooth' }});
    }});
}});
</script>

</body>
</html>
";
        }

        private static string Encode(object value)
        {
            return WebUtility.HtmlEncode(Convert.ToString(value ?? string.Empty));
        }
    }
}

/*
==============================================================================
OFFLINE USE (recommended for a shop-floor PC with no internet access)
==============================================================================
This template loads Bootstrap, DataTables, jsPDF and SheetJS from public
CDNs. If your production machine is not connected to the internet, do this
instead:

1. Download once (on a PC with internet) and copy the files into a folder
   inside your project, e.g.  ProjectFolder\wwwroot\lib\...
      - bootstrap.min.css / bootstrap.bundle.min.js
      - dataTables.bootstrap5.min.css/js
      - fixedHeader / fixedColumns / buttons (css + js, incl. buttons.html5,
        buttons.print, jszip, pdfmake, vfs_fonts)

2. In your Form, map that folder to a virtual host BEFORE calling
   NavigateToString, then rewrite the CDN URLs above to use the virtual host:

    await webView21.EnsureCoreWebView2Async();
    webView21.CoreWebView2.SetVirtualHostNameToFolderMapping(
        "appassets", "wwwroot", CoreWebView2HostResourceAccessKind.Allow);

    // then reference assets as: https://appassets/lib/bootstrap.min.css
    // instead of the jsdelivr/cdn.datatables.net URLs.

This keeps the exact same look/feel and features fully working offline.
==============================================================================
*/