using BusinessLogicLibrary.Models;
using WPFUI.VVM.Core;
using WPFUI.VVM.MainWindow;

namespace WPFUI.VVM.NoteTreeView;

public class NoteWrapperWithSelectionData : NoteWrapperBase<NoteWrapperWithSelectionData>
{
    internal NoteWrapperWithSelectionData(Note note)
        : base(note)
    {
    }

    private bool _isChecked;

    public bool get_isChecked
    {
        get => _isChecked;
        set
        {
            _isChecked = value;
            OnPropertyChanged();
        }
    }

    private bool _isSelectionControlledByParent = false;

    public bool get_isSelectionControlledByParent
    {
        get => _isSelectionControlledByParent;
        internal set
        {
            _isSelectionControlledByParent = value;
            OnPropertyChanged();
        }
    }

    protected override NoteWrapperWithSelectionData CreateChildrenFromNote(Note note)
    {
        return new(note);
    }
}