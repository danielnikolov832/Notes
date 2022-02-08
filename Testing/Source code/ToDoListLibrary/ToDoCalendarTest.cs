using ToDoListLibrary;

namespace Testing.ToDoListLibrary
{
    internal static class ToDoCalendarTest
    {
        private static void Main(string[] args)
        {
            TestConstruction();
            TestMethods();
        }

        private static void TestConstruction()
        {
            ToDoCalendar toDoCalendar1 = new ToDoCalendar();

            DateTimeOffset soonTime = new DateTimeOffset(DateTime.Now.AddDays(2));

            LimitedTimeNote limitedTimeNote1 = new LimitedTimeNote("1", soonTime);
            LimitedTimeNote limitedTimeNote2 = new LimitedTimeNote("2", soonTime.AddMinutes(12.23));
            LimitedTimeNote limitedTimeNote3 = new LimitedTimeNote("3", soonTime.AddHours(34.22));

            List<LimitedTimeNote> limitedTimeNoteList = new List<LimitedTimeNote>();

            limitedTimeNoteList.Add(limitedTimeNote1);
            limitedTimeNoteList.Add(limitedTimeNote3);
            limitedTimeNoteList.Add(limitedTimeNote2);

            ToDoCalendar toDoCalendar2 = new ToDoCalendar(limitedTimeNoteList);

            List<ToDoList> toDoListList = new List<ToDoList>()
            {
                new ToDoList()
                {
                    limitedTimeNote3, 
                    limitedTimeNote1, 
                    limitedTimeNote2
                },

                new ToDoList(limitedTimeNoteList)
            };

            ToDoCalendar toDoCalendar3 = new ToDoCalendar(toDoListList);
        }

        private static void TestMethods()
        {
            ToDoCalendar toDoCalendar1 = new ToDoCalendar();

            DateTimeOffset soonTime = new DateTimeOffset(DateTime.Now.AddDays(2));

            LimitedTimeNote limitedTimeNote1 = new LimitedTimeNote("1", soonTime);
            LimitedTimeNote limitedTimeNote2 = new LimitedTimeNote("2", soonTime.AddMinutes(12.23));
            LimitedTimeNote limitedTimeNote3 = new LimitedTimeNote("3", soonTime.AddHours(34.22));

            List<LimitedTimeNote> limitedTimeNoteCollection1 = new List<LimitedTimeNote>();

            limitedTimeNoteCollection1.Add(limitedTimeNote1);
            limitedTimeNoteCollection1.Add(limitedTimeNote3);
            limitedTimeNoteCollection1.Add(limitedTimeNote2);

            ToDoCalendar toDoCalendar2 = new ToDoCalendar(limitedTimeNoteCollection1);

            List<ToDoList> toDoListList1 = new List<ToDoList>()
            {
                new ToDoList()
                {
                    limitedTimeNote3, 
                    limitedTimeNote1, 
                    limitedTimeNote2
                },

                new ToDoList(limitedTimeNoteCollection1)
            };

            ToDoCalendar toDoCalendar3 = new ToDoCalendar(toDoListList1);

            List<LimitedTimeNote> limitedTimeNoteCollection2 = new List<LimitedTimeNote>();

            LimitedTimeNote limitedTimeNote4 = new LimitedTimeNote("4", DateTimeOffset.MaxValue);

            limitedTimeNoteCollection2.Add(limitedTimeNote1);
            limitedTimeNoteCollection2.Add(limitedTimeNote4);
            limitedTimeNoteCollection2.Add(limitedTimeNote2);

            ToDoList toDoList1 = new ToDoList(limitedTimeNoteCollection2);

            List<ToDoList> toDoListList2 = new List<ToDoList>(toDoListList1);

            toDoListList2.Add(toDoList1);

            toDoCalendar1.AddToDoList(toDoList1);
            toDoCalendar1.AddToDoLists(toDoListList2);

            toDoCalendar2.AddToDoList(toDoList1);
            toDoCalendar2.AddToDoLists(toDoListList2);

            toDoCalendar3.AddToDoList(toDoList1);
            toDoCalendar3.AddToDoLists(toDoListList2);
        }
    }
}