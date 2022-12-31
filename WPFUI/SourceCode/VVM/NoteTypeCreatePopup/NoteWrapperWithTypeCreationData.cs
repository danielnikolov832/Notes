using System.Windows.Input;
using System.Collections.ObjectModel;
using BusinessLogicLibrary.Models;
using WPFUI.StaticLibs;
using WPFUI.VVM.Core;
using CommunityToolkit.Mvvm.Input;

namespace WPFUI.VVM.NoteTypeCreatePopup;

public class NoteWrapperWithTypeCreationData : NoteWrapperBase<NoteWrapperWithTypeCreationData>
{
    public NoteWrapperWithTypeCreationData(Note note, NoteWrapperWithTypeCreationData? parentWrapper = null)
        : base(note)
    {
        foreach (IValueContainerNonGeneric valueContainer in get_properties)
        {
            get_unboundPropertiesForTypeCreation.Add(new ValueContainerWrapperWithEnumData(valueContainer));
        }

        get_parentWrapper = parentWrapper;

        get_unboundChildrenForTypeCreation = new(get_children);
    }

    private bool _isSelectedAsPartOfType;

    public bool get_isSelectedAsPartOfType
    {
        get => _isSelectedAsPartOfType;
        internal set
        {
            _isSelectedAsPartOfType = value;

            OnPropertyChanged();
        }
    }

    public NoteWrapperWithTypeCreationData? get_parentWrapper { get; init; }

    public ObservableCollection<ValueContainerWrapperWithEnumData> get_unboundPropertiesForTypeCreation { get; init; } = new();

    public ObservableCollection<NoteWrapperWithTypeCreationData> get_unboundChildrenForTypeCreation { get; init; }

    protected override NoteWrapperWithTypeCreationData CreateChildrenFromNote(Note note)
    {
        return new(note, this);
    }

    #region Commands

    private RelayCommand? _recursivelySelectItemsFromNoteTreeViewCommand;
    private RelayCommand? _addChildCommand;

    public ICommand get_recursivelySelectItemsFromNoteTreeViewCommand =>
    _recursivelySelectItemsFromNoteTreeViewCommand ??= new RelayCommand(
        () =>
        {
            if (get_parentWrapper?.get_isSelectedAsPartOfType == false) return;

            get_isSelectedAsPartOfType = !get_isSelectedAsPartOfType;

            DangerousRecursiveMethodWrapper.Wrapper_RecursiveSetNoteTypeCreationSelectionOnChildrenOfElement(
                noteRepresentationWithAnyNotesWithData: this,
                valueToSetSelectionTo: get_isSelectedAsPartOfType);
        }
    );

    public ICommand get_addChildCommand =>
    _addChildCommand ??= new RelayCommand(
        () =>
        {
            NoteWrapperWithTypeCreationData newDefaultNote = new(new Note(""), this);

            get_unboundChildrenForTypeCreation.Add(newDefaultNote);
        }
    );

    #endregion Commands

    private static class DangerousRecursiveMethodWrapper
    {
        public static void Wrapper_RecursiveSetNoteTypeCreationSelectionOnChildrenOfElement(
           NoteWrapperWithTypeCreationData noteRepresentationWithAnyNotesWithData, bool valueToSetSelectionTo)
        {
            foreach (NoteWrapperWithTypeCreationData noteChild in noteRepresentationWithAnyNotesWithData.get_children)
            {
                RecursiveSetNoteeTypeCreationSelectionOnChildrenOfElement(noteChild, valueToSetSelectionTo);
            }
        }

        private static void RecursiveSetNoteeTypeCreationSelectionOnChildrenOfElement(
            NoteWrapperWithTypeCreationData noteRepresentationWithAnyNotesWithSelectionData,
            bool valueToSetSelectionTo)
        {
            DangerousRecursiveNoteCheckingMethodWrapper.RecursiveSetAnythingOnChildrenOfElementBase(
                noteRepresentationWithAnyNotesWithSelectionData,
                (NoteWrapperWithTypeCreationData note) =>
                {
                    note.get_isSelectedAsPartOfType = valueToSetSelectionTo;
                });
        }
    }
}