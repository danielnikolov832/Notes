using BusinessLogicLibrary.Models;
using CustomMediatorForTalksBetweenVMsExp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFUI.SourceCode.Mediators.Publications;
internal static class PropertyCollectionChangedPublications
{
    internal record class ItemInsertedInPropertyCollectionPublication(PropertyInCollectonInsertArgs get_parameterObject) : IPublication<PropertyInCollectonInsertArgs>;
    internal record class PropertyInCollectonInsertArgs(IValueContainerNonGeneric get_addedItem, int? get_index);

    internal record class ItemReplaceInPropertyCollectionPublication(PropertyInCollectonReplaceArgs get_parameterObject) : IPublication<PropertyInCollectonReplaceArgs>;
    internal record class PropertyInCollectonReplaceArgs(IValueContainerNonGeneric get_oldItem, IValueContainerNonGeneric get_newItem);

    internal record class PropertyCollectionChangePublication(IEnumerable<IValueContainerNonGeneric> get_parameterObject) : IPublication<IEnumerable<IValueContainerNonGeneric>>;
}
