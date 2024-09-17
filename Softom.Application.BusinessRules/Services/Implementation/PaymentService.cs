using Softom.Application.BusinessRules.Services.Interface;
using Softom.Application.Models.Entities;
using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Implementation
{
    public class PaymentService : IPaymentService
    {
        public Session CreateStripeSession(SessionCreateOptions options)
        {
            var service = new SessionService();
            Session session = service.Create(options);
            return session;
        }

        public SessionCreateOptions CreateStripeSessionOptions(Booking booking, Villa villa, string domain)
        {
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"booking/BookingConfirmation?bookingId={booking.BookingId}",
                CancelUrl = domain + $"booking/FinalizeBooking?villaId={booking.VillaId}&checkInDate={booking.CheckInDate}&nights={booking.Nights}",
            };


            options.LineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmount = (long)(booking.TotalCost * 100),
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = villa.Name                        
                    },
                },
                Quantity = 1,
            });

            return options;
        }
    }
}
