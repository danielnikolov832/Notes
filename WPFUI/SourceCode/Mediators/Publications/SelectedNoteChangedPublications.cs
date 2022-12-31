using BusinessLogicLibrary.Models;
using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUI.VVM.NoteTreeView;

namespace WPFUI.Mediators.Publications;
internal record class SelectedNoteChangedPublication(NoteWrapperWithSelectionData? get_parameterObject) : IPublication<NoteWrapperWithSelectionData?>;
internal record class SelectedNoteDataChangeRequestedPublication(Note get_parameterObject) : IPublication<Note>;