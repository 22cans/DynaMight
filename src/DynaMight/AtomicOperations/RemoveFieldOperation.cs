using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace DynaMight.AtomicOperations;

public class RemoveFieldOperation : IAtomicOperation
{
    public virtual string UpdateExpressionType => "REMOVE";
    private string FieldName { get; }

    public virtual string GetUpdateExpression() => $"#{FieldName}";

    public virtual (string key, string value) GetNameExpression() => ($"#{FieldName}", FieldName);

    public virtual (string, AttributeValue AttributeValue, DynamoDBEntry?) GetValueExpression() => (string.Empty, null!, null);

    public RemoveFieldOperation(string fieldName)
    {
        FieldName = fieldName;
    }
}