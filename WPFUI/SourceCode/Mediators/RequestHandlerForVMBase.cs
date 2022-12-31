using CommunityToolkit.Mvvm.ComponentModel;
using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.Mediators;
internal abstract class RequestHandlerForVMBase<TRequest, TOutput, TViewModel> : RequestHandlerBase<TRequest, TOutput>
    where TRequest : IRequest<TOutput>
    where TViewModel : ObservableObject
{

    private readonly WeakReference<TViewModel> _viewModelReference;
    protected TViewModel? get_viewModelIfExists => GetViewModel();

    private TViewModel? GetViewModel()
    {
        _viewModelReference.TryGetTarget(out TViewModel? output);

        return output;
    }

    protected bool get_viewModelExists => (get_viewModelIfExists == null);

    protected RequestHandlerForVMBase(TViewModel viewModel, Mediator publisher)
        : base(publisher)
    {
        _viewModelReference = new WeakReference<TViewModel>(viewModel);
    }
}

internal abstract class RequestHandlerAsyncForVMBase<TRequest, TOutput, TViewModel> : RequestHandlerAsyncBase<TRequest, TOutput>
    where TRequest : IRequestAsync<TOutput>
    where TViewModel : ObservableObject
{

    private readonly WeakReference<TViewModel> _viewModelReference;
    protected TViewModel? get_viewModelIfExists => GetViewModel();

    private TViewModel? GetViewModel()
    {
        _viewModelReference.TryGetTarget(out TViewModel? output);

        return output;
    }

    protected bool get_viewModelExists => (get_viewModelIfExists == null);

    protected RequestHandlerAsyncForVMBase(TViewModel viewModel, Mediator publisher)
        : base(publisher)
    {
        _viewModelReference = new WeakReference<TViewModel>(viewModel);
    }
}