using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPFUI.VVM.TypeEditorView;

// NOTE : Types are not working for now because the data is being sent back to the Business logic and updated there
// but its not being updated in the view
public class TypeEditorViewModel : ObservableObject
{
    public TypeEditorViewModel()
    {
        IReadOnlyList<TypeCategory> typeCategories = NoteTypeDataHolder.get_instance.GetRootCategoriesList();

        IEnumerable<TypeCategoryWrapper> categoryWrappers =
            from TypeCategory category in typeCategories
            select new TypeCategoryWrapper(category);

        IReadOnlyList<NoteType> rootTypes = NoteTypeDataHolder.get_instance.GetRootTypesList();

        IEnumerable<NoteTypeWrapper> rootTypeWrappers =
            from NoteType type in rootTypes
            select new NoteTypeWrapper(type);

        get_rootTypesAndCategories = new(categoryWrappers);

        foreach (NoteTypeWrapper type in rootTypeWrappers)
        {
            get_rootTypesAndCategories.Add(type);
        }
    }

    public ObservableCollection<ITypeAndCategoryWrapper> get_rootTypesAndCategories { get; init; }

    private object? _selectedItemInCategoryTreeView;

    public object? getset_selectedItemInCategoryTreeView
    {
        get => _selectedItemInCategoryTreeView;
        set
        {
            _selectedItemInCategoryTreeView = value;

            OnPropertyChanged();
        }
    }
}