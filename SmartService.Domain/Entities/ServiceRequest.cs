using SmartService.Domain.Interfaces;
using SmartService.Domain.ValueObjects;
using SmartService.Domain.Exceptions;

namespace SmartService.Domain.Entities;

public class ServiceRequest : IAggregateRoot
{
    public Guid Id { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid CategoryId { get; private set; }

    public string Description { get; private set; }
    public ServiceComplexity Complexity { get; private set; }
    public ServiceStatus Status { get; private set; }
    public Guid? AssignedProviderId { get; private set; }
    public Money? EstimatedCost { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private ServiceRequest() { }

    private ServiceRequest(Guid id, Guid customerId, Guid categoryId, string description)
    {
        Id = id;
        CustomerId = customerId;
        CategoryId = categoryId;
        Description = description;
        Status = ServiceStatus.Created;
        CreatedAt = DateTime.UtcNow;
    }

    // ✅ Factory Method – CHỈ DÙNG KHI CREATE
    public static ServiceRequest Create(
        Guid customerId,
        Guid categoryId,
        string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required.");

        return new ServiceRequest(
            Guid.NewGuid(),
            customerId,
            categoryId,
            description);
    }

    // 🧠 Domain Behaviors

    public void Evaluate(ServiceComplexity complexity)
    {
        if (Status != ServiceStatus.Created)
            throw new DomainException("Service request must be in Created state.");

        Complexity = complexity;
        Status = ServiceStatus.Evaluating;
    }

    public void AssignProvider(Guid providerId, Money estimatedCost)
    {
        if (Status != ServiceStatus.Evaluating)
            throw new DomainException("Service request must be evaluated first.");

        AssignedProviderId = providerId;
        EstimatedCost = estimatedCost;
        Status = ServiceStatus.Assigned;
    }

    public void Start()
    {
        if (Status != ServiceStatus.Assigned)
            throw new DomainException("Service request must be assigned.");

        Status = ServiceStatus.InProgress;
    }

    public void Complete()
    {
        if (Status != ServiceStatus.InProgress)
            throw new DomainException("Service request must be in progress.");

        Status = ServiceStatus.Completed;
    }
}
