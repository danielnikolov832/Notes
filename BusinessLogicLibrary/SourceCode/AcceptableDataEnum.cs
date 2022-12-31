using System.Collections.Generic;
using System;

using BusinessLogicLibrary.Models;
using BusinessLogicLibrary;

namespace BusinessLogicLibrary;

// If you add elements to this enum, you must follow the system of definition of new elements explained in the comments below

// This is so that there are no drawbacks for the mutation of this enum, which is supposed to mutate upon new 
// features, which means frequently and along with new code for those features (increasing overall system complexity)
// Therefore there are a few different places in code where the data that needs to be extracted for the particular enum is stored
// These places are listed in the below comments, so mutation of them to comform to the new data in the enum ensures safety of the system
public enum AcceptableDataEnum
{
    Text = 0,
    Integer = 1,
    Decimal = 2,
    Currency = 3,
    Checkmark = 4,
    Date = 5,
    Time = 6,
    DateAndTime = 7
}

internal interface IAcceptableDataEnumDefaultValueHolder
{
    object get_defaultValue { get; init; }
    Type get_Type { get; init; }
}

internal interface IAcceptableDataEnumDefaultValueHolderInternal
{
    internal IValueContainerNonGeneric CreateInstanceOfValueContainer(string name);
}

internal abstract class AcceptableDataEnumDefaultValueHolderBase : IAcceptableDataEnumDefaultValueHolder, IAcceptableDataEnumDefaultValueHolderInternal
{
    protected AcceptableDataEnumDefaultValueHolderBase(AcceptableDataEnum get_dataEnum, object get_defaultValue, Type get_Type)
    {
        this.get_dataEnum = get_dataEnum;
        this.get_defaultValue = get_defaultValue;
        this.get_Type = get_Type;
    }

    public AcceptableDataEnum get_dataEnum { get; init; }
    public object get_defaultValue { get; init; }
    public Type get_Type { get; init; }

    public abstract IValueContainerNonGeneric CreateInstanceOfValueContainer(string name);
}

internal sealed class AcceptableDataEnumDefaultValueHolder<T> : AcceptableDataEnumDefaultValueHolderBase
    where T : notnull
{
    internal AcceptableDataEnumDefaultValueHolder(AcceptableDataEnum dataEnum, T defaultValue)
        : base(dataEnum, defaultValue, typeof(T))
    {
    }

    public override IValueContainerNonGeneric CreateInstanceOfValueContainer(string name)
    {
        return new ValueContainer<T>(name, (T)get_defaultValue, get_dataEnum);
    }
}

// System for defining new elements
// 1. Add an entry to the dictionary "AcceptableDataEnumDefaultValuesHolder._acceptableDataEnumDefaultValues" with 
// the "AcceptableDataEnum" value and the data for its behaviour

// 2. Add an entry to the dictionary "AcceptableDataEnumConverterUtils.acceptableDataEnumAndAcceptableDataValuesHolderPairs" in WPFUI with 
// the "AcceptableDataEnum" value and the data for its behaviour

public static class AcceptableDataEnumDefaultValuesHolder
{
    private static readonly IReadOnlyDictionary<AcceptableDataEnum, AcceptableDataEnumDefaultValueHolderBase> _acceptableDataEnumDefaultValues =
    new Dictionary<AcceptableDataEnum, AcceptableDataEnumDefaultValueHolderBase>()
    {
        {AcceptableDataEnum.Text, new AcceptableDataEnumDefaultValueHolder<string>(
            AcceptableDataEnum.Text,
            string.Empty)},

        {AcceptableDataEnum.Integer, new AcceptableDataEnumDefaultValueHolder<long>(
            AcceptableDataEnum.Integer,
            default)},

        {AcceptableDataEnum.Decimal, new AcceptableDataEnumDefaultValueHolder<decimal>(
            AcceptableDataEnum.Decimal,
            default)},

        {AcceptableDataEnum.Currency, new AcceptableDataEnumDefaultValueHolder<decimal>(
            AcceptableDataEnum.Currency,
            default)},

        {AcceptableDataEnum.Checkmark, new AcceptableDataEnumDefaultValueHolder<bool>(
            AcceptableDataEnum.Checkmark,
            default)},

        {AcceptableDataEnum.Date, new AcceptableDataEnumDefaultValueHolder<DateOnly>(
            AcceptableDataEnum.Date,
            DateOnly.FromDateTime(DateTime.Today))},

        {AcceptableDataEnum.Time, new AcceptableDataEnumDefaultValueHolder<TimeOnly>(
            AcceptableDataEnum.Time,
            TimeOnly.FromDateTime(DateTime.Now))},

        {AcceptableDataEnum.DateAndTime, new AcceptableDataEnumDefaultValueHolder<DateTime>(
            AcceptableDataEnum.DateAndTime,
            DateTime.Now)}
    };

    public static IValueContainerNonGeneric CreateValueContainerWithDefaultValue(this AcceptableDataEnum acceptableDataEnum, string name)
    {
        return _acceptableDataEnumDefaultValues[acceptableDataEnum].CreateInstanceOfValueContainer(name);
    }

    public static object GetDefaultValue(this AcceptableDataEnum acceptableDataEnum)
    {
        return _acceptableDataEnumDefaultValues[acceptableDataEnum].get_defaultValue;
    }
}