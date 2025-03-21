﻿namespace Microsoft.ApplicationInsights.Extensibility.Filtering
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Filter determines whether a telemetry document matches the criterion.
    /// The filter's configuration (condition) is specified in a <see cref="FilterInfo"/> DTO.
    /// </summary>
    /// <typeparam name="TTelemetry">Type of telemetry documents.</typeparam>
    internal class Filter<TTelemetry>
    {
        private const string FieldNameCustomDimensionsPrefix = "CustomDimensions.";

        private const string FieldNameCustomMetricsPrefix = "CustomMetrics.";

        private const string FieldNameAsterisk = "*";

        private const string CustomMetricsPropertyName = "Metrics";

        private const string CustomDimensionsPropertyName = "Properties";

        private const char FieldNameTrainSeparator = '.';

        private static readonly MethodInfo DoubleToStringMethodInfo = GetMethodInfo<double, string>(x => x.ToString(CultureInfo.InvariantCulture));

        private static readonly MethodInfo NullableDoubleToStringMethodInfo = GetMethodInfo<double?, string>(x => x.ToString());

        private static readonly MethodInfo ObjectToStringMethodInfo = GetMethodInfo<object, string>(x => x.ToString());

        private static readonly MethodInfo ValueTypeToStringMethodInfo = GetMethodInfo<ValueType, string>(x => x.ToString());

        private static readonly MethodInfo UriToStringMethodInfo = GetMethodInfo<Uri, string>(x => x.ToString());

        private static readonly MethodInfo StringIndexOfMethodInfo =
            GetMethodInfo<string, string, int>((x, y) => x.IndexOf(y, StringComparison.OrdinalIgnoreCase));

        private static readonly MethodInfo StringEqualsMethodInfo =
            GetMethodInfo<string, string, bool>((x, y) => x.Equals(y, StringComparison.OrdinalIgnoreCase));

        private static readonly MethodInfo DoubleTryParseMethodInfo = typeof(double).GetMethod(
            "TryParse",
            new[] { typeof(string), typeof(NumberStyles), typeof(IFormatProvider), typeof(double).MakeByRefType() });

        private static readonly MethodInfo DictionaryStringStringTryGetValueMethodInfo = typeof(IDictionary<string, string>).GetMethod("TryGetValue");

        private static readonly MethodInfo DictionaryStringDoubleTryGetValueMethodInfo = typeof(IDictionary<string, double>).GetMethod("TryGetValue");

        private static readonly MethodInfo DictionaryStringStringScanMethodInfo =
            GetMethodInfo<IDictionary<string, string>, string, bool>((dict, searchValue) => Filter<int>.ScanDictionary(dict, searchValue));

        private static readonly MethodInfo DictionaryStringDoubleScanMethodInfo =
           GetMethodInfo<IDictionary<string, double>, string, bool>((dict, searchValue) => Filter<int>.ScanDictionary(dict, searchValue));

        private static readonly ConstantExpression DoubleDefaultNumberStyles = Expression.Constant(NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent);
        private static readonly ConstantExpression InvariantCulture = Expression.Constant(CultureInfo.InvariantCulture);

        private readonly Func<TTelemetry, bool> filterLambda;

        private readonly double? comparandDouble;

        private readonly bool? comparandBoolean;

        private readonly TimeSpan? comparandTimeSpan;

        private readonly string comparand;

        private readonly Predicate predicate;

        private readonly string fieldName;

        private readonly FilterInfo info;

        public Filter(FilterInfo filterInfo)
        {
            ValidateInput(filterInfo);

            this.info = filterInfo;

            this.fieldName = filterInfo.FieldName;
            this.predicate = filterInfo.Predicate;
            this.comparand = filterInfo.Comparand;

            FieldNameType fieldNameType;
            Type fieldType = GetFieldType(filterInfo.FieldName, out fieldNameType);
            this.ThrowOnInvalidFilter(
                null,
                fieldNameType == FieldNameType.AnyField && this.predicate != Predicate.Contains && this.predicate != Predicate.DoesNotContain);

            double comparandDouble;
            this.comparandDouble = double.TryParse(filterInfo.Comparand, NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite | NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out comparandDouble) ? comparandDouble : (double?)null;

            bool comparandBoolean;
            this.comparandBoolean = bool.TryParse(filterInfo.Comparand, out comparandBoolean) ? comparandBoolean : (bool?)null;

            TimeSpan comparandTimeSpan;
            this.comparandTimeSpan = TimeSpan.TryParse(filterInfo.Comparand, CultureInfo.InvariantCulture, out comparandTimeSpan)
                                         ? comparandTimeSpan
                                         : (TimeSpan?)null;

            ParameterExpression documentExpression = Expression.Variable(typeof(TTelemetry));

            Expression comparisonExpression;

            try
            {
                if (fieldNameType == FieldNameType.AnyField)
                {
                    // multiple fields => multiple comparison expressions connected with ORs
                    comparisonExpression = this.ProduceComparatorExpressionForAnyFieldCondition(documentExpression);
                }
                else
                {
                    // a single field filterInfo.FieldName of type fieldType => a single comparison expression
                    Expression fieldExpression = ProduceFieldExpression(documentExpression, filterInfo.FieldName, fieldNameType);

                    comparisonExpression = this.ProduceComparatorExpressionForSingleFieldCondition(fieldExpression, fieldType);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "Could not construct the filter."), e);
            }

            try
            {
                Expression<Func<TTelemetry, bool>> lambdaExpression = Expression.Lambda<Func<TTelemetry, bool>>(
                    comparisonExpression,
            catch (Exception e)
            {
                throw new ArgumentOutOfRangeException(string.Format(CultureInfo.InvariantCulture, "Could not compile the filter."), e);
            }
        }

        internal enum FieldNameType
        {
            FieldName,

            switch (fieldNameType)
            {
                case FieldNameType.FieldName:
                    return fieldName.Split(FieldNameTrainSeparator).Aggregate<string, Expression>(documentExpression, Expression.Property);
                case FieldNameType.CustomMetricName:
                    string customMetricName = fieldName.Substring(
                        FieldNameCustomMetricsPrefix.Length,
                        fieldName.Length - FieldNameCustomMetricsPrefix.Length);

                    return CreateDictionaryAccessExpression(
                        documentExpression,
                        CustomMetricsPropertyName,
                        DictionaryStringDoubleTryGetValueMethodInfo,
                        typeof(double),
                        customMetricName);
                case FieldNameType.CustomDimensionName:
                    string customDimensionName = fieldName.Substring(
                        FieldNameCustomDimensionsPrefix.Length,
                        fieldName.Length - FieldNameCustomDimensionsPrefix.Length);

        {
            if (fieldName.StartsWith(FieldNameCustomDimensionsPrefix, StringComparison.Ordinal))
            {
                fieldNameType = FieldNameType.CustomDimensionName;
                return typeof(string);
            }

            if (fieldName.StartsWith(FieldNameCustomMetricsPrefix, StringComparison.Ordinal))
            {
                fieldNameType = FieldNameType.CustomMetricName;
            // no special case in filterInfo.FieldName, treat it as the name of a property in TTelemetry type
            fieldNameType = FieldNameType.FieldName;
            return GetPropertyTypeFromFieldName(fieldName);
        }

        private static Expression CreateDictionaryAccessExpression(ParameterExpression documentExpression, string dictionaryName, MethodInfo tryGetValueMethodInfo, Type valueType, string keyValue)
        {
            // valueType value;
            // document.dictionaryName.TryGetValue(keyValue, out value)
            // return value;
            // a block will "return" its last expression
            return Expression.Block(valueType, new[] { valueVariable }, tryGetValueCall, valueVariable);
        }

        private static MethodInfo GetMethodInfo<T, TResult>(Expression<Func<T, TResult>> expression)
        {
            if (expression.Body is MethodCallExpression member)
            {
                return member.Method;
            }
        {
            if (expression.Body is MethodCallExpression member)
            {
                return member.Method;
            }

            throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Expression is not a method"), nameof(expression));
        }

        private static Type GetPropertyTypeFromFieldName(string fieldName)
            {
                Type propertyType = fieldName.Split(FieldNameTrainSeparator)
                    .Aggregate(
                        typeof(TTelemetry),
                        (type, propertyName) => type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public).PropertyType);

                if (propertyType == null)
                {
                    string propertyNotFoundMessage = string.Format(
                        CultureInfo.InvariantCulture,
                        "Error finding property {0} in the type {1}",
                        fieldName,
                        typeof(TTelemetry).FullName);

                    throw new ArgumentOutOfRangeException(nameof(fieldName), propertyNotFoundMessage);
                }

                return propertyType;
            }
            catch (Exception e)
            }
        }

        private static bool ScanDictionary(IDictionary<string, string> dict, string searchValue)
        {
            return dict?.Values.Any(val => (val ?? string.Empty).IndexOf(searchValue ?? string.Empty, StringComparison.OrdinalIgnoreCase) != -1)
                   ?? false;
        }

        private static bool ScanDictionary(IDictionary<string, double> dict, string searchValue)
        {
            return dict?.Values.Any(val => val.ToString(CultureInfo.InvariantCulture).IndexOf(searchValue, StringComparison.OrdinalIgnoreCase) != -1)
                   ?? false;
        }

        private Expression ProduceComparatorExpressionForSingleFieldCondition(Expression fieldExpression, Type fieldType, bool isFieldTypeNullable = false)
        {
            // this must determine an appropriate runtime comparison given the field type, the predicate, and the comparand
            TypeCode fieldTypeCode = Type.GetTypeCode(fieldType);
            switch (fieldTypeCode)
            {
                case TypeCode.Boolean:
                    {
                        this.ThrowOnInvalidFilter(fieldType, !this.comparandBoolean.HasValue);

                        switch (this.predicate)
                        {
                            case Predicate.Equal:
                                // fieldValue == this.comparandBoolean.Value;
                                return Expression.Equal(fieldExpression, Expression.Constant(this.comparandBoolean.Value, isFieldTypeNullable ? typeof(bool?) : typeof(bool)));
                                    // (int)fieldValue < (int)enumValue
                                    // (int?)fieldValue < (int?)enumValue
                                    Type underlyingType = isFieldTypeNullable ? typeof(Nullable<>).MakeGenericType(enumUnderlyingType) : enumUnderlyingType;
                                    return Expression.LessThan(
                                        Expression.Convert(fieldExpression, underlyingType),
                                        Expression.Convert(Expression.Constant(enumValue, fieldType), underlyingType));
                                case Predicate.GreaterThan:
                                    // (int)fieldValue > (int)enumValue
                                    // (int?)fieldValue > (int?)enumValue
                                    underlyingType = isFieldTypeNullable ? typeof(Nullable<>).MakeGenericType(enumUnderlyingType) : enumUnderlyingType;
                                    return Expression.GreaterThanOrEqual(
                                        Expression.Convert(fieldExpression, underlyingType),
                                        Expression.Convert(Expression.Constant(enumValue, fieldType), underlyingType));
                                case Predicate.Contains:
                                    // fieldValue.ToString(CultureInfo.InvariantCulture).IndexOf(this.comparand, StringComparison.OrdinalIgnoreCase) != -1
                                    Expression toStringCall = Expression.Call(fieldExpression, isFieldTypeNullable ? ValueTypeToStringMethodInfo : ObjectToStringMethodInfo);
                                    Expression indexOfCall = Expression.Call(toStringCall, StringIndexOfMethodInfo, Expression.Constant(this.comparand), Expression.Constant(StringComparison.OrdinalIgnoreCase));
                                    return Expression.NotEqual(indexOfCall, Expression.Constant(-1));
                                case Predicate.DoesNotContain:
                                    // fieldValue.ToString(CultureInfo.InvariantCulture).IndexOf(this.comparand, StringComparison.OrdinalIgnoreCase) == -1
                        {
                            // this is a regular numerical type
                            // in order for the expression to compile, we must cast to double unless it's already double
                            // we're using double as the lowest common denominator for all numerical types
                                default:
                                    this.ThrowOnInvalidFilter(fieldType);
                                    break;
                            }
                        }
                    }

                    break;
                case TypeCode.String:
                    {
                                // (fieldValue ?? string.Empty).Equals(this.comparand, StringComparison.OrdinalIgnoreCase)
                                return Expression.Call(fieldValueOrEmptyString, StringEqualsMethodInfo, Expression.Constant(this.comparand), Expression.Constant(StringComparison.OrdinalIgnoreCase));
                            case Predicate.NotEqual:
                                // !(fieldValue ?? string.Empty).Equals(this.comparand, StringComparison.OrdinalIgnoreCase)
                                return Expression.Not(Expression.Call(fieldValueOrEmptyString, StringEqualsMethodInfo, Expression.Constant(this.comparand), Expression.Constant(StringComparison.OrdinalIgnoreCase)));
                            case Predicate.LessThan:
                            case Predicate.GreaterThan:
                            case Predicate.LessThanOrEqual:
                            case Predicate.GreaterThanOrEqual:
                                // double.TryParse(fieldValue, out temp) && temp {<, <=, >, >=} comparandDouble
                                this.ThrowOnInvalidFilter(fieldType);
                                break;
                        }
                    }

                    break;
                default:
                    Type nullableUnderlyingType;
                    if (fieldType == typeof(TimeSpan))
                    {
                        this.ThrowOnInvalidFilter(fieldType, !this.comparandTimeSpan.HasValue);

                        switch (this.predicate)
                        {
                            case Predicate.Equal:
                                Func<TimeSpan, bool> comparator = fieldValue => fieldValue == this.comparandTimeSpan.Value;
                                return Expression.Call(Expression.Constant(comparator.Target), comparator.GetMethodInfo(), fieldExpression);
                            case Predicate.NotEqual:
                                comparator = fieldValue => fieldValue != this.comparandTimeSpan.Value;
                                return Expression.Call(Expression.Constant(comparator.Target), comparator.GetMethodInfo(), fieldExpression);
                            default:
                                this.ThrowOnInvalidFilter(fieldType);
                                break;
                        }
                    }
                    else if (fieldType == typeof(Uri))
                    {
                        Expression toStringCall = Expression.Call(fieldExpression, UriToStringMethodInfo);

                        Expression fieldValueOrEmptyString = Expression.Condition(Expression.Equal(fieldExpression, Expression.Constant(null)), Expression.Constant(string.Empty), toStringCall);
                            default:
                                this.ThrowOnInvalidFilter(fieldType);
                                break;
                        }
                    }
                    else if ((nullableUnderlyingType = Nullable.GetUnderlyingType(fieldType)) != null)
                    {
                        // make a recursive call for the underlying type
                        return ProduceComparatorExpressionForSingleFieldCondition(fieldExpression, nullableUnderlyingType, true);
                    }
            return null;
        }

        private Expression ProduceComparatorExpressionForAnyFieldCondition(ParameterExpression documentExpression)
        {
            // this.predicate is either Predicate.Contains or Predicate.DoesNotContain at this point
            if (this.predicate != Predicate.Contains && this.predicate != Predicate.DoesNotContain)
            {
                throw new InvalidOperationException(
                    "ProduceComparatorExpressionForAnyFieldCondition is called while this.predicate is neither Predicate.Contains nor Predicate.DoesNotContain");
            }

            Expression comparisonExpression = this.predicate == Predicate.Contains ? Expression.Constant(false) : Expression.Constant(true);

            foreach (PropertyInfo propertyInfo in typeof(TTelemetry).GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                try
                {
                    Expression propertyComparatorExpression;
                    if (string.Equals(propertyInfo.Name, CustomDimensionsPropertyName, StringComparison.Ordinal))
                            null,
                            DictionaryStringStringScanMethodInfo,
                            customDimensionsProperty,
                            Expression.Constant(this.comparand));

                        if (this.predicate == Predicate.DoesNotContain)
                        {
                            propertyComparatorExpression = Expression.Not(propertyComparatorExpression);
                        }
                    }
                            null,
                            DictionaryStringDoubleScanMethodInfo,
                            customMetricsProperty,
                            Expression.Constant(this.comparand));

                        if (this.predicate == Predicate.DoesNotContain)
                        {
                            propertyComparatorExpression = Expression.Not(propertyComparatorExpression);
                        }
                    }
            return comparisonExpression;
        }

        private Expression CreateStringToDoubleComparisonBlock(Expression fieldExpression, Predicate predicate)
        {
            ParameterExpression tempVariable = Expression.Variable(typeof(double));
            MethodCallExpression doubleTryParseCall = Expression.Call(DoubleTryParseMethodInfo, fieldExpression, DoubleDefaultNumberStyles, InvariantCulture, tempVariable);

            BinaryExpression comparisonExpression;
            switch (predicate)


# This file contains partial code from the original project
# Some functionality may be missing or incomplete
