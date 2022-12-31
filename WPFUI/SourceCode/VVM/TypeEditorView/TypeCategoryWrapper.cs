using BusinessLogicLibrary.Models;
using BusinessLogicLibrary.Services.NoteTypeGenerationService;
using BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;

namespace WPFUI.VVM.TypeEditorView;

public interface ITypeAndCategoryWrapper
{
    bool get_isCategory { get; }
}

public class TypeCategoryWrapper : ObservableObject, ITypeAndCategoryWrapper
{
    public TypeCategoryWrapper(TypeCategory typeCategory, TypeCategoryWrapper? parentCategoryWrapper = null)
    {
        foreach (TypeCategory category in typeCategory.get_childCategories)
        {
            _typesAndCategories.Add(new TypeCategoryWrapper(category, this));
        }

        foreach (NoteType type in typeCategory.get_types)
        {
            _typesAndCategories.Add(new NoteTypeWrapper(type, this));
        }

        _typeCategory = typeCategory;
        get_parentCategoryWrapper = parentCategoryWrapper;
    }

    private readonly ObservableCollection<ITypeAndCategoryWrapper> _typesAndCategories = new();

    private readonly TypeCategory _typeCategory;

    //public INotifyCollectionChanged get_childrenAsCollectionChanged => _children;
    //public INotifyCollectionChanged get_typesAsCollectionChanged => _types;

    public INotifyCollectionChanged get_typesAndCategories => _typesAndCategories;

    public string getset_categoryNameWrapper
    {
        get => _typeCategory.getset_name;
        set => SetProperty(_typeCategory.getset_name, value, _typeCategory, (TypeCategory typeCategory, string newName) => typeCategory.getset_name = newName);
    }

    public TypeCategoryWrapper? get_parentCategoryWrapper { get; init; }

    public bool get_isCategory => true;

    internal void AddChild(string name)
    {
        (bool _, TypeCategory category) = NoteTypeDataHolder.get_instance.TryAddCategory(name, _typeCategory);

        _typesAndCategories.Add(new TypeCategoryWrapper(category, this));
    }

    internal void AddType(string name, Note note)
    {
        (bool _, NoteType type) = NoteTypeDataHolder.get_instance.TryAddNoteType(name, note, _typeCategory);

        _typesAndCategories.Add(new NoteTypeWrapper(type, this));
    }

    internal bool TryUpdateType(NoteTypeWrapper type, NoteTypeWrapper newType)
    {
        for (int i = 0; i < _typesAndCategories.Count; i++)
        {
            ITypeAndCategoryWrapper childType = _typesAndCategories[i];

            if (childType.get_isCategory || childType != type) continue;

            _typesAndCategories[i] = newType;

            return true;
        }

        return false;
    }

    internal void RemoveChild(TypeCategoryWrapper childCategory)
    {
        for (int i = 0; i < _typesAndCategories.Count; i++)
        {
            ITypeAndCategoryWrapper child = _typesAndCategories[i];

            if (!child.get_isCategory || child != childCategory) continue;

            _typesAndCategories.RemoveAt(i);

            return;
        }
    }

    internal bool TryRemoveType(NoteTypeWrapper type)
    {
        for (int i = 0; i < _typesAndCategories.Count; i++)
        {
            ITypeAndCategoryWrapper childType = _typesAndCategories[i];

            if (childType.get_isCategory || childType != type) continue;

            _typesAndCategories.RemoveAt(i);

            return true;
        }

        return false;
    }

    internal void RemoveSelf()
    {
        get_parentCategoryWrapper?.RemoveChild(this);

        NoteTypeDataHolder.get_instance.TryRemoveCategory(_typeCategory);
    }

    #region Commands

    private RelayCommand? _addTypeCommand;
    private RelayCommand? _addCategoryCommand;
    private RelayCommand? _deleteSelfCommand;

    public ICommand get_addTypeCommand =>
    _addTypeCommand ??= new(
        () =>
        {
            AddType(string.Empty, new Note(string.Empty));
        }
    );

    public ICommand get_addCategoryCommand =>
    _addCategoryCommand ??= new(
        () =>
        {
            AddChild(string.Empty);
        }
    );

    public ICommand get_deleteSelfCommand =>
   _deleteSelfCommand ??= new(
       () =>
       {
           RemoveSelf();
       }
   );

    #endregion Commands
}
public class NoteTypeWrapper : ObservableObject, ITypeAndCategoryWrapper
{
    internal NoteTypeWrapper(NoteType noteType, TypeCategoryWrapper? parentCategoryWrapper = null)
    {
        _noteType = noteType;
        get_parentCategoryWrapper = parentCategoryWrapper;
    }

    private NoteType _noteType;
    internal TypeCategoryWrapper? get_parentCategoryWrapper { get; init; }

    public string getset_typeNameWrapper
    {
        get => _noteType.getset_name;
        set => SetProperty(_noteType.getset_name, value, _noteType, (NoteType model, string name) => model.getset_name = name);
    }

    public NoteData get_typeData => _noteType.get_noteData;

    public bool get_isCategory => false;

    internal void RemoveSelf()
    {
        bool shouldTypeBeRemovedFromDB = true;

        if (get_parentCategoryWrapper != null)
        {
            shouldTypeBeRemovedFromDB = get_parentCategoryWrapper.TryRemoveType(this);
        }

        if (!shouldTypeBeRemovedFromDB) return;

        NoteTypeDataHolder.get_instance.TryRemoveNoteType(_noteType);
    }

    #region Commands

    private RelayCommand? _updateTypeCommand;
    private RelayCommand? _deleteSelfCommand;

    public ICommand get_updateTypeCommand =>
    _updateTypeCommand ??= new(
        () =>
        {
            //UpdateType();
        }
    );

    public ICommand get_deleteSelfCommand =>
   _deleteSelfCommand ??= new(
       () =>
       {
           RemoveSelf();
       }
   );
    #endregion Commands
}
