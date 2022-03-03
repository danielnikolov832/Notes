using NotesLibrary;
using NotesLibrary.Notes;

namespace Testing.NotesLibrary
{
    internal class NoteWithSubNotesTest
    {
        private static void Main(string[] args)
        {
            TestConstruction();
        }

        private static void TestConstruction()
        {
            NoteWithSubNotes<Note> noteWithSubNotes1 = new NoteWithSubNotes<Note>("Basic1");

            NoteCollection<Note> noteCollection1 = new NoteCollection<Note>();

            Note note1 = new Note("1");
            Note note2 = new Note("2", "im 2");
            Note note3 = new Note("3", note1, "im 3");

            noteCollection1.Add(note1);
            noteCollection1.Add(note2);
            noteCollection1.Add(note3);

            NoteWithSubNotes<Note> noteWithSubNotes2 = new NoteWithSubNotes<Note>("Basic2", noteCollection1);

            note1.getset_name = "1 modified";

            noteWithSubNotes2.get_subNotes.Remove(note1);

            note1.getset_name = "1 modified twice";
        }
    }   
}