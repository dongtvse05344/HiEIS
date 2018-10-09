using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HiEIS.CustomHelpers
{
    public static class CustomHelpersClass
    {
        public static MvcHtmlString RenderPaymentStatus(bool paymentStatus)
        {
            var text = (paymentStatus) ? "Đã thanh toán" : "Chưa thanh toán";
            return new MvcHtmlString(text);
        }

        public static MvcHtmlString RenderInvoiceStatus(int status)
        {
            var text = "";
            if (status == 1)
            {
                text = "<span class='label label-info'>Mới tạo</span>";
            }
            else if (status == 2)
            {
                text = "<span class='label label-warning'>Chờ ký</span>";
            }
            else
            {
                text = "<span class='label label-primary'>Đã ký</span>";
            }
            return new MvcHtmlString(text);
        }

        public static MvcHtmlString RenderMoneyFormatWithComma(decimal money)
        {
            var text = money.ToString("#,##0");
            return new MvcHtmlString(text);
        }
    }
}