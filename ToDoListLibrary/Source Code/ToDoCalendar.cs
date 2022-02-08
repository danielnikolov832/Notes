namespace ToDoListLibrary
{
    // Defines an object capable of working with a multitude of ToDoList objects
    // to create a calendar of LimitedTimeNote objects, with the end goal of 
    // organizing people's notes in one place and in a human readable timeline.
    public class ToDoCalendar : ToDoList
    {
        private List<ToDoList> _toDoLists;

        public ToDoCalendar() : base()
        {
            _toDoLists = new List<ToDoList>();
        }

        public ToDoCalendar(List<LimitedTimeNote> limitedTimeNoteList) : base(limitedTimeNoteList)
        {
            _toDoLists = new List<ToDoList>();
        }

        public ToDoCalendar(IEnumerable<ToDoList> toDoLists) : base()
        {
            _toDoLists = toDoLists.ToList();

            AddToDoLists(_toDoLists);
        }

        public void AddToDoLists(IEnumerable<ToDoList> toDoLists)
        {
            foreach (ToDoList toDoListItem in toDoLists)
            {
                AddToDoList(toDoListItem);
            }
        }

        public void AddToDoList(ToDoList toDoList)
        {
            foreach (LimitedTimeNote limitedTimeNoteItem in toDoList)
            {
                Add(limitedTimeNoteItem);
            }
        }
    }
}