//Chain of resposibility - The intent of this pattern is to avoid coupling the sender of a request to its receiver by giving more than one object a chance to handle the request. It does that by chaining the receiving objects and passing the request along the chain until an object handles it.
//  Use cases: If one or more objects may handle a request and the handler isn't known beforehand, if there is more than one handlers for issue reuqests

using System.ComponentModel.DataAnnotations;

namespace Chain_Of_Responsibility;

//Real life example

public class Document
{
    public string Title { get; set; }
    public DateTimeOffset LastModified { get; set; }
    public bool ApprovedByLitigation { get; set; }
    public bool ApprovedByManagement { get; set; }

    public Document(
        string title,
        DateTimeOffset lastModified,
        bool approvedByLitigation,
        bool approvedByManagement)
    {
        Title = title;
        LastModified = lastModified;
        ApprovedByLitigation = approvedByLitigation;
        ApprovedByManagement = approvedByManagement;
    }
}


/// <summary>
/// Handler
/// </summary> 
public interface IHandler<T> where T : class
{
    IHandler<T> SetSuccessor(IHandler<T> successor);
    void Handle(T request);
}

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentTitleHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;

    public void Handle(Document document)
    {
        if (document.Title == string.Empty)
        {
            // validation doesn't check out
            throw new ValidationException(
                new ValidationResult(
                    "Title must be filled out",
                    new List<string>() { "Title" }), null, null);
        }

        // go to the next handler
        _successor?.Handle(document);
    }

    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }
}

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentLastModifiedHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;

    public void Handle(Document document)
    {
        if (document.LastModified < DateTime.UtcNow.AddDays(-30))
        {
            // validation doesn't check out
            throw new ValidationException(
                new ValidationResult(
                    "Document must be modified in the last 30 days",
                    new List<string>() { "LastModified" }), null, null);
        }

        // go to the next handler
        _successor?.Handle(document);
    }

    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }
}

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentApprovedByLitigationHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;

    public void Handle(Document document)
    {
        if (!document.ApprovedByLitigation)
        {
            // validation doesn't check out
            throw new ValidationException(
                new ValidationResult(
                    "Document must be approved by litigation",
                    new List<string>() { "ApprovedByLitigation" }), null, null);
        }

        // go to the next handler
        _successor?.Handle(document);
    }

    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }
}

/// <summary>
/// ConcreteHandler
/// </summary>
public class DocumentApprovedByManagementHandler : IHandler<Document>
{
    private IHandler<Document>? _successor;

    public void Handle(Document document)
    {
        if (!document.ApprovedByManagement)
        {
            // validation doesn't check out
            throw new ValidationException(
                new ValidationResult(
                    "Document must be approved by management",
                    new List<string>() { "ApprovedByManagement" }), null, null);
        }

        // go to the next handler
        _successor?.Handle(document);
    }

    public IHandler<Document> SetSuccessor(IHandler<Document> successor)
    {
        _successor = successor;
        return successor;
    }
}