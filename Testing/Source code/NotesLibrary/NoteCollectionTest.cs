using NotesLibrary;
using NotesLibrary.Notes;

namespace Testing.NotesLibrary
{
    internal static class NoteCollectionTest
    {
        private static void Main(string[] args)
        {
            TestConstruction();
            TestMethods();
            TestItemBinding();
        }

        private static void TestConstruction()
        {
            NoteCollection<Note> noteCollection1 = new NoteCollection<Note>(); 

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
            NoteCollection<Note> noteCollection1 = new NoteCollection<Note>();

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

            // noteCollection1.MoveItemFromOldToNewIndex(1, 2); Member NoteCollection.MoveItem was made inaccessable, but still works;

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
    
        // Tests the binding this collection performs to its items
        private static void TestItemBinding()
        {
            NoteCollection<Note> noteCollection1 = new NoteCollection<Note>();

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

            note1.getset_name = "Num 1 modified";

            // note3.getset_name = "Num 1 modified";  Causes : Unhandled exception. System.ArgumentException: There is a note, equal to this one
            // at NotesLibrary.NoteCollection`1.OnInvalidItem(TNote note) in C:\Users\User\Desktop\THE_PARENT\OUTSIDE_PROGRAMMING\Pure C-Sharp\Notes\NotesLibrary\Source code\NoteCollection.cs:line 168

            noteCollection1[0] = note1;

            // noteCollection1[1] = note1;  Causes : Unhandled exception. System.ArgumentException: There is a note, equal to this one
            // at NotesLibrary.NoteCollection`1.OnInvalidItem(TNote note) in C:\Users\User\Desktop\THE_PARENT\OUTSIDE_PROGRAMMING\Pure C-Sharp\Notes\NotesLibrary\Source code\NoteCollection.cs:line 168

            noteCollection1.Remove(note2);

            note2.getset_name = "Num 2 modified";

            noteCollection1.RemoveAt(1);

            Note? note1FromCollection = noteCollection1.GetNoteWithName("Num 1 modified");

            if (note1FromCollection != null)
            {
                note1FromCollection.getset_name = "Num 1 modified twice";
            }

            noteCollection1.TryRemoveNoteWithName("Num 1 modified twice");

            if (note1FromCollection != null)
            {
                note1FromCollection.getset_name = "Num 1 modified trice";
            }

            noteCollection1.AddRange(notesList);

            noteCollection1.RemoveAllWhere((Note note) => {
                return note.getset_name == "Num 3";
            });

            noteCollection1.RemoveAll();
        }
    }
}