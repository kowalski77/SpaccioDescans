using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Invoices;
public interface IWorkbookFacade
{
    IWorkbook Workbook { get; }
}
