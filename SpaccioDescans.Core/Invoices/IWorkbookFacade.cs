namespace SpaccioDescans.Core.Invoices;

public interface IWorkbookFacade
{
    void AddNumber(string cell, double number);

    void AddText(string cell, string text);

    void SetWorksheetNumber(int worksheetNumber);

    MemoryStream Save();
}
