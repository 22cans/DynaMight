using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using DynaMight.Extensions;

namespace DynaMight.Converters;

public class DynaMightCustomConverterAttribute : Attribute
{
}

// public class DynaMightCustomConverterAttribute<T> : DynaMightCustomConverterAttribute
// {
//     internal readonly Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> Action;
//     internal readonly Func<T, AttributeValue> AttributeValue;
//     internal readonly Func<T, DynamoDBEntry?> DynamoDbEntry;
//
//     public DynaMightCustomConverterAttribute() : this(
//         (obj, prop, dict) => prop.SetValue(obj, Convert.ChangeType(int.Parse(dict[prop.Name].N), typeof(T))),
//         r => new AttributeValue { N = Convert.ToInt32(r).ToString() },
//         r => Convert.ToInt32(r))
//     {
//     }
//
//     public DynaMightCustomConverterAttribute(
//         Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action,
//         Func<T, AttributeValue> attributeValue,
//         Func<T, DynamoDBEntry?> dynamoDbEntry)
//     {
//         Action = action;
//         AttributeValue = attributeValue;
//         DynamoDbEntry = dynamoDbEntry;
//     }
// }
//
// public class DynaMightEnumCustomConverterAttribute<T> : DynaMightCustomConverterAttribute<T> where T : Enum
// {
//     // public DynaMightEnumCustomConverterAttribute() : this(
//     //     (obj, prop, dict) => prop.SetValue(obj, Convert.ChangeType(int.Parse(dict[prop.Name].N), typeof(T))),
//     //     r => new AttributeValue { N = Convert.ToInt32(r).ToString() },
//     //     r => Convert.ToInt32(r)
//     // )
//     // {
//     // }
//     //
//     // public DynaMightEnumCustomConverterAttribute(
//     //     Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action,
//     //     Func<T, AttributeValue> attributeValue,
//     //     Func<T, DynamoDBEntry?> dynamoDbEntry) : base(action, attributeValue, dynamoDbEntry)
//     // {
//     // }
//
//     public DynaMightEnumCustomConverterAttribute()
//     {
//     }
//
//     public DynaMightEnumCustomConverterAttribute(
//         Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action,
//         Func<T, AttributeValue> attributeValue, Func<T, DynamoDBEntry?> dynamoDbEntry) :
//         base(action, attributeValue, dynamoDbEntry)
//     {
//     }
// }
//
// // public class DynaMightNullableEnumCustomConverterAttribute<T> : DynaMightEnumCustomConverterAttribute<T> where T : Enum
// // {
// //     public DynaMightNullableEnumCustomConverterAttribute() : this(
// //         (obj, prop, dict) => prop.SetValue(obj, Convert.ChangeType(int.Parse(dict[prop.Name].N), typeof(T))),
// //         r => new AttributeValue { N = Convert.ToInt32(r).ToString() },
// //         r => Convert.ToInt32(r)
// //     )
// //     {
// //     }
// //
// //     public DynaMightNullableEnumCustomConverterAttribute(
// //         Action<object, PropertyInfo, IReadOnlyDictionary<string, AttributeValue>> action,
// //         Func<T?, AttributeValue> attributeValue,
// //         Func<T?, DynamoDBEntry?> dynamoDbEntry) : base(action, attributeValue, dynamoDbEntry)
// //     {
// //         AttributeValueDictionaryConverter.AddCustomConverter<T>((obj, prop, dict) => prop.SetValue(obj,
// //             dict.Get<T?>(prop.Name, x => (T?)(object)int.Parse(x.N))));
// //
// //         // DynaMightCustomConverter.Register<T?>(
// //         //     (obj, prop, dict) => prop.SetValue(obj,
// //         //         dict.Get<T?>(prop.Name, x => (T)Convert.ChangeType(int.Parse(x.N), typeof(T)))),
// //         //     r => r is null
// //         //         ? new AttributeValue { N = Convert.ToInt32(r).ToString() }
// //         //         : new AttributeValue { NULL = true }
// //         //     , r => r is null ? null : Convert.ToInt32(r));
// //     }
// // }