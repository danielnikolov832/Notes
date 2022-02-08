using ToDoListLibrary;

namespace Testing.ToDoListLibrary
{
    internal static class ToDoListTest
    {
        private static void Main(string[] args)
        {
            TestConstruction();
            TestMethods();
        }

        private static void TestConstruction()
        {
            List<LimitedTimeNote> limitedTimeNoteCollection = new List<LimitedTimeNote>();

            DateTimeOffset soonTime = new DateTimeOffset(DateTime.Now.AddDays(2));

            limitedTimeNoteCollection.Add(new LimitedTimeNote("1", soonTime));
            limitedTimeNoteCollection.Add(new LimitedTimeNote("1", soonTime.AddHours(23.44)));

            ToDoList toDoList1 = new ToDoList(limitedTimeNoteCollection);

            limitedTimeNoteCollection.Add(new LimitedTimeNote("2", soonTime.AddMinutes(12.432)));

            ToDoList toDoList2 = new ToDoList(limitedTimeNoteCollection);
        }

        private static void TestMethods()
        {
            List<LimitedTimeNote> limitedTimeNoteCollection = new List<LimitedTimeNote>();

            DateTimeOffset soonTime = new DateTimeOffset(DateTime.Now.AddDays(2));

            LimitedTimeNote limitedTimeNote1 = new LimitedTimeNote("1", soonTime);
            LimitedTimeNote limitedTimeNote2 = new LimitedTimeNote("2", soonTime.AddMinutes(12.23));
            LimitedTimeNote limitedTimeNote3 = new LimitedTimeNote("3", soonTime.AddHours(34.22));

            limitedTimeNoteCollection.Add(limitedTimeNote1);
            limitedTimeNoteCollection.Add(limitedTimeNote3);
            limitedTimeNoteCollection.Add(limitedTimeNote2);
            // limitedTimeNoteCollection.Add(new LimitedTimeNote("2", DateTime.Now)); Causes : An unhandled exception of type 'System.ArgumentException' occurred in ToDoListLibrary.dll:
            // 'The argument 'finalTime' is invalid, because (finalTime =< DateTime.Now)' 

            ToDoList toDoList1 = new ToDoList(limitedTimeNoteCollection);

            toDoList1.Add(limitedTimeNote2);

            LimitedTimeNote limitedTimeNote4 = toDoList1[1];

            toDoList1.IndexOf(limitedTimeNote1);

            toDoList1.RemoveAt(2);
            
            toDoList1.Contains(limitedTimeNote2);

            toDoList1.Remove(limitedTimeNote1);

            LimitedTimeNote[] limitedTimeNoteArr = new LimitedTimeNote[toDoList1.Count + 2];

            toDoList1.CopyTo(limitedTimeNoteArr, 1);
            
            toDoList1.Clear();
        }
    }
}