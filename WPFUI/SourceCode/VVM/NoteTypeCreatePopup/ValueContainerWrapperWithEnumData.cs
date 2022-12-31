using System.Collections.Generic;
using static WPFUI.VVM.Core.AcceptableDataEnumDataCollection;
using BusinessLogicLibrary.Models;
using BusinessLogicLibrary;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFUI.VVM.NoteTypeCreatePopup;

public class ValueContainerWrapperWithEnumData : ObservableObject
{
    public ValueContainerWrapperWithEnumData(IValueContainerNonGeneric valueContainer)
        : this(valueContainer.getset_name, valueContainer.get_dataEnum)
    {
    }

    public ValueContainerWrapperWithEnumData(string name, AcceptableDataEnum dataEnum)
    {
        getset_nameOfValueContainer = name;

        getset_enumData = dataEnum.GetAcceptableDataEnumValuesHolder();
    }

    public string getset_nameOfValueContainer { get; set; }

    public IAcceptableDataValuesHolder_NonGeneric getset_enumData { get; set; }

    private bool _isSelected = true;

    public bool getset_isSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;

            OnPropertyChanged();
        }
    }

    public static IEnumerable<IAcceptableDataValuesHolder_NonGeneric> get_itemsSourceForTypeComboBox { get; }
        = GetAllAcceptableDataEnumValuesHolders();
}
