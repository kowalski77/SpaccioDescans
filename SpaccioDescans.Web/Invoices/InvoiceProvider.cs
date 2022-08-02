namespace SpaccioDescans.Web.Invoices;

public abstract class InvoiceProvider
{
    public string FilePath => Path.Combine("Files", "invoices.xls");
}
