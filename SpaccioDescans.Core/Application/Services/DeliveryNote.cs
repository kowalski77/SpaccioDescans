using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Application.Services;

public class DeliveryNote : InvoiceParser
{
    public DeliveryNote() : base(5)
    {
    }

    public override void ParseHeader(IWorksheet worksheet, Header header)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(header);

        worksheet.Range["S7"].Number = header.InvoiceId;
        worksheet.Range["C3"].Text = header.Name;
        worksheet.Range["G4"].Text = header.FiscalId;
        worksheet.Range["C6"].Text = header.Address;
        worksheet.Range["C7"].Text = header.City;
    }

    public override void ParseCustomer(IWorksheet worksheet, CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(customerInfo);

        worksheet.Range["N10"].Text = customerInfo.Name;
        worksheet.Range["N11"].Text = customerInfo.Nif;
        worksheet.Range["N12"].Text = customerInfo.Address;
        worksheet.Range["N13"].Text = customerInfo.Phone;
    }
}
