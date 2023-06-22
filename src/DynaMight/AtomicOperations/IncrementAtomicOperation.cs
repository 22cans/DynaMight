namespace DynaMight.AtomicOperations;

/// <summary>
/// Creates an atomic operation that will use the ADD operation in the DynamoDB with value 1 (`ADD #field 1`).
/// </summary>
public class IncrementAtomicOperation : IncrementByAtomicOperation<int>
{
    
    /// <summary>
    /// Creates an atomic operation that will increment the current value by 1.
    /// </summary>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    public IncrementAtomicOperation(string fieldName) : base(fieldName, 1) { }
}