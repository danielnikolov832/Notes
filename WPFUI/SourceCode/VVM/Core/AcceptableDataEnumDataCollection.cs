using System;
using System.Collections.Generic;

using BusinessLogicLibrary;
using BusinessLogicLibrary.Models;
using WPFUI.VVM.PropertyListView;

namespace WPFUI.VVM.Core
{
    // Here is where the data for the new element getset_types, supposed to be made by the modification of the BusinessLogic.AcceptableDataEnumVariable is declared and connected to the ViewModel logic
    public static class AcceptableDataEnumDataCollection
    {
#pragma warning disable S3963
        // reason : this constructor is used for validation of whether all listed AcceptableDataEnum Options are implemented
        static AcceptableDataEnumDataCollection()
        {
            Type typeOfAcceptableDataEnum = typeof(AcceptableDataEnum);

            foreach (AcceptableDataEnum value in (AcceptableDataEnum[])typeOfAcceptableDataEnum.GetEnumValues())
            {
                if (_acceptableDataEnumAndAcceptableDataValuesHolderPairs.ContainsKey(value) == false)
                {
                    throw new NotImplementedException($"Not all values of the enum 'AcceptableDataEnum' have been implemented, this one hasn't been : AcceptableDataEnum.{typeOfAcceptableDataEnum.GetEnumName(value)}");
                }
            }
        }

#pragma warning restore S3963

        private interface IAcceptableDataValuesHolderFull : IAcceptableDataValuesHolder_NonGeneric
        {
            IValueContainerWrapperNonGeneric CreateValueContainerInstanceAsNonGenericInternal(string nameOfValueContainer = "");
            IValueContainerWrapperNonGeneric CreateValueContainerInstanceAsNonGenericInternal(IValueContainerNonGeneric valueContainerAsNonGeneric);
        }

        public interface IAcceptableDataValuesHolder_NonGeneric
        {
            AcceptableDataEnum get_dataEnum { get; }
            string get_stringRepresentation { get; }
            Type get_valueContainerWrapperType { get; }
            Type get_valueInDataType { get; }
            Type get_valueInUIType { get; }
        }

        private sealed class AcceptableDataValuesHolder<TValueInUI, TValueInData, TValueContainerWrapper> : IAcceptableDataValuesHolderFull
            where TValueInData : notnull
            where TValueContainerWrapper : ValueContainerWrapperBase<TValueInUI, TValueInData>, new()
        {
            public string get_stringRepresentation { get; }

            public AcceptableDataEnum get_dataEnum { get; }

            public Type get_valueInUIType { get; }

            public Type get_valueInDataType { get; }

            public Type get_valueContainerWrapperType { get; }

            //NOTE : Do not use outside of this class for any reason
            internal AcceptableDataValuesHolder(AcceptableDataEnum dataEnum, string stringRepresentation)
            {
                get_dataEnum = dataEnum;

                get_valueInUIType = typeof(TValueInUI);
                get_valueInDataType = typeof(TValueInData);

                get_valueContainerWrapperType = typeof(TValueContainerWrapper);

                get_stringRepresentation = stringRepresentation;
            }

            public static TValueContainerWrapper CreateValueContainerInstance(string nameOfValueContainer = "")
            {
                return new TValueContainerWrapper()
                {
                    getset_valueContainerName = nameOfValueContainer
                };
            }

            public static TValueContainerWrapper CreateValueContainerInstance(IValueContainerNonGeneric valueContainerAsNonGeneric)
            {
                if (valueContainerAsNonGeneric is ValueContainer<TValueInData> valueContainer)
                {
                    TValueContainerWrapper valueContainerWrapper = new();

                    valueContainerWrapper.SetValueContainer(valueContainer);

                    return valueContainerWrapper;
                }

                throw new ArgumentException("Invalid argument 'valueContainerAsNonGeneric' is not if type ValueContainer<TValueInData> for the current instance", nameof(valueContainerAsNonGeneric));
            }

            public IValueContainerWrapperNonGeneric CreateValueContainerInstanceAsNonGenericInternal(string nameOfValueContainer = "") => AcceptableDataValuesHolder<TValueInUI, TValueInData, TValueContainerWrapper>.CreateValueContainerInstance(nameOfValueContainer);
            public IValueContainerWrapperNonGeneric CreateValueContainerInstanceAsNonGenericInternal(IValueContainerNonGeneric valueContainerAsNonGeneric) => AcceptableDataValuesHolder<TValueInUI, TValueInData, TValueContainerWrapper>.CreateValueContainerInstance(valueContainerAsNonGeneric);
        }

        private static readonly IReadOnlyDictionary<AcceptableDataEnum, IAcceptableDataValuesHolderFull> _acceptableDataEnumAndAcceptableDataValuesHolderPairs =
        new Dictionary<AcceptableDataEnum, IAcceptableDataValuesHolderFull>()
        {
            {AcceptableDataEnum.Text, new AcceptableDataValuesHolder<string, string, StringValueContainerWrapper>(
                AcceptableDataEnum.Text,
                "Text")},

            {AcceptableDataEnum.Integer, new AcceptableDataValuesHolder<long?, long, WholeNumberValueContainerWrapper>(
                AcceptableDataEnum.Integer,
                "Whole Number")},

            {AcceptableDataEnum.Decimal, new AcceptableDataValuesHolder<decimal?, decimal, NotWholeNumberValueContainerWrapper>(
                AcceptableDataEnum.Decimal,
                "Decimal Number")},

            {AcceptableDataEnum.Currency, new AcceptableDataValuesHolder<decimal?, decimal, CurrencyValueContainerWrapper>(
                AcceptableDataEnum.Currency,
                "Currency")},

            {AcceptableDataEnum.Checkmark, new AcceptableDataValuesHolder<bool, bool, CheckmarkValueContainerWrapper>(
                AcceptableDataEnum.Checkmark,
                "Checkmark")},

            {AcceptableDataEnum.Date, new AcceptableDataValuesHolder<DateTime, DateOnly, DateValueContainerWrapper>(
                AcceptableDataEnum.Date,
                "Date")},

            {AcceptableDataEnum.Time, new AcceptableDataValuesHolder<DateTime, TimeOnly, TimeValueContainerWrapper>(
                AcceptableDataEnum.Time,
                "Time of day")},

            {AcceptableDataEnum.DateAndTime, new AcceptableDataValuesHolder<DateTime, DateTime, DateAndTimeValueContainerWrapper>(
                AcceptableDataEnum.DateAndTime,
                "Date & Time")}
        };

        internal static IAcceptableDataValuesHolder_NonGeneric GetAcceptableDataEnumValuesHolder(this AcceptableDataEnum acceptableDataEnum)
        {
            return _acceptableDataEnumAndAcceptableDataValuesHolderPairs[acceptableDataEnum];
        }

        internal static IEnumerable<IAcceptableDataValuesHolder_NonGeneric> GetAllAcceptableDataEnumValuesHolders()
        {
            return _acceptableDataEnumAndAcceptableDataValuesHolderPairs.Values;
        }

        internal static IAcceptableDataValuesHolder_NonGeneric GetDataHolderFromValueContainerWrapperType(Type typeOfValueContainerWrapper)
        {
            foreach (KeyValuePair<AcceptableDataEnum, IAcceptableDataValuesHolderFull> kvp in
            _acceptableDataEnumAndAcceptableDataValuesHolderPairs)
            {
                IAcceptableDataValuesHolder_NonGeneric value = kvp.Value;

                if (value.get_valueContainerWrapperType == typeOfValueContainerWrapper)
                {
                    return value;
                }
            }

            throw new NotSupportedException("UnSupported TValueContainerWrapper Type");
        }

        internal static IValueContainerWrapperNonGeneric CreateValueContainerInstanceAsNonGeneric(this AcceptableDataEnum acceptableDataEnum, string nameOfValueContainer = "")
        {
            return _acceptableDataEnumAndAcceptableDataValuesHolderPairs[acceptableDataEnum].CreateValueContainerInstanceAsNonGenericInternal(nameOfValueContainer);
        }

        internal static IValueContainerWrapperNonGeneric CreateValueContainerInstanceAsNonGeneric(this AcceptableDataEnum acceptableDataEnum, IValueContainerNonGeneric valueContainer)
        {
            return _acceptableDataEnumAndAcceptableDataValuesHolderPairs[acceptableDataEnum].CreateValueContainerInstanceAsNonGenericInternal(valueContainer);
        }

        internal static IValueContainerWrapperNonGeneric CreateWrapper(this IValueContainerNonGeneric valueContainer)
        {
            return _acceptableDataEnumAndAcceptableDataValuesHolderPairs[valueContainer.get_dataEnum].CreateValueContainerInstanceAsNonGenericInternal(valueContainer);
        }
    }
}
