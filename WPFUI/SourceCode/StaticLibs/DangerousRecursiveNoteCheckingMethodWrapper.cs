using System;
using System.Collections.Generic;
using WPFUI.VVM.Core;
using WPFUI.VVM.MainWindow;
using WPFUI.VVM.NoteTreeView;
using static WPFUI.VVM.NoteTypeCreatePopup.NoteTypeCreatePopupViewModel;

namespace WPFUI.StaticLibs;

// A static class to wrap the logic of the method "RecursiveSetAnythingOnChildrenOfElementBase" to prevent unsafe calls to it
internal static class DangerousRecursiveNoteCheckingMethodWrapper
{
    public static void AddOrRemoveFromCollectionBasedOnBoolValue<TNoteWrapperBase> (TNoteWrapperBase noteRepresentationWithAnyNotesWithSelectionData,
        bool value, ICollection<TNoteWrapperBase> collection)
        where TNoteWrapperBase : INoteWrapperBase
    {
        if (value)
        {
            collection.Add(noteRepresentationWithAnyNotesWithSelectionData);
        }
        else
        {
            collection.Remove(noteRepresentationWithAnyNotesWithSelectionData);
        }
    }

    public static void RecursiveSetAnythingOnChildrenOfElementBase<TNoteWrapperBase> (
        TNoteWrapperBase noteRepresentationWithAnyNotesWithSelectionData,
        Action<TNoteWrapperBase>? actionToRecursivelyPerformOnObjects = null)
            where TNoteWrapperBase : NoteWrapperBase<TNoteWrapperBase>
    {
        actionToRecursivelyPerformOnObjects?.Invoke(noteRepresentationWithAnyNotesWithSelectionData);

        foreach (TNoteWrapperBase child in noteRepresentationWithAnyNotesWithSelectionData.get_children)
        {
            RecursiveSetAnythingOnChildrenOfElementBase(child, actionToRecursivelyPerformOnObjects);
        }
    }
}
