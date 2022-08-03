using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Services;

public class CustomerInvoiceParser : InvoiceParser
{
    public CustomerInvoiceParser() : base(4)
    {
    }

    public override void ParseHeader(IWorksheet worksheet, HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(header);

        worksheet.Range["B4"].Text = header.Name;
        worksheet.Range["L4"].Text = header.FiscalId;
        worksheet.Range["N4"].Number = header.InvoiceId;
        worksheet.Range["B6"].Text = header.Address;
    }

    public override void ParseCustomer(IWorksheet worksheet, CustomerInfo customerInfo)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(customerInfo);

        worksheet.Range["M10"].Text = customerInfo.Name;
        worksheet.Range["M11"].Text = customerInfo.Nif;
        worksheet.Range["M13"].Text = customerInfo.Address;
        worksheet.Range["M14"].Text = customerInfo.Phone;
    }

    public override void ParseOrderDetail(IWorksheet worksheet, IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 19;
        foreach (var orderInfo in orderInfos)
        {
            worksheet.Range[$"B{row}"].Number = orderInfo.Quantity;
            worksheet.Range[$"F{row}"].Text = orderInfo.ProductDescription;
            worksheet.Range[$"M{row}"].Number = orderInfo.NetPrice;
            worksheet.Range[$"N{row}"].Number = orderInfo.Discount;
            worksheet.Range[$"R{row}"].Number = orderInfo.Total;
            row++;
        }
    }

    public override void ParsePayment(IWorksheet worksheet, PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(paymentInfo);

        worksheet.Range["R44"].Number = paymentInfo.Net;
        worksheet.Range["R49"].Number = paymentInfo.Total;
    }
}
