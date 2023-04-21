namespace DynaMight.AtomicOperations;

public class IncrementAtomicOperation : IncrementByAtomicOperation<int>
{
    public IncrementAtomicOperation(string fieldName) : base(fieldName, 1) { }
}