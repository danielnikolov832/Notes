using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Windows.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUI.Mediators;
using static WPFUI.Mediators.Publications.SettingsVMPublications;

namespace WPFUI.VVM.MainWindow;

public class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        //TEMP_StressTestNoteTreeView(20, 170);

        _primaryColorChangedSubscriber = new(this, StaticMediators.get_currentPublisherForVMs);
        _backgroundColorChangedSubscriber = new(this, StaticMediators.get_currentPublisherForVMs);
    }

    //private void TEMP_StressTestNoteTreeView (int rootNotes, int recursiveChildCount)
    //{
    //    for (int i = 0; i < rootNotes; i++)
    //    {
    //        NoteRepresentationWithAnyElementsWithSelectionData parent = new()
    //        {
    //            getset_name = TEMP_GetDebugTextFromIndexes(i, 0)
    //        };

    //        get_noteTreeViewItemsSource.Add(parent);

    //        for (int j = 1; j < recursiveChildCount; j++)
    //        {
    //            NoteRepresentationWithAnyElementsWithSelectionData current = new()
    //            {
    //                getset_name = TEMP_GetDebugTextFromIndexes(i, j)
    //            };

    //            parent.get_children.Add(current);

    //            parent = current;
    //        }
    //    }

    //    static string TEMP_GetDebugTextFromIndexes (int i, int j)
    //    {
    //        return $"{i}.{j}";
    //    }
    //}

    private DifferentWindowsEnum _previousWindowEnum = DifferentWindowsEnum.MainWindow;

    private DifferentWindowsEnum _differentWindowsEnum;

    public DifferentWindowsEnum getset_differentWindowsEnum
    {
        get => _differentWindowsEnum;
        private set
        {
            if (value == _differentWindowsEnum) return;

            _differentWindowsEnum = value;

            OnPropertyChanged();
        }
    }

    private Color _primaryColor;

    public Color getset_primaryColor
    {
        get => _primaryColor;
        private set
        {
            _primaryColor = value;

            OnPropertyChanged();
        }
    }

    private Color _backgroundColor;

    public Color getset_backgroundColor
    {
        get => _backgroundColor;
        private set
        {
            _backgroundColor = value;

            OnPropertyChanged();
        }
    }

    #region Commands

    private RelayCommand? _changeToSettingsAndBackCommand;
    private RelayCommand? _changeToTypeEditorAndBackCommand;

    public ICommand get_changeToSettingsAndBackCommand =>
    _changeToSettingsAndBackCommand ??= new RelayCommand(
        () =>
        {
            if (getset_differentWindowsEnum == DifferentWindowsEnum.Settings)
            {
                getset_differentWindowsEnum = _previousWindowEnum;

                return;
            }

            _previousWindowEnum = getset_differentWindowsEnum;

            getset_differentWindowsEnum = DifferentWindowsEnum.Settings;
        }
    );

    public ICommand get_changeToTypeEditorAndBackCommand =>
    _changeToTypeEditorAndBackCommand ??= new RelayCommand(
        () =>
        {
            if (getset_differentWindowsEnum == DifferentWindowsEnum.TypeEditor)
            {
                getset_differentWindowsEnum = _previousWindowEnum;

                return;
            }

            _previousWindowEnum = getset_differentWindowsEnum;

            getset_differentWindowsEnum = DifferentWindowsEnum.TypeEditor;
        }
    );

    #endregion Commands

    #region Publisher setup

#pragma warning disable IDE0052 // Remove unread private members

    private readonly PrimaryColorChangedSubscriber _primaryColorChangedSubscriber;
    private readonly BackgroundColorChangedSubscriber _backgroundColorChangedSubscriber;

#pragma warning restore IDE0052 // Remove unread private members

    private sealed class PrimaryColorChangedSubscriber : SubscriberForVMBase<PrimaryColorChangedPublication, MainWindowViewModel>
    {
        public PrimaryColorChangedSubscriber(MainWindowViewModel viewModel, Publisher publisher) : base(viewModel, publisher)
        {
        }

        public override Task Handle(PrimaryColorChangedPublication request, CancellationToken cancellationToken)
        {
            if (!get_viewModelExists) return Task.CompletedTask;

            get_viewModelIfExists!.getset_primaryColor = request.get_parameterObject;

            return Task.CompletedTask;
        }
    }

    private sealed class BackgroundColorChangedSubscriber : SubscriberForVMBase<BackgroundColorChangedPublication, MainWindowViewModel>
    {
        public BackgroundColorChangedSubscriber(MainWindowViewModel viewModel, Publisher publisher) : base(viewModel, publisher)
        {
        }

        public override Task Handle(BackgroundColorChangedPublication request, CancellationToken cancellationToken)
        {
            if (!get_viewModelExists) return Task.CompletedTask;

            get_viewModelIfExists!.getset_backgroundColor = request.get_parameterObject;

            return Task.CompletedTask;
        }
    }

    #endregion Publisher setup
}