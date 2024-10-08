﻿using Softom.Application.BusinessRules.Common.DTO;
using Softom.Application.BusinessRules.Common.Interfaces;
using Softom.Application.BusinessRules.Common.Utility;
using Softom.Application.BusinessRules.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Softom.Application.BusinessRules.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        static int previousMonth = DateTime.Now.Month == 1 ? 12 : DateTime.Now.Month - 1;
        readonly DateTime previousMonthStartDate = new(DateTime.Now.Year, previousMonth, 1);
        readonly DateTime currentMonthStartDate = new(DateTime.Now.Year, DateTime.Now.Month, 1);
        public DashboardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<PieChartDto> GetBookingPieChartData()
        {
            var totalBookings = _unitOfWork.Payment.GetAll(u => u.PaymentDate >= DateTime.Now.AddDays(-30));

            var customerWithOneBooking = totalBookings.GroupBy(b => b.Notes).Where(x => x.Count() == 1).Select(x => x.Key).ToList();

            int bookingsByNewCustomer = customerWithOneBooking.Count();
            int bookingsByReturningCustomer = totalBookings.Count() - bookingsByNewCustomer;

            PieChartDto PieChartDto = new()
            {
                Labels = new string[] { "New Customer Payments", "Returning Customer Payments" },
                Series = new decimal[] { bookingsByNewCustomer, bookingsByReturningCustomer }
            };

            return PieChartDto;
        }

        public async Task<LineChartDto> GetMemberAndBookingLineChartData()
        {
            var bookingData = _unitOfWork.Booking.GetAll(u => u.BookingDate >= DateTime.Now.AddDays(-30) &&
             u.BookingDate.Date <= DateTime.Now)
                 .GroupBy(b => b.BookingDate.Date)
                 .Select(u => new {
                     DateTime = u.Key,
                     NewBookingCount = u.Count()
                 });

            var customerData = _unitOfWork.User.GetAll(u => u.CreatedDate >= DateTime.Now.AddDays(-30) &&
            u.CreatedDate.Date <= DateTime.Now)
                .GroupBy(b => b.CreatedDate.Date)
                .Select(u => new {
                    DateTime = u.Key,
                    NewCustomerCount = u.Count()
                });


            var leftJoin = bookingData.GroupJoin(customerData, booking => booking.DateTime, customer => customer.DateTime,
                (booking, customer) => new
                {
                    booking.DateTime,
                    booking.NewBookingCount,
                    NewCustomerCount = customer.Select(x => x.NewCustomerCount).FirstOrDefault()
                });


            var rightJoin = customerData.GroupJoin(bookingData, customer => customer.DateTime, booking => booking.DateTime,
                (customer, booking) => new
                {
                    customer.DateTime,
                    NewBookingCount = booking.Select(x => x.NewBookingCount).FirstOrDefault(),
                    customer.NewCustomerCount
                });

            var mergedData = leftJoin.Union(rightJoin).OrderBy(x => x.DateTime).ToList();

            var newBookingData = mergedData.Select(x => x.NewBookingCount).ToArray();
            var newCustomerData = mergedData.Select(x => x.NewCustomerCount).ToArray();
            var categories = mergedData.Select(x => x.DateTime.ToString("MM/dd/yyyy")).ToArray();

            List<ChartData> chartDataList = new()
            {
                new ChartData
                {
                    Name = "New Payments",
                    Data = newBookingData
                },
                new ChartData
                {
                    Name = "New Members",
                    Data = newCustomerData
                },
            };

            LineChartDto LineChartDto = new()
            {
                Categories = categories,
                Series = chartDataList
            };

            return LineChartDto;
        }

        public async Task<RadialBarChartDto> GetRegisteredUserChartData()
        {

            var totalUsers = _unitOfWork.User.GetAll();

            var countByCurrentMonth = totalUsers.Count(u => u.CreatedDate >= currentMonthStartDate &&
            u.CreatedDate <= DateTime.Now);

            var countByPreviousMonth = totalUsers.Count(u => u.CreatedDate >= previousMonthStartDate &&
            u.CreatedDate <= currentMonthStartDate);

            var CurrentlyPaid = _unitOfWork.Payment.GetAll().Where(u => u.Createddate.ToString("dd.MM.yyy") == DateTime.Now.ToString("dd.MM.yyy")).Sum(f=>f.Amount);

            return SD.GetRadialCartDataModel(Convert.ToInt32(CurrentlyPaid), countByCurrentMonth, countByPreviousMonth);
        }

        public async Task<RadialBarChartDto> GetRevenueChartData()
        {
            var totalBookings = _unitOfWork.Payment.GetAll(u => u.PaymentStatus.Name == SD.StatusPaid);

            var totalRevenue = Convert.ToInt32(totalBookings.Sum(f=>f.Amount));

            var countByCurrentMonth = totalBookings.Where(u => u.Createddate >= currentMonthStartDate &&
            u.Createddate <= DateTime.Now).Sum(u => u.Amount);

            var countByPreviousMonth = totalBookings.Where(u => u.Createddate >= previousMonthStartDate &&
            u.Createddate <= currentMonthStartDate).Sum(u => u.Amount);

            return SD.GetRadialCartDataModel(totalRevenue, Convert.ToDouble(countByCurrentMonth), Convert.ToDouble(countByPreviousMonth));
        }

        public async Task<RadialBarChartDto> GetTotalBookingRadialChartData()
        {
            var totalBookings = _unitOfWork.Member.GetAll();

            var countByCurrentMonth = totalBookings.Count(u => u.Createddate >= currentMonthStartDate &&
            u.Createddate <= DateTime.Now);

            var countByPreviousMonth = totalBookings.Count(u => u.Createddate >= previousMonthStartDate &&
            u.Createddate <= currentMonthStartDate);

            return SD.GetRadialCartDataModel(totalBookings.Count(), countByCurrentMonth, countByPreviousMonth);
        }        
    }
}
