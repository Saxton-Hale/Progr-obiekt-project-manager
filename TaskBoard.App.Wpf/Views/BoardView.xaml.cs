using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TaskBoard.App.Wpf.ViewModels;
using TaskBoard.Domain.Entities;

namespace TaskBoard.App.Wpf.Views
{
    public partial class BoardView : UserControl
    {
        public BoardView()
        {
            InitializeComponent();
        }

        private Point _dragStartPoint;
        private bool _suppressDrag;

        private void TasksList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _dragStartPoint = e.GetPosition(null);

            _suppressDrag = e.ClickCount > 1;
        }

        private void TasksList_MouseMove(object sender, MouseEventArgs e)
        {
            if (_suppressDrag) return;
            if (e.LeftButton != MouseButtonState.Pressed) return;

            var pos = e.GetPosition(null);
            var diff = _dragStartPoint - pos;

            if (Math.Abs(diff.X) < SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(diff.Y) < SystemParameters.MinimumVerticalDragDistance)
                return;

            if (sender is not ListBox listBox) return;

            if (listBox.SelectedItem is not TaskItem task) return;

            if (listBox.DataContext is not Column fromColumn) return;

            var data = new DataObject();
            data.SetData(typeof(TaskItem), task);
            data.SetData("FromColumn", fromColumn);

            DragDrop.DoDragDrop(listBox, data, DragDropEffects.Move);
        }

        private void TasksList_Drop(object sender, DragEventArgs e)
        {
            if (DataContext is not BoardViewModel vm) return;
            if (sender is not ListBox targetListBox) return;

            if (targetListBox.DataContext is not Column toColumn) return;

            var task = e.Data.GetData(typeof(TaskItem)) as TaskItem;
            var fromColumn = e.Data.GetData("FromColumn") as Column;

            if (task is null || fromColumn is null) return;
            if (fromColumn.Id == toColumn.Id) return;

            vm.Board?.MoveTask(task.Id, fromColumn.Id, toColumn.Id);

            vm.SetBoard(vm.Board);
        }

        private void TaskItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not BoardViewModel vm) return;
            if (sender is not ListBoxItem item) return;
            if (item.DataContext is not TaskItem task) return;

            _suppressDrag = true;

            vm.OpenTaskDetailsCommand.Execute(task);

            e.Handled = true;
        }

        private void ColumnBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DataContext is not BoardViewModel vm) return;

            if (sender is not System.Windows.Controls.Border border) return;
            if (border.DataContext is not Column col) return;

            vm.SelectedColumn = col;
            e.Handled = true;
        }
    }
}