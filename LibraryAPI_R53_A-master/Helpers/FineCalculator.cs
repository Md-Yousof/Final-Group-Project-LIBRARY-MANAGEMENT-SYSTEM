using LibraryAPI_R53_A.Core.Domain;

namespace LibraryAPI_R53_A.Helpers
{
    public class FineCalculator
    {
        private const decimal DailyFineRate = 5.00m; //5tk perday
        public decimal CalculateFine(BorrowedBook borrowedBook)
        {
           
            if (borrowedBook?.DueDate == null || borrowedBook.ActualReturnDate == null)
            {
                throw new InvalidOperationException("DueDate and ActualReturnDate must have valid values.");
            }

            DateTime dueDate = borrowedBook.DueDate.Value;
            DateTime actualReturnDate = borrowedBook.ActualReturnDate.Value;

            if (actualReturnDate <= dueDate)
            {
                // Book was returned on or before the due date, no fine
                return 0;
            }

            // Calculate the number of days late
            int daysLate = (int)(actualReturnDate - dueDate).TotalDays;

            // Calculate the fine amount based on the number of days late and the daily fine rate
            decimal fineAmount = daysLate * DailyFineRate;

            return fineAmount;
        }
    }
}
