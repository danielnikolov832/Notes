using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace WPFUI.Mediators.Publications;
internal class SettingsVMPublications
{
    internal record class PrimaryColorChangedPublication(Color get_parameterObject) : IPublication<Color>;
    internal record class BackgroundColorChangedPublication(Color get_parameterObject) : IPublication<Color>;
}
