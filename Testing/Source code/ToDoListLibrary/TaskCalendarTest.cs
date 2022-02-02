using NotesLibrary;
using ToDoListLibrary;

namespace Testing.ToDoListLibrary
{
    internal static class TaskCalendarTest
    {
        private static void Main(string[] args)
        {
            TestConstruction();
        }

        private static void TestConstruction()
        {
            List<NoteCollection<LimitedTimeNote>> noteCollectionList = new List<NoteCollection<LimitedTimeNote>>()
            {
                new NoteCollection<LimitedTimeNote>()
                {
                    new LimitedTimeNote("Note1", new DateTime(2022, 3, 2, 2, 59, 34)),
                    new LimitedTimeNote("Note2", new DateTime(2022, 4, 2)),
                    new LimitedTimeNote("Note3", new DateTime(2022, 11, 3))
                },

                new NoteCollection<LimitedTimeNote>()
                {
                    new LimitedTimeNote("Note1", new DateTime(2022, 3, 2)),
                    new LimitedTimeNote("Note2", new DateTime(2022, 4, 3)),
                    new LimitedTimeNote("Note3", new DateTime(2022, 11, 7))
                }
            };

            TaskCalendar taskCalendar1 = new TaskCalendar(noteCollectionList);

            List<LimitedTimeNote> sortedTasks = taskCalendar1.notesSortedByTime;

            foreach (LimitedTimeNote limitedTimeNoteItem in sortedTasks)
            {
                Console.WriteLine($"{limitedTimeNoteItem.getset_name} is due {limitedTimeNoteItem.get_finalTime}");    
            } 
        }
    }
}