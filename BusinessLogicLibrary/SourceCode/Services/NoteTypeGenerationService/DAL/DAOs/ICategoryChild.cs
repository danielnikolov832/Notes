using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;
internal interface ICategoryChild
{
    string getset_name { get; }
    TypeCategoryDao get_parentCategoryInverseProperty { get; }
    int get_parentCategoryForeignKey { get; set; }
}
