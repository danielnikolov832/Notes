using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomMediatorForTalksBetweenVMsExp;

namespace WPFUI.Mediators;

internal static class StaticMediators
{
    public static Mediator get_currentMediatorForVMs { get; } = new();
    public static Publisher get_currentPublisherForVMs { get; } = new();
}
