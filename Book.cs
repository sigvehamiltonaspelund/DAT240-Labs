using System;

namespace UiS.Dat240.Lab1.Domain.Entities;

public class Book
{
    public string ISBN { get;  }
    public string Title { get;}
    public string Author { get; }
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
        
    }
    
    public void Borrow(string memberId)
    {
        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Member ID cannot be null or empty.", nameof(memberId));

        if (Status != BookStatus.Available)
            throw new InvalidOperationException("Book is not available for borrowing.");
        
        if (ReservedForMemberId != null && ReservedForMemberId != memberId)
            throw new InvalidOperationException("Book is reserved for another member.");

        Status = BookStatus.Borrowed;
        ReservedForMemberId = null; 
    }

    
    public void Return()
    {
        if (Status != BookStatus.Borrowed)
            throw new InvalidOperationException("Only borrowed books can be returned.");


        Status = ReservedForMemberId == null
            ? BookStatus.Available
            : BookStatus.Reserved;
     
    }

    //public void CancelReservation()
    public void Reserve(string memberId)
     {

        if (string.IsNullOrWhiteSpace(memberId))
            throw new ArgumentException("Member ID cannot be null or empty.", nameof(memberId));


        if (Status != BookStatus.Borrowed)
            throw new InvalidOperationException("Only borrowed books can be reserved.");


         if (ReservedForMemberId != null)
            throw new InvalidOperationException("Book is already reserved.");
        
            ReservedForMemberId = memberId;
            Status = BookStatus.Reserved;
     }

    //public void Reserve(string memberId)
    public void CancelReservation()
    {
            if (Status != BookStatus.Reserved)
            throw new InvalidOperationException("Book is not reserved.");

             
        ReservedForMemberId = null; //memberId;
        Status = BookStatus.Available; //Reserved;
    }

    public bool IsAvailableForBorrowing()
    {
        return Status == BookStatus.Available;
    }
    public bool CanBeReserved()
    {
        return Status == BookStatus.Borrowed && ReservedForMemberId == null;
    }
}
    
public enum BookStatus
{
    Available,
    Borrowed,
    Reserved
}
