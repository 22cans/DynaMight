using DynaMight.Criteria;
using FluentAssertions;

namespace DynaMight.UnitTests.Criteria;

public class CriteriaTests
{
    private const string FieldName = "field";
    private const string FieldValue = "value";

    [Fact]
    public void And()
    {
        var left = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var right = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var criteria = new AndDynamoCriteria(left, right);
        
        criteria.Should().NotBeNull();
        criteria.ToString(true).Should().MatchRegex($"\\(\\#{FieldName} = :{FieldName}_.{{32}} AND \\#{FieldName} = :{FieldName}_.{{32}}\\)");
    }
    
    [Fact]
    public void Contains()
    {
        var criteria = new ContainsDynamoCriteria<string>(FieldName, FieldValue);

        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"contains\\(\\#{FieldName}, :{FieldName}_.{{32}}\\)");
    }
    
    [Fact]
    public void In()
    {
        var criteria = new InDynamoCriteria<string>(FieldName, new[]{ FieldValue, FieldValue });
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} IN \\(:{FieldName}_.{{32}}, :{FieldName}_.{{32}}\\)");
    }

    [Fact]
    public void Equal()
    {
        var criteria = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} = :{FieldName}_.{{32}}");
    }
    
    [Fact]
    public void Greater()
    {
        var criteria = new GreaterDynamoCriteria<string>(FieldName, FieldValue);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} > :{FieldName}_.{{32}}");
    }
    
    [Fact]
    public void GreaterOrEqual()
    {
        var criteria = new GreaterOrEqualDynamoCriteria<string>(FieldName, FieldValue);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} >= :{FieldName}_.{{32}}");
    }
    
    [Fact]
    public void Less()
    {
        var criteria = new LessDynamoCriteria<string>(FieldName, FieldValue);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} < :{FieldName}_.{{32}}");
    }
    
    [Fact]
    public void LessOrEqual()
    {
        var criteria = new LessOrEqualDynamoCriteria<string>(FieldName, FieldValue);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} <= :{FieldName}_.{{32}}");
    }

    [Fact]
    public void NotExists()
    {
        var criteria = new NotExistsDynamoCriteria(FieldName);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex("attribute_not_exists\\(\\#field\\)");
    }
    
    [Fact]
    public void Exists()
    {
        var criteria = new ExistsDynamoCriteria(FieldName);
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex("attribute_exists\\(\\#field\\)");
    }

    [Fact]
    public void Or()
    {
        var left = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var right = new EqualDynamoCriteria<string>(FieldName, FieldValue);
        var criteria = new OrDynamoCriteria(left, right);
        
        criteria.Should().NotBeNull();
        criteria.ToString().Should().MatchRegex($"\\#{FieldName} = :{FieldName}_.{{32}} OR \\#{FieldName} = :{FieldName}_.{{32}}");
    }
}