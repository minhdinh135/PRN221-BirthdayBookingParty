using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Models;
using Repositories;

namespace PRN221_BirthdayBookingParty.Pages
{
    [Authorize(Policy = "AdminSessionPolicy")]
    public class RevenueByWeekModel : PageModel
    {
        private readonly BookingRepository _bookingRepository;
        private readonly PaymentRepository _paymentRepository;

        public List<decimal> WeeklyRevenue { get; set; }
        public List<string> WeekLabels { get; set; }

        public RevenueByWeekModel()
        {
            _bookingRepository = new BookingRepository();
            _paymentRepository = new PaymentRepository();
        }

        public void OnGet()
        {
            var payments = _paymentRepository.GetAll();

            var currentWeek = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var currentYear = DateTime.Now.Year;

            WeeklyRevenue = new List<decimal>();
            WeekLabels = new List<string>();

            for (int i = 1; i <= currentWeek; i++)
            {
                var firstDayOfWeek = FirstDateOfWeekISO8601(currentYear, i);

                var lastDayOfWeek = firstDayOfWeek.AddDays(6);

                var revenue = payments.Where(p => p.PaidMoney > 0 && p.Booking != null && p.Booking.PartyEndTime >= firstDayOfWeek && p.Booking.PartyEndTime <= lastDayOfWeek)
                                      .Sum(p => p.PaidMoney);

                WeeklyRevenue.Add(revenue);
                WeekLabels.Add($"Week {i}");
            }
        }


        private DateTime FirstDateOfWeekISO8601(int year, int weekOfYear)
        {
            DateTime march1 = new DateTime(year, 3, 1);
            int daysOffset = (int)DayOfWeek.Thursday - (int)march1.DayOfWeek;

            DateTime firstThursday = march1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }
    }
}
