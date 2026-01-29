using System;
using System.Collections.Generic;

namespace UiS.Dat240.Lab1.Domain.Entities;

public class Member
{
    
    private const int MaxActiveLoans = 5;
    private const decimal MaxFinesBeforeSuspension = 10.00m;

    public string MemberId { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public MemberStatus Status { get; private set; }
    public decimal OutstandingFines { get; private set; }

    private readonly List<Loan> _activeLoans = new();
    public IReadOnlyList<Loan> ActiveLoans => _activeLoans.AsReadOnly();


    public Member(string memberId, string name, string email)
    {
       
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("MemberId cannot be null or whitespace.", nameof(memberId));
 
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
        if (!email.Contains('@'))
            throw new ArgumentException("Email must contain '@' character.", nameof(email));
   
        MemberId = memberId;
        Name = name;
        Email = email;
        Status = MemberStatus.Active;
        OutstandingFines = 0m;
    }


    public bool CanBorrow()
    {

        return Status == MemberStatus.Active &&
               _activeLoans.Count < MaxActiveLoans &&
               OutstandingFines < MaxFinesBeforeSuspension;

        //throw new NotImplementedException();
    }


    public void BorrowBook(Book book)
    {


        if (!CanBorrow())
            throw new InvalidOperationException("Member cannot borrow books at this time.");

        book.Borrow(this.MemberId);

        var loan = new Loan(Guid.NewGuid().ToString(), this.MemberId, book, DateTime.Now);
        _activeLoans.Add(loan);

    }

/*************  ✨ Windsurf Command 🌟  *************/
    /// <summary>
    /// Returns a book and removes it from active loans.
    /// </summary>
    /// <param name="loan">The loan to return</param>
    public void ReturnBook(Loan loan)
    {

        if (!_activeLoans.Contains(loan))
            throw new InvalidOperationException("This loan is not active for the member or has already been returned.");
        //throw new NotImplementedException();
        decimal fine = loan.CalculateFine();
        loan.Return(DateTime.Now);
        
        // Mark the loan as returned
        
        if (fine > 0)
            AddFine(fine);

        _activeLoans.Remove(loan);
    }   
  

    /// <summary>
    /// Adds a fine to the member's account.
    /// </summary>
    /// <param name="amount">The fine amount to add</param>
    public void AddFine(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Fine amount cannot be negative.", nameof(amount));
        OutstandingFines += amount;
        if (OutstandingFines >= MaxFinesBeforeSuspension)
            Suspend();
    }

    /// <summary>
    /// Pays off some or all of the outstanding fines.
    /// </summary>
    /// <param name="amount">The amount to pay</param>
    /// <exception cref="ArgumentException">Thrown when amount is negative or exceeds outstanding fines</exception>
    public void PayFine(decimal amount)
    {
        
        // 1. Validate amount is positive and doesn't exceed OutstandingFines
        // 2. Subtract amount from OutstandingFines
        // 3. If member was suspended and fines drop below $10, consider auto-activating (optional)
        if (amount <= 0)
            throw new ArgumentException("Payment amount cannot be negative or zero.", nameof(amount));
        if (amount > OutstandingFines)
            throw new ArgumentException("Payment amount cannot exceed outstanding fines.", nameof(amount));
        OutstandingFines -= amount;

        if (Status == MemberStatus.Suspended && OutstandingFines < MaxFinesBeforeSuspension)
            Activate();
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Suspends the member's account.
    /// </summary>
    public void Suspend()
    {
       
        // Change Status to Suspended
        Status = MemberStatus.Suspended;
        //throw new NotImplementedException();
    }

    /// <summary>
    /// Activates the member's account.
    /// </summary>
    public void Activate()
    {

        Status = MemberStatus.Active;
        //throw new NotImplementedException();
    }


    public void Expire()
    {

        Status = MemberStatus.Expired;
        
    }


    public enum MemberStatus
    {
        Active,
        Suspended,
        Expired
    }
}