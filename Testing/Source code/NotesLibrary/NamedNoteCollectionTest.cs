using NotesLibrary;
using NotesLibrary.Notes;

namespace Testing.NotesLibrary
{
    internal static class NamedNoteCollectionTest
    {
        private static void Main(string[] args)
        {
            TestConstruction();
            TestMethods();
        }

        private static void TestConstruction()
        {
            NamedNoteCollection<Note> noteCollection1 = new NamedNoteCollection<Note>("basic collection"); 

            List<Note> notesList = new List<Note>()
            {
                new Note("Num 1"),
                new Note("Num 2"),
                new Note("Num 3")
            };

            noteCollection1.AddRange(notesList);            
        }

        private static void TestMethods()
        {
            NamedNoteCollection<Note> noteCollection1 = new NamedNoteCollection<Note>("basic collection");

            Note note1 = new Note("Num 1");
            Note note2 = new Note("Num 2");
            Note note3 = new Note("Num 3");

            List<Note> notesList = new List<Note>()
            {
                note1,
                note2,
                note3
            };

            noteCollection1.TryAdd(note1);
            noteCollection1.TryAddRange(notesList);

            noteCollection1[0] = note1;

            noteCollection1.Remove(note2);
            noteCollection1.RemoveAt(1);

            noteCollection1.GetNoteWithName("Num 1");

            noteCollection1.TryRemoveNoteWithName("Num 1");

            noteCollection1.AddRange(notesList);

            noteCollection1.RemoveAllWhere((Note note) => {
                return note.getset_name == "Num 3";
            });

            noteCollection1.RemoveAll();
        }
    }
}