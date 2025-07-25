import React from 'react';
import { useTask } from '../contexts/TaskContext';
import { format, isToday, isPast } from 'date-fns';
import { Calendar, Edit, Trash2, CheckCircle, Circle } from 'lucide-react';
import toast from 'react-hot-toast';

const TaskItem = ({ task, onEdit }) => {
  const { toggleTaskStatus, deleteTask } = useTask();

  const handleToggleStatus = async () => {
    try {
      await toggleTaskStatus(task.id, !task.isCompleted);
    } catch (error) {
      console.error('Error toggling task status:', error);
    }
  };

  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      try {
        await deleteTask(task.id);
      } catch (error) {
        console.error('Error deleting task:', error);
      }
    }
  };

  const formatDueDate = (dueDate) => {
    if (!dueDate) return null;
    
    const date = new Date(dueDate);
    if (isToday(date)) {
      return { text: 'Today', className: 'due-date today' };
    } else if (isPast(date)) {
      return { text: format(date, 'MMM d'), className: 'due-date overdue' };
    } else {
      return { text: format(date, 'MMM d'), className: 'due-date' };
    }
  };

  const getPriorityClass = (priority) => {
    switch (priority?.toLowerCase()) {
      case 'high':
        return 'priority-badge priority-high';
      case 'medium':
        return 'priority-badge priority-medium';
      case 'low':
        return 'priority-badge priority-low';
      default:
        return 'priority-badge priority-medium';
    }
  };

  const dueDateInfo = formatDueDate(task.dueDate);

  return (
    <div className={`task-item ${task.isCompleted ? 'completed' : ''}`}>
      <button
        onClick={handleToggleStatus}
        className="task-checkbox"
        title={task.isCompleted ? 'Mark as incomplete' : 'Mark as complete'}
      >
        {task.isCompleted ? (
          <CheckCircle className="text-green-500" size={20} />
        ) : (
          <Circle className="text-gray-400" size={20} />
        )}
      </button>

      <div className="task-content">
        <h3 className={`task-title ${task.isCompleted ? 'completed' : ''}`}>
          {task.title}
        </h3>
        
        {task.description && (
          <p className="task-description">
            {task.description}
          </p>
        )}

        <div className="task-meta">
          {task.category && (
            <span 
              className="category-badge"
              style={{ backgroundColor: task.category.color }}
            >
              {task.category.name}
            </span>
          )}

          <span className={getPriorityClass(task.priority)}>
            {task.priority}
          </span>

          {dueDateInfo && (
            <span className={dueDateInfo.className}>
              <Calendar size={12} />
              {dueDateInfo.text}
            </span>
          )}

          {task.tags && task.tags.length > 0 && (
            <div className="tag-list">
              {task.tags.map((tag) => (
                <span key={tag.id} className="tag">
                  {tag.name}
                </span>
              ))}
            </div>
          )}
        </div>
      </div>

      <div className="task-actions">
        <button
          onClick={() => onEdit(task)}
          className="btn btn-ghost btn-sm"
          title="Edit task"
        >
          <Edit size={16} />
        </button>
        <button
          onClick={handleDelete}
          className="btn btn-ghost btn-sm text-red-500 hover:text-red-700"
          title="Delete task"
        >
          <Trash2 size={16} />
        </button>
      </div>
    </div>
  );
};

export default TaskItem;
