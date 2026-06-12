
using System;
using System.Collections.Generic;

namespace wsTripXML
{

    public class Items
    {
        public string _ref { get; set; }
    }

    public class SchemaArray
    {
        public string type { get; set; }
        public int minItems { get; set; }
        public Items items { get; set; }
    }

    public class PositiveInteger
    {
        public string type { get; set; }
        public int minimum { get; set; }
    }

    public class AllOf
    {
        public string _ref { get; set; }
        public int? default { get; set; }
    }

    public class PositiveIntegerDefault0
    {
        public IList<AllOf> allOf { get; set; }
    }

    public class SimpleTypes
    {
        public IList<string> enum { get; set; }
    }

    public class Items2
    {
        public string type { get; set; }
    }

    public class StringArray
    {
        public string type { get; set; }
        public Items2 items { get; set; }
        public int minItems { get; set; }
        public bool uniqueItems { get; set; }
    }

    public class Definitions
    {
        public SchemaArray schemaArray { get; set; }
        public PositiveInteger positiveInteger { get; set; }
        public PositiveIntegerDefault0 positiveIntegerDefault0 { get; set; }
        public SimpleTypes simpleTypes { get; set; }
        public StringArray stringArray { get; set; }
    }

    public class Id
    {
        public string type { get; set; }
        public string format { get; set; }
    }

    public class Schema
    {
        public string type { get; set; }
        public string format { get; set; }
    }

    public class Title
    {
        public string type { get; set; }
    }

    public class Description
    {
        public string type { get; set; }
    }

    public class Default
    {
    }

    public class MultipleOf
    {
        public string type { get; set; }
        public int minimum { get; set; }
        public bool exclusiveMinimum { get; set; }
    }

    public class Maximum
    {
        public string type { get; set; }
    }

    public class ExclusiveMaximum
    {
        public string type { get; set; }
        public bool default { get; set; }
    }

    public class Minimum
    {
        public string type { get; set; }
    }

    public class ExclusiveMinimum
    {
        public string type { get; set; }
        public bool default { get; set; }
    }

    public class MaxLength
    {
        public string _ref { get; set; }
    }

    public class MinLength
    {
        public string _ref { get; set; }
    }

    public class Pattern
    {
        public string type { get; set; }
        public string format { get; set; }
    }

    public class AnyOf
    {
        public string type { get; set; }
        public string _ref { get; set; }
    }

    public class Default2
    {
    }

    public class AdditionalItems
    {
        public IList<AnyOf> anyOf { get; set; }
        public Default2 default { get; set; }
    }

    public class AnyOf2
    {
        public string _ref { get; set; }
    }

    public class Default3
    {
    }

    public class Items3
    {
        public IList<AnyOf2> anyOf { get; set; }
        public Default3 default { get; set; }
    }

    public class MaxItems
    {
        public string _ref { get; set; }
    }

    public class MinItems
    {
        public string _ref { get; set; }
    }

    public class UniqueItems
    {
        public string type { get; set; }
        public bool default { get; set; }
    }

    public class MaxProperties
    {
        public string _ref { get; set; }
    }

    public class MinProperties
    {
        public string _ref { get; set; }
    }

    public class Required
    {
        public string _ref { get; set; }
    }

    public class AnyOf3
    {
        public string type { get; set; }
        public string _ref { get; set; }
    }

    public class Default4
    {
    }

    public class AdditionalProperties
    {
        public IList<AnyOf3> anyOf { get; set; }
        public Default4 default { get; set; }
    }

    public class AdditionalProperties2
    {
        public string _ref { get; set; }
    }

    public class Default5
    {
    }

    public class Definitions2
    {
        public string type { get; set; }
        public AdditionalProperties2 additionalProperties { get; set; }
        public Default5 default { get; set; }
    }

    public class AdditionalProperties3
    {
        public string _ref { get; set; }
    }

    public class Default6
    {
    }

    public class Properties2
    {
        public string type { get; set; }
        public AdditionalProperties3 additionalProperties { get; set; }
        public Default6 default { get; set; }
    }

    public class AdditionalProperties4
    {
        public string _ref { get; set; }
    }

    public class Default7
    {
    }

    public class PatternProperties
    {
        public string type { get; set; }
        public AdditionalProperties4 additionalProperties { get; set; }
        public Default7 default { get; set; }
    }

    public class AnyOf4
    {
        public string _ref { get; set; }
    }

    public class AdditionalProperties5
    {
        public IList<AnyOf4> anyOf { get; set; }
    }

    public class Dependencies
    {
        public string type { get; set; }
        public AdditionalProperties5 additionalProperties { get; set; }
    }

    public class Enum
    {
        public string type { get; set; }
        public int minItems { get; set; }
        public bool uniqueItems { get; set; }
    }

    public class Items4
    {
        public string _ref { get; set; }
    }

    public class AnyOf5
    {
        public string _ref { get; set; }
        public string type { get; set; }
        public Items4 items { get; set; }
        public int? minItems { get; set; }
        public bool? uniqueItems { get; set; }
    }

    public class Type
    {
        public IList<AnyOf5> anyOf { get; set; }
    }

    public class AllOf2
    {
        public string _ref { get; set; }
    }

    public class AnyOf6
    {
        public string _ref { get; set; }
    }

    public class OneOf
    {
        public string _ref { get; set; }
    }

    public class Not
    {
        public string _ref { get; set; }
    }

    public class Properties
    {
        public Id id { get; set; }
        public Schema _schema { get; set; }
        public Title title { get; set; }
        public Description description { get; set; }
        public Default default { get; set; }
        public MultipleOf multipleOf { get; set; }
        public Maximum maximum { get; set; }
        public ExclusiveMaximum exclusiveMaximum { get; set; }
        public Minimum minimum { get; set; }
        public ExclusiveMinimum exclusiveMinimum { get; set; }
        public MaxLength maxLength { get; set; }
        public MinLength minLength { get; set; }
        public Pattern pattern { get; set; }
        public AdditionalItems additionalItems { get; set; }
        public Items3 items { get; set; }
        public MaxItems maxItems { get; set; }
        public MinItems minItems { get; set; }
        public UniqueItems uniqueItems { get; set; }
        public MaxProperties maxProperties { get; set; }
        public MinProperties minProperties { get; set; }
        public Required required { get; set; }
        public AdditionalProperties additionalProperties { get; set; }
        public Definitions2 definitions { get; set; }
        public Properties2 properties { get; set; }
        public PatternProperties patternProperties { get; set; }
        public Dependencies dependencies { get; set; }
        public Enum enum { get; set; }
        public Type type { get; set; }
        public AllOf2 allOf { get; set; }
        public AnyOf6 anyOf { get; set; }
        public OneOf oneOf { get; set; }
        public Not not { get; set; }
    }

    public class Dependencies2
    {
        public IList<string> exclusiveMaximum { get; set; }
        public IList<string> exclusiveMinimum { get; set; }
    }

    public class Default8
    {
    }

    public class Contract
    {
        public string id { get; set; }
        public string _schema { get; set; }
        public string description { get; set; }
        public Definitions definitions { get; set; }
        public string type { get; set; }
        public Properties properties { get; set; }
        public Dependencies2 dependencies { get; set; }
        public Default8 default { get; set; }
    }

}
