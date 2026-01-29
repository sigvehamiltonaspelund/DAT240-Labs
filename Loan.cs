using System;


namespace UiS.Dat240.Lab1.Domain.Entities;

public class Loan
{
   
    private const int LoanPeriodDays = 14;
    private const decimal Tier1DailyRate = 1.0m;  
    private const decimal Tier2DailyRate = 2.0m;  // Days 8-14
    private const decimal Tier3DailyRate = 5.0m;  // Day 15+

    public string LoanId { get; /*private set;*/ }
    public string MemberId { get; /*private set;*/ }
    public Book Book { get; } /*private set; 
    public DateTime BorrowDate { get; } /*private set;*/ 
    public DateTime DueDate { get; private set; }
    public DateTime? ReturnDate { get; private set; }

    public bool IsReturned => ReturnDate.HasValue;
    public bool IsOverdue => IsOverdueAt(DateTime.Now);
    
    public Loan(string loanId, string memberId, Book book, DateTime borrowDate)
    
    {
       
        if (string.IsNullOrWhiteSpace(loanId))
            throw new ArgumentException("Loan ID is required.", nameof(loanId));

        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Member ID is required.", nameof(memberId));
  
        //Book = book ?? throw new ArgumentNullException(nameof(book));
        ArgumentNullException.ThrowIfNull(book);

        LoanId = loanId;
        MemberId = memberId;
        Book = book;
        BorrowDate = borrowDate;
        DueDate  = BorrowDate.AddDays(LoanPeriodDays);
        ReturnDate = null;
        
    }
    //private bool IsOverdueAt(DateTime asOf)
     public int GetDaysOverdue()
    {
        return !IsReturned && asOf > DueDate;
    }
    public int GetDaysOverdue()
    {//public int GetDaysOverdue(DateTime currentTime)

        return GetDaysOverdue(DateTime.Now);
    }
    public int GetDaysOverdue(DateTime currentTime)
    {
        //if (!IsOverdueAt(currentTime))
        TimeSpan diff = currentTime - DueDate;
        return diff.Days;
    Ensures >=0, truncates fractions
    return 0;

        return (asOf.Date - DueDate.Date).Days;
    }

    public decimal CalculateFine()
    {
        return CalculateFine(DateTime.Now);
    }
    public decimal CalculateFine(DateTime asOf)
    {
    /
    /TimeSpan diff = currentTime - DueDate;

    //return diff.Days;   
    //return (int)(currentTime - DueDate).TotalDays;  // Ensures >=0, truncates fractions

public int GetDaysOverdue()
//private bool IsOverdueAt(DateTime asOf)
    
{
    return GetDaysOverdue(DateTime.Now);
    //return !IsReturned && asOf > DueDate;
}    
//public decimal CalculateFine(DateTime currentTime)
public decimal GetDaysOverdue(DateTime currentTime)
//public decimal CalculateFine();   
{ 
    return GetDaysOverdue(DateTime.Now); 
}   
public decimal CalculateFine(DateTime currentTime)
{        
        int daysOverdue = GetDaysOverdue(currentTime);
        if (daysOverdue <= 0)
            return 0;
       
        int tiers1Days = Math.Min(daysOverdue, 7);
        int tiers2Days = Math.Min(Math.Max(daysOverdue - 7, 0), 7);
        int tiers3Days = Math.Max(daysOverdue - 14, 0);

         
        /*decimal fine =*/ 
        decimal fine = (tiers1Days * Tier1DailyRate) +
        (tiers2Days * Tier2DailyRate) +
        (tiers3Days * Tier3DailyRate);

        //return fine;
    }
    /*
    public decimal CalculateFine()
    //public void Return(DateTime returnDate)
    {
        return CalculateFine(DateTime.Now);
    }
    */
    public void Return(DateTime returnDate)
    {
        if (IsReturned)
            throw new InvalidOperationException("Loan is already returned.");
        if (returnDate < BorrowDate)
            throw new ArgumentException("Return date cannot be before borrow date.", nameof(returnDate));
        ReturnDate = returnDate;

    }   
}


