//using System.ComponentModel;

namespace UiS.Dat240.Lab1.Domain.Entities;

public class Book
{
    public string ISBN { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public BookStatus Status { get; private set; }
    public string? ReservedForMemberId { get; private set; }

    
   
    public Book(string isbn, string title, string author)
    {
        
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("ISBN cannot be null or empty.", nameof(isbn));
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Title cannot be null or empty", nameof(title));
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Author cannot be null or empty", nameof(author));

        ISBN = isbn;
        Title = title;
        Author = author;
        Status = BookStatus.Available;
        ReservedForMemberId = null;
    }
    
    public void Borrow(string memberId)
    {
        
        if (!IsAvailableForBorrowing())
            throw new InvalidOperationException("Book is not available for borrowing.");
      
        if (String.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Member ID cannot be null or empty.", nameof(memberId));
        // Change status to Borrowed
        Status = BookStatus.Borrowed;
        ReservedForMemberId = null; 
    }

    
    public void Return()
    {
        Status = BookStatus.Available; 
        ReservedForMemberId = null;
     
    }

    public void CancelReservation()
     {

        if (Status != BookStatus.Reserved)  
            throw new InvalidOperationException("Book is not reserved.");
        
            Status = BookStatus.Available;
            ReservedForMemberId = null;
     }

    public void Reserve(string memberId)
    {
        
        if (!CanBeReserved())
            throw new InvalidOperationException("Book cannot be reserved in its current state.");
        // Can only reserve if status is Borrowed (not Available or already Reserved)
        if (String.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Member ID cannot be null or empty.", nameof(memberId));
        
        ReservedForMemberId = memberId;
        Status = BookStatus.Reserved;
    }

    public bool IsAvailableForBorrowing()
    {
        return Status == BookStatus.Available;
    }
    public bool CanBeReserved()
    {
        return Status == BookStatus.Available;//Borrowed && string.IsNullOrWhiteSpace(ReservedForMemberId);
    }
}
    
public enum BookStatus
{
    Available,
    Borrowed,
    Reserved
}
