using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogicLibrary.Models;
using CustomMediatorForTalksBetweenVMsExp;
using WPFUI.VVM.Core;
using WPFUI.VVM.MainWindow;

namespace WPFUI.Mediators.Requests;

internal record class GetSelectedNoteRequest : IRequest<INoteWrapperBase?>;