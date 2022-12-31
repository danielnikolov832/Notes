using CommunityToolkit.Mvvm.ComponentModel;
using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFUI.Mediators;
internal abstract class SubscriberForVMBase<TPublication, TViewModel> : SubscriberBase<TPublication>
    where TPublication : IPublication
    where TViewModel : ObservableObject
{
    private readonly WeakReference<TViewModel> _viewModelReference;

    protected TViewModel? get_viewModelIfExists => GetViewModel();

    private TViewModel? GetViewModel()
    {
        _viewModelReference.TryGetTarget(out TViewModel? output);

        return output;
    }

    protected bool get_viewModelExists => (get_viewModelIfExists != null);

    protected SubscriberForVMBase(TViewModel viewModel, Publisher publisher)
        : base(publisher)
    {
        _viewModelReference = new WeakReference<TViewModel>(viewModel);
    }
}