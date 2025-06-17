namespace MementoWebApp.Memento
{
    public class EditorHistory
    {
        private Stack<ArticleMemento> _undoStack = new();
        private Stack<ArticleMemento> _redoStack = new();

        public void SaveState(ArticleMemento memento)
        {
            _undoStack.Push(memento);
            _redoStack.Clear(); // Reset redo on new action
        }

        public ArticleMemento Undo(ArticleEditor editor)
        {
            if (_undoStack.Count == 0) return null;

            var current = editor.CreateMemento();
            _redoStack.Push(current);

            var previous = _undoStack.Pop();
            return previous;
        }

        public ArticleMemento Redo(ArticleEditor editor)
        {
            if (_redoStack.Count == 0) return null;

            var current = editor.CreateMemento();
            _undoStack.Push(current);

            var next = _redoStack.Pop();
            return next;
        }
    }
}
