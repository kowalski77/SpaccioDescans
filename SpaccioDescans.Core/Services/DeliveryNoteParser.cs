﻿using Syncfusion.XlsIO;

namespace SpaccioDescans.Core.Services;

public class DeliveryNoteParser : InvoiceParser
{
    public DeliveryNoteParser() : base(5)
    {
    }

    public override void ParseHeader(IWorksheet worksheet, HeaderInfo header)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(header);

        worksheet.Range["S7"].Number = header.InvoiceId;
        worksheet.Range["C3"].Text = header.Name;
        worksheet.Range["G4"].Text = header.FiscalId;
        worksheet.Range["C6"].Text = header.Address;
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

    public override void ParseOrderDetail(IWorksheet worksheet, IEnumerable<OrderDetailInfo> orderInfos)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(orderInfos);

        var row = 16;
        foreach (var orderInfo in orderInfos)
        {
            worksheet.Range[$"C{row}"].Number = orderInfo.Quantity;
            worksheet.Range[$"E{row}"].Text = orderInfo.ProductDescription;
            worksheet.Range[$"N{row}"].Number = orderInfo.NetPrice;
            worksheet.Range[$"O{row}"].Number = orderInfo.Discount;
            worksheet.Range[$"S{row}"].Number = orderInfo.Total;
            row++;
        }
    }

    public override void ParsePayment(IWorksheet worksheet, PaymentInfo paymentInfo)
    {
        ArgumentNullException.ThrowIfNull(worksheet);
        ArgumentNullException.ThrowIfNull(paymentInfo);

        worksheet.Range["S38"].Number = paymentInfo.Net;
        worksheet.Range["S40"].Number = paymentInfo.Total;
        worksheet.Range["S43"].Number = paymentInfo.Pending;
        worksheet.Range["H40"].Number = paymentInfo.Cash;
        worksheet.Range["H41"].Number = paymentInfo.CreditCard;
        worksheet.Range["H42"].Number = paymentInfo.Financed;
    }
}
