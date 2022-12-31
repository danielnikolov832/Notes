using System.Collections.Generic;
using BusinessLogicLibrary.Models;
using System.Collections.ObjectModel;
using BindingIdeasLib;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WPFUI.VVM.Core;

public interface INoteWrapperBase
{
    public Note getset_note { get; set; }
    string getset_nameWrapperProperty { get; set; }
    IReadOnlyList<INoteWrapperBase> get_childrenAsInterfaces { get; }
    ObservableCollection<IValueContainerNonGeneric> get_properties { get; }
}

public abstract class NoteWrapperBase<TNoteWrapper> : ObservableObject, INoteWrapperBase
    where TNoteWrapper : NoteWrapperBase<TNoteWrapper>
{
    private protected NoteWrapperBase(Note note)
    {
        _note = note;

        get_children = new(
            observableCollectionToBindTo : note.get_children,
            conversionFunc : CreateChildrenFromNote,
            backwardsConversionFunc : noteWrapper => noteWrapper.getset_note,
            (TNoteWrapper wrapper, Note newNote) =>
            {
                wrapper.getset_note = newNote;

                return wrapper;
            });
    }

    private Note _note;
    public Note getset_note { get => _note; set => SetNote(value); }

    private void SetNote(Note newNote)
    {
        get_children.getset_observableCollectionToBindTo = newNote.get_children;

        _note = newNote;
    }

    public string getset_nameWrapperProperty
    {
        get => _note.getset_name;
        set
        {
            _note.getset_name = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollectionBoundToAnotherObservableCollectionInTwoWayMode<Note, TNoteWrapper> get_children { get; }

    public IReadOnlyList<INoteWrapperBase> get_childrenAsInterfaces => get_children;

    public ObservableCollection<IValueContainerNonGeneric> get_properties => _note.get_properties;

    protected abstract TNoteWrapper CreateChildrenFromNote(Note note);
}
