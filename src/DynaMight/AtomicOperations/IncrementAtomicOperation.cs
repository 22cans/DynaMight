namespace DynaMight.AtomicOperations;

public class IncrementAtomicOperation : IncrementByAtomicOperation<int>
{
    
    /// <summary>
    /// Creates an atomic operation that will increment the current value by 1.
    /// </summary>
    /// <remarks>
    /// If the field does not exist in the register, it will throw an exception. 
    /// </remarks>
    /// <param name="fieldName">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    public IncrementAtomicOperation(string fieldName) : base(fieldName, 1) { }
}