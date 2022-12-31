using Mapster;
using SqliteDALLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.Models;
public abstract class PrivatePrimaryKeyUserWithNoDBUse
{
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable RCS1170 // Use read-only auto-implemented property.
    [AdaptMember(nameof(PrimaryKeyUser.Id))]
    internal int Id { get; set; }
#pragma warning restore RCS1170 // Use read-only auto-implemented property.
#pragma warning restore IDE0051 // Remove unused private members
}
