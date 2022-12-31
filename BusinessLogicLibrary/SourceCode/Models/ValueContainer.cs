using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;
using BusinessLogicLibrary;

namespace BusinessLogicLibrary.Models;

public interface IValueContainerNonGeneric
{
    AcceptableDataEnum get_dataEnum { get; }
    string getset_name { get; set; }
}

public sealed class ValueContainer<T> : IValueContainerNonGeneric
    where T : notnull
{
    public string getset_name { get; set; }

    public AcceptableDataEnum get_dataEnum { get; init; }

    public T getset_value { get; set; }

    public ValueContainer(string name, T value, AcceptableDataEnum dataEnum)
    {
        getset_name = name;
        getset_value = value;

        get_dataEnum = dataEnum;
    }

    public ValueContainer(string name, AcceptableDataEnum dataEnum)
    {
        getset_name = name;
        getset_value = (T)dataEnum.GetDefaultValue();

        get_dataEnum = dataEnum;
    }
}