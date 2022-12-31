using BusinessLogicLibrary.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUI.Mediators;
using WPFUI.Mediators.Publications;
using WPFUI.Mediators.Requests;
using WPFUI.StaticLibs;
using WPFUI.VVM.Core;

namespace WPFUI.VVM.NoteTreeView;

public sealed class NoteTreeViewModel : ObservableObject
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    // reason : commands are instantiated at a later point
    public NoteTreeViewModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        _getSelectedNoteRequestHandler = new(this, StaticMediators.get_currentMediatorForVMs);

        _selectedNoteChangeRequestedSubscriber = new(this, StaticMediators.get_currentPublisherForVMs);
    }

    // UI must bind to this property in OneWayToSourceMode
    public ObservableCollection<NoteWrapperWithSelectionData> get_noteTreeViewItemsSource { get; init; } = new();

    private NoteWrapperWithSelectionData? _selectedNoteInNoteTreeView;
    public NoteWrapperWithSelectionData? getset_selectedNoteInNoteTreeView
    {
        get => _selectedNoteInNoteTreeView;
        set
        {
            _selectedNoteInNoteTreeView = value;

            StaticMediators.get_currentPublisherForVMs.Publish(new SelectedNoteChangedPublication(getset_selectedNoteInNoteTreeView));

            OnPropertyChanged();
        }
    }

    private readonly ObservableCollection<NoteWrapperWithSelectionData> _multiSelectedItemsInNoteTreeView = new();

    // Must only be set through the property "get_isNoteTreeViewInMultiSelectionMode"
    private bool _isNoteTreeViewInMultiSelectionMode = false;
    public bool get_isNoteTreeViewInMultiSelectionMode
    {
        get => _isNoteTreeViewInMultiSelectionMode;
        private set
        {
            _isNoteTreeViewInMultiSelectionMode = value;
            OnPropertyChanged();
        }
    }

    private static void RecursiveRemovalOfCheckedItems(IList<NoteWrapperWithSelectionData> rootNodes)
    {
        int noteCollectionCount = rootNodes.Count;

        for (int i = 0; i < noteCollectionCount; i++)
        {
            NoteWrapperWithSelectionData node = rootNodes[i];

            if (node.get_isChecked)
            {
                rootNodes.RemoveAt(i);

                i--;
                noteCollectionCount--;

                continue;
            }

            if (node.get_children.Count == 0) continue;

            RecursiveRemovalOfCheckedItems(node.get_children);
        }
    }

    private void SelectionLogicForMultiSelectionScenarios(NoteWrapperWithSelectionData? noteRepresentationWithAnyNotesWithSelectionData)
    {
        if (noteRepresentationWithAnyNotesWithSelectionData is null || noteRepresentationWithAnyNotesWithSelectionData.get_isSelectionControlledByParent) return;

        noteRepresentationWithAnyNotesWithSelectionData.get_isChecked = !noteRepresentationWithAnyNotesWithSelectionData.get_isChecked;

        DangerousRecursiveMethodWrapper.Wrapper_RecursiveSetSelectionOnChildrenOfElement(
            noteRepresentationWithAnyNotesWithSelectionData: noteRepresentationWithAnyNotesWithSelectionData,
            valueToSetSelectionTo: noteRepresentationWithAnyNotesWithSelectionData.get_isChecked,
            shouldChildrenBeControlledByParent: noteRepresentationWithAnyNotesWithSelectionData.get_isChecked,
            collection: _multiSelectedItemsInNoteTreeView);

        getset_selectedNoteInNoteTreeView = null;
    }

    private static class DangerousRecursiveMethodWrapper
    {
        public static void Wrapper_RecursiveSetSelectionOnChildrenOfElement(
            NoteWrapperWithSelectionData noteRepresentationWithAnyNotesWithSelectionData, bool valueToSetSelectionTo,
            bool shouldChildrenBeControlledByParent, ICollection<NoteWrapperWithSelectionData> collection)
        {
            DangerousRecursiveNoteCheckingMethodWrapper.AddOrRemoveFromCollectionBasedOnBoolValue(noteRepresentationWithAnyNotesWithSelectionData, valueToSetSelectionTo, collection);

            foreach (NoteWrapperWithSelectionData noteChild in noteRepresentationWithAnyNotesWithSelectionData.get_children)
            {
                RecursiveSetSelectionOnChildrenOfElement(noteChild, valueToSetSelectionTo,
                    shouldChildrenBeControlledByParent, collection);
            }
        }

        private static void RecursiveSetSelectionOnChildrenOfElement(
            NoteWrapperWithSelectionData noteWrapper,
            bool valueToSetSelectionTo,
            bool shouldChildrenBeControlledByParent,
            ICollection<NoteWrapperWithSelectionData> collection)
        {
            DangerousRecursiveNoteCheckingMethodWrapper.RecursiveSetAnythingOnChildrenOfElementBase(noteWrapper,
                (NoteWrapperWithSelectionData note) =>
                {
                    note.get_isChecked = valueToSetSelectionTo;
                    note.get_isSelectionControlledByParent = shouldChildrenBeControlledByParent;

                    DangerousRecursiveNoteCheckingMethodWrapper.AddOrRemoveFromCollectionBasedOnBoolValue(noteWrapper, valueToSetSelectionTo, collection);
                });
        }
    }

    #region Commands

    private RelayCommand _addItemToTreeViewCommand;
    private RelayCommand _deleteItemsFromTreeViewCommand;
    private RelayCommand _switchSelectionModeCommand;
    private RelayCommand _noteTreeView_SelectionChanged_MultiSelectionLogic;

    public ICommand get_addItemToTreeViewCommand =>
    _addItemToTreeViewCommand ??= new RelayCommand(
        () =>
        {
            Note noteToAdd = new(Random.Shared.Next().ToString());

            if (_selectedNoteInNoteTreeView is not null)
            {
                _selectedNoteInNoteTreeView.get_children.Add(new(noteToAdd));

                return;
            }

            get_noteTreeViewItemsSource.Add(new(noteToAdd));
        },

        () => !get_isNoteTreeViewInMultiSelectionMode
    );

    public ICommand get_deleteItemsFromTreeViewCommand =>
    _deleteItemsFromTreeViewCommand ??= new RelayCommand(
        () =>
        {
            RecursiveRemovalOfCheckedItems(get_noteTreeViewItemsSource);
        },

        () => get_isNoteTreeViewInMultiSelectionMode
    );

    public ICommand get_switchSelectionModeCommand =>
    _switchSelectionModeCommand ??= new RelayCommand(
        () =>
        {
            if (get_isNoteTreeViewInMultiSelectionMode)
            {
                foreach (NoteWrapperWithSelectionData noteWrapper in _multiSelectedItemsInNoteTreeView)
                {
                    noteWrapper.get_isChecked = false;
                    noteWrapper.get_isSelectionControlledByParent = false;
                }
            }

            _multiSelectedItemsInNoteTreeView.Clear();

            get_isNoteTreeViewInMultiSelectionMode = !get_isNoteTreeViewInMultiSelectionMode;
        }
    );

    public ICommand get_noteTreeView_SelectionChanged_MultiSelectionLogic =>
    _noteTreeView_SelectionChanged_MultiSelectionLogic ??= new RelayCommand(
        () =>
        {
            SelectionLogicForMultiSelectionScenarios(_selectedNoteInNoteTreeView);
        },

        () => _isNoteTreeViewInMultiSelectionMode
    );

    #endregion Commands

    #region Mediator Setup

#pragma warning disable IDE0052 // Remove unread private members
    private readonly GetSelectedNoteRequestHandler _getSelectedNoteRequestHandler;
#pragma warning restore IDE0052 // Remove unread private members

    private sealed class GetSelectedNoteRequestHandler : RequestHandlerForVMBase<GetSelectedNoteRequest, INoteWrapperBase?, NoteTreeViewModel>
    {
        public GetSelectedNoteRequestHandler(NoteTreeViewModel vm, Mediator mediator)
            : base(vm, mediator)
        {
        }

        public override INoteWrapperBase? Handle(GetSelectedNoteRequest request, CancellationToken cancellationToken)
        {
            return get_viewModelIfExists?.getset_selectedNoteInNoteTreeView;
        }
    }

    #endregion Mediator Setup

    #region Publisher setup

#pragma warning disable IDE0052 // Remove unread private members
    private readonly SelectedNoteChangeRequestedSubscriber _selectedNoteChangeRequestedSubscriber;
#pragma warning restore IDE0052 // Remove unread private members

    private sealed class SelectedNoteChangeRequestedSubscriber : SubscriberForVMBase<SelectedNoteDataChangeRequestedPublication, NoteTreeViewModel>
    {
        public SelectedNoteChangeRequestedSubscriber(NoteTreeViewModel viewModel, Publisher publisher) : base(viewModel, publisher)
        {
        }

        public override Task Handle(SelectedNoteDataChangeRequestedPublication request, CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested) return Task.FromCanceled(cancellationToken);

            if (!get_viewModelExists) return Task.CompletedTask;

            if (get_viewModelIfExists!.getset_selectedNoteInNoteTreeView is null) return Task.CompletedTask;

            get_viewModelIfExists!.getset_selectedNoteInNoteTreeView.getset_note = request.get_parameterObject;

            return Task.CompletedTask;
        }
    }

    #endregion Publisher setup
}