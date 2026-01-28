using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UiS.Dat240.Lab1.Domain.Entities;

public class Loan
{
   
    private const int LoanPeriodDays = 14;
    private const decimal Tier1DailyRate = 5.00m;  
    private const decimal Tier2DailyRate = 10.00m;  // Days 8-14
    private const decimal Tier3DailyRate = 20.00m;  // Day 15+

    public string LoanId { get; private set; }
    public string MemberId { get; private set; }
    public Book Book { get; private set; }
    public DateTime BorrowDate { get; private set; }
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public bool IsReturned => ReturnDate.HasValue;
    public bool IsOverdue => !IsReturned && DateTime.Now > DueDate;

    public Loan(string loanId, string memberId, Book book, DateTime borrowDate)
    {
       
        if (string.IsNullOrWhiteSpace(loanId))
            throw new ArgumentException("Loan ID is required.", nameof(loanId));

        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Member ID is required.", nameof(memberId));
  
        Book = book ?? throw new ArgumentNullException(nameof(book));

        LoanId = loanId;
        MemberId = memberId;
        BorrowDate = borrowDate;
        DueDate  = BorrowDate.AddDays(LoanPeriodDays);
        ReturnDate = null;

    }
public int GetDaysOverdue(DateTime currentTime)
{
    if (IsReturned || currentTime <= DueDate)
        return 0;
    return (int)(currentTime - DueDate).TotalDays;  // Ensures >=0, truncates fractions
}

    public decimal CalculateFine(DateTime currentTime)
    {
        if (IsReturned || currentTime <= DueDate) 
            return 0;
       
       
        int daysOverdue = GetDaysOverdue(currentTime);
        int tiers1Days = Math.Min(daysOverdue, 7);
        int tiers2Days = Math.Min(Math.Max(daysOverdue - 7, 0), 7);
        int tiers3Days = Math.Max(daysOverdue - 14, 0);

         
        return (tiers1Days * Tier1DailyRate) +
            (tiers2Days * Tier2DailyRate) +
            (tiers3Days * Tier3DailyRate);

       
    }
    public int GetDaysOverdue()
    {
        return GetDaysOverdue(DateTime.Now);
    }
    //public void Return(DateTime returnDate)
    public decimal CalculateFine()
    {
        return CalculateFine(DateTime.Now);
    }
    public void Return(DateTime returnDate)
    {

        if (IsReturned)
            throw new InvalidOperationException("Loan is already returned.");
        if (returnDate < BorrowDate)
            throw new ArgumentException("Return date cannot be before borrow date.", nameof(returnDate));
        ReturnDate = returnDate;
    }

}

 
