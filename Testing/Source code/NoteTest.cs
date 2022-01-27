using NotesLibrary;

namespace Testing
{
    internal class NoteTest
    {
        static void Main(string[] args)
        {
            // TestConstruction();
            TestSubNotes();
        }

        private static void TestConstruction()
        {
            Note note1 = new Note("Basic1", "Do 1");

            Note note2 = new Note("Basic2", subNotes: new List<Note>() { note1 }, "Do 2");
        }

        private static void TestSubNotes()
        {
            Note note1 = new Note("Basic1", "Do 1");

            Note note2 = new Note("Basic2", subNotes: new List<Note>() { note1 }, "Do 2");
            Note note3 = new Note("Basic3", subNotes: new List<Note>(), "Do 3");

            note1.AddSubNote(note2);
            Console.WriteLine(note1.Temp_GetEventInvocationListAsString());

            note1.RemoveSubNote(note2);
            Console.WriteLine(note1.Temp_GetEventInvocationListAsString());
        }
    }   
}