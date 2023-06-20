using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.AtomicOperations;
using DynaMight.Criteria;

namespace DynaMight.Builders;

/// <summary>
/// Basic interface for Atomic Builder
/// </summary>
public interface IAtomicBuilder : IDynamoBuilder
{
    /// <summary>
    /// Adds a new Operation into the builder if the condition is true
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the operation</param>
    /// <param name="func">The operation that will be added into the list. It uses a Func, which will be normally a
    /// `() => condition`.</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder AddOperation(bool condition, Func<IAtomicOperation> func);
    /// <summary>
    /// Adds a new Operation into the builder
    /// </summary>
    /// <param name="atomicOperation">The operation that will be added into the list</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder AddOperation(IAtomicOperation atomicOperation);
    /// <summary>
    /// Sets the Key for the Builder. If the table has a Hash and Sort key, use the method twice, one of each key.
    /// </summary>
    /// <param name="name">The field's name that will be added. Recommended to use `nameof` function to avoid issues when renaming.</param>
    /// <param name="value">The key value</param>
    /// <typeparam name="TK">The type of the key (int, long, string, etc)</typeparam>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder SetKey<TK>(string name, TK value);
    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="condition">The condition that needs to be satisfied to add the criteria</param>
    /// <param name="func">The criteria that will be added into the list. It uses a Func, which will be normally a
    /// `() => criteria`.</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder AddCriteria(bool condition, Func<IDynamoCriteria> func);
    /// <summary>
    /// Adds a new Criteria into the builder
    /// </summary>
    /// <param name="criteria">The criteria that will be added into the builder</param>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder AddCriteria(IDynamoCriteria criteria);
    /// <summary>
    /// Build the DynamoDB object that will be used to do the atomic operation. Called internally by the
    /// DynaMightContext.
    /// </summary>
    /// <returns>An `UpdateItemRequest` object</returns>
    UpdateItemRequest Build();
    /// <summary>
    /// Defines if the build expression will use parenthesis when creating the build expression.
    /// Recommended to set True if you are using several AND/OR criteria
    /// </summary>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder UseParenthesis();
    /// <summary>
    /// Set the Table's name, based on the DynamoDBContextConfig and the DynamoDBTable attribute
    /// </summary>
    /// <param name="dynamoConfig">The DynamoDBContextConfig used by the application</param>
    /// <typeparam name="T">The table's type in the C# code</typeparam>
    /// <returns>The AtomicBuilder (for a fluent API usage)</returns>
    IAtomicBuilder SetTableName<T>(DynamoDBContextConfig dynamoConfig);
}