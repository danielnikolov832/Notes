using NotesLibrary.Notes;

namespace Testing.NotesLibrary
{
    internal class NoteTest
    {
        static void Main(string[] args)
        {
            TestConstruction();
        }

        private static void TestConstruction()
        {
            Note note1 = new Note("Basic1");

            Note note2 = new Note("Basic2", "Do 2");
        }
    }   
}