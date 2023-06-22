using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Converters;

namespace DynaMight.AtomicOperations;

/// <inheritdoc />
public class IncrementByAtomicOperation<T> : AddAtomicOperation<T>
{
    /// <inheritdoc />
    public IncrementByAtomicOperation(string fieldName, T increment) : base(fieldName, increment)
    {
    }
}