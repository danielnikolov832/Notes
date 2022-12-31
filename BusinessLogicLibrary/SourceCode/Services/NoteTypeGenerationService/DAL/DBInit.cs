using BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL.DAOs;
using SqliteDALLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLibrary.Services.NoteTypeGenerationService.DAL;
internal class DBInit
{
	public DBInit()
	{
		const string dbConnectionStringName = "NoteType";

		DB = SqliteDatabaseAccessLayer.CreateInstance(dbConnectionStringName);

		DB.CreateTableWithPrimaryKeyChain<TypeCategoryDao>()
			.CreateTableWithPrimaryKeyChain<NoteTypeDao>()
			.CreateTableWithPrimaryKeyChain<NoteDataDao>()
			.CreateTableWithPrimaryKeyChain<ValueContainerDataDao>();
	}

	public ISqliteDatabaseAccessLayer DB { get; init; }
}