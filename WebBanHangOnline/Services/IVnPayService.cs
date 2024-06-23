using WebBanHangOnline.Models.Payments;

namespace WebBanHangOnline.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl (HttpContext context, VnPaymentRequestModel model);
        VnPaymentResponseModel PaymentExecute(IQueryCollection collection);

    }
}
