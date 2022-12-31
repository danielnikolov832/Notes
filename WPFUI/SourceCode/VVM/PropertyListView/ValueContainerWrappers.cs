using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using WPFUI.Constants;

using BusinessLogicLibrary.Models;
using BusinessLogicLibrary;
using WPFUI.VVM.Core;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFUI.VVM.PropertyListView;

public sealed class PropertyListItem : ObservableObject
{
    public PropertyListItem(IValueContainerWrapperNonGeneric valueContainerWrapper)
    {
        _valueContainerWrapper = valueContainerWrapper;
        _dataHolder = valueContainerWrapper.get_valueContainerDataHolder;
    }

    public PropertyListItem(IValueContainerNonGeneric valueContainer)
    {
        _valueContainerWrapper = valueContainer.CreateWrapper();
        _dataHolder = valueContainer.get_dataEnum.GetAcceptableDataEnumValuesHolder();
    }

    public string getset_nameOfValueContainer
    {
        get => _valueContainerWrapper.getset_valueContainerName;
        set => SetProperty(_valueContainerWrapper.getset_valueContainerName, value, _valueContainerWrapper,
            (model, value) =>
            {
                model.getset_valueContainerName = value;
            });
    }

    private AcceptableDataEnumDataCollection.IAcceptableDataValuesHolder_NonGeneric _dataHolder;
    public AcceptableDataEnumDataCollection.IAcceptableDataValuesHolder_NonGeneric getset_dataHolder
    {
        get => _dataHolder;
        set
        {
            _dataHolder = value;

            OnPropertyChanged();
        }
    }

    private IValueContainerWrapperNonGeneric _valueContainerWrapper;
    public IValueContainerWrapperNonGeneric get_valueContainerWrapper
    {
        get => _valueContainerWrapper;
        internal set
        {
            _valueContainerWrapper = value;

            OnPropertyChanged();
        }
    }

    internal void MakeANewValueContainerWrapper(IValueContainerNonGeneric valueContainer)
    {
        get_valueContainerWrapper = _dataHolder.get_dataEnum.CreateValueContainerInstanceAsNonGeneric(valueContainer);
    }

    internal void MakeANewValueContainerWrapper()
    {
        IValueContainerNonGeneric valueContainer = _dataHolder.get_dataEnum.CreateValueContainerWithDefaultValue(getset_nameOfValueContainer);

        get_valueContainerWrapper = _dataHolder.get_dataEnum.CreateValueContainerInstanceAsNonGeneric(valueContainer);
    }
}

// NOTE : NEVER USE, UNDER ANY CIRCUMSTANCES, IT IS ALREADY USED IN
// ValueContainerWrapperBase2<T, TValueContainerGenericType>, SO DO NOT USE FURTHER
public interface IValueContainerWrapperNonGeneric
{
    public string getset_valueContainerName { get; internal set; }
    public AcceptableDataEnumDataCollection.IAcceptableDataValuesHolder_NonGeneric get_valueContainerDataHolder { get; }
    IValueContainerNonGeneric get_valueContainerNonGeneric { get; }
}

// NOTE: All derived getset_types of this one should be in this file for simplicty

// NOTE: All derived getset_types of this one (directly or indirectly) should implement a paramaterless constructor
// (This is fully enforced by the AcceptableEnumDataHolder logic, which requires (through generic paremeter 'new()' constraint)
// that all instantiated getset_types with it to have one) - Better than a reflaction solution with untrackable dependancies from the code
public abstract class ValueContainerWrapperBase<TValueInUI, TValueInData> : ObservableObject, IValueContainerWrapperNonGeneric
    where TValueInData : notnull
{
    protected ValueContainerWrapperBase()
    {
        get_valueContainer = new(
            string.Empty,
            AcceptableDataEnumDataCollection.GetDataHolderFromValueContainerWrapperType(GetType()).get_dataEnum);

#pragma warning disable S1699
        // reason - calling an overridable method is desirable and isolated enough to be impossible for weird behaviour to occur

        _objectToProvideValue = ConvertFromDataToUIData(get_valueContainer.getset_value);

#pragma warning restore S1699

    }

    private TValueInUI _objectToProvideValue;

    public TValueInUI getset_objectToProvideValue
    {
        get => _objectToProvideValue;
        set
        {
            bool isNewValueValid = false;

            if (!Equals(_objectToProvideValue, value))
            {
                isNewValueValid = ValidateValueInUIAsANewValue(value);
            }

            if (isNewValueValid)
            {
                _objectToProvideValue = value;
                get_valueContainer.getset_value = ConvertFromUIDataToData(_objectToProvideValue);

                OnPropertyChanged();
            }
        }
    }

    public ValueContainer<TValueInData> get_valueContainer { get; private set; }

    public IValueContainerNonGeneric get_valueContainerNonGeneric => get_valueContainer;

    public string getset_valueContainerName
    {
        get => get_valueContainer.getset_name;
        set
        {
            SetProperty(
                get_valueContainer.getset_name, value,
                get_valueContainer, Set_getset_valueContainerName);
        }
    }

    private static void Set_getset_valueContainerName(ValueContainer<TValueInData> model, string value)
    {
        model.getset_name = value;
    }

    public AcceptableDataEnumDataCollection.IAcceptableDataValuesHolder_NonGeneric get_valueContainerDataHolder =>
        get_valueContainer.get_dataEnum.GetAcceptableDataEnumValuesHolder();

    internal void SetValueContainer(ValueContainer<TValueInData> valueContainer)
    {
        get_valueContainer = valueContainer;

        _objectToProvideValue = ConvertFromDataToUIData(get_valueContainer.getset_value);
    }

    protected virtual bool ValidateValueInUIAsANewValue(TValueInUI valueInUI)
    {
        return true;
    }

    protected abstract TValueInData ConvertFromUIDataToData(TValueInUI objectToProvideValue);
    protected abstract TValueInUI ConvertFromDataToUIData(TValueInData valueInData);
}

public class StringValueContainerWrapper : ValueContainerWrapperBase<string, string>
{
    protected override string ConvertFromUIDataToData(string objectToProvideValue)
    {
        return objectToProvideValue;
    }

    protected override string ConvertFromDataToUIData(string valueInData)
    {
        return valueInData;
    }
}

public class WholeNumberValueContainerWrapper : ValueContainerWrapperBase<long?, long>
{
    private const long _defaultValue = 0;

    protected override long ConvertFromUIDataToData(long? objectToProvideValue)
    {
        return objectToProvideValue ?? _defaultValue;
    }

    protected override long? ConvertFromDataToUIData(long valueInData)
    {
        return valueInData;
    }
}

public class NotWholeNumberValueContainerWrapper : ValueContainerWrapperBase<decimal?, decimal>
{
    private const decimal _defaultValue = 0M;

    protected override decimal ConvertFromUIDataToData(decimal? objectToProvideValue)
    {
        return objectToProvideValue ?? _defaultValue;
    }

    protected override decimal? ConvertFromDataToUIData(decimal valueInData)
    {
        return valueInData;
    }
}

public class CurrencyValueContainerWrapper : NotWholeNumberValueContainerWrapper
{
    private static CultureCurrencyInfo GetValueHolder()
    {
        CultureCurrencyInfo currencyValueContainer = AcceptableTypesFormatConstants.get_currencyData.FirstOrDefault(
            (valueHolder) => (valueHolder.get_CurrencySymbol == CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol),
            AcceptableTypesFormatConstants.get_currencyData[0]);

        return currencyValueContainer;
    }

    public static IReadOnlyList<CultureCurrencyInfo> get_cultureCurrencySymbolAndNamesData => AcceptableTypesFormatConstants.get_currencyData;

    private CultureCurrencyInfo _valueHolder = GetValueHolder();

    public CultureCurrencyInfo getset_valueHolder
    {
        get => _valueHolder;
        set
        {
            _valueHolder = value;
            OnPropertyChanged();
        }
    }
}

public class CheckmarkValueContainerWrapper : ValueContainerWrapperBase<bool, bool>
{
    protected override bool ConvertFromUIDataToData(bool objectToProvideValue)
    {
        return objectToProvideValue;
    }

    protected override bool ConvertFromDataToUIData(bool valueInData)
    {
        return valueInData;
    }
}

public class DateValueContainerWrapper : ValueContainerWrapperBase<DateTime, DateOnly>
{
    protected override DateOnly ConvertFromUIDataToData(DateTime objectToProvideValue)
    {
        return DateOnly.FromDateTime(objectToProvideValue);
    }

    protected override DateTime ConvertFromDataToUIData(DateOnly valueInData)
    {
        return new DateTime(valueInData.Year, valueInData.Month, valueInData.Day);
    }
}

public class TimeValueContainerWrapper : ValueContainerWrapperBase<DateTime, TimeOnly>
{
    protected override TimeOnly ConvertFromUIDataToData(DateTime objectToProvideValue)
    {
        return TimeOnly.FromDateTime(objectToProvideValue);
    }

    protected override DateTime ConvertFromDataToUIData(TimeOnly valueInData)
    {
        return new DateTime(1, 1, 1, valueInData.Hour, valueInData.Minute, valueInData.Second, valueInData.Millisecond);
    }
}

public class DateAndTimeValueContainerWrapper : ValueContainerWrapperBase<DateTime, DateTime>
{
    protected override DateTime ConvertFromUIDataToData(DateTime objectToProvideValue)
    {
        return objectToProvideValue;
    }

    protected override DateTime ConvertFromDataToUIData(DateTime valueInData)
    {
        return valueInData;
    }
}