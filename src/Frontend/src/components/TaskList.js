import React, { useState } from 'react';
import { useTask } from '../contexts/TaskContext';
import { Plus, Filter, X } from 'lucide-react';
import TaskItem from './TaskItem';
import TaskModal from './TaskModal';
import TaskFilters from './TaskFilters';

const TaskList = () => {
  const { tasks, loading, filters } = useTask();
  const [showTaskModal, setShowTaskModal] = useState(false);
  const [editingTask, setEditingTask] = useState(null);
  const [showFilters, setShowFilters] = useState(false);

  const handleCreateTask = () => {
    setEditingTask(null);
    setShowTaskModal(true);
  };

  const handleEditTask = (task) => {
    setEditingTask(task);
    setShowTaskModal(true);
  };

  const handleCloseModal = () => {
    setShowTaskModal(false);
    setEditingTask(null);
  };

  const hasActiveFilters = filters.category || filters.tag || filters.isCompleted !== null;

  if (loading) {
    return (
      <div className="loading">
        <div className="spinner"></div>
        <span>Loading tasks...</span>
      </div>
    );
  }

  return (
    <div>
      {/* Header */}
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-2xl font-bold text-gray-900">My Tasks</h1>
        <div className="flex items-center gap-3">
          <button
            onClick={() => setShowFilters(!showFilters)}
            className={`btn btn-secondary ${hasActiveFilters ? 'ring-2 ring-blue-500' : ''}`}
          >
            <Filter size={16} />
            Filters
            {hasActiveFilters && (
              <span className="ml-1 bg-blue-500 text-white text-xs rounded-full w-2 h-2"></span>
            )}
          </button>
          <button
            onClick={handleCreateTask}
            className="btn btn-primary"
          >
            <Plus size={16} />
            Add Task
          </button>
        </div>
      </div>

      {/* Filters */}
      {showFilters && (
        <div className="mb-6">
          <TaskFilters />
        </div>
      )}

      {/* Task List */}
      {tasks.length === 0 ? (
        <div className="empty-state">
          <div className="empty-state-icon">
            <svg
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
              className="w-12 h-12"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth={2}
                d="M9 5H7a2 2 0 00-2 2v10a2 2 0 002 2h8a2 2 0 002-2V7a2 2 0 00-2-2h-2M9 5a2 2 0 002 2h2a2 2 0 002-2M9 5a2 2 0 012-2h2a2 2 0 012 2"
              />
            </svg>
          </div>
          <h3 className="empty-state-title">No tasks found</h3>
          <p className="empty-state-description">
            {hasActiveFilters 
              ? 'Try adjusting your filters or create a new task.'
              : 'Get started by creating your first task.'
            }
          </p>
          <button
            onClick={handleCreateTask}
            className="btn btn-primary"
          >
            <Plus size={16} />
            Create First Task
          </button>
        </div>
      ) : (
        <div className="space-y-3">
          {tasks.map((task) => (
            <TaskItem
              key={task.id}
              task={task}
              onEdit={handleEditTask}
            />
          ))}
        </div>
      )}

      {/* Task Modal */}
      {showTaskModal && (
        <TaskModal
          task={editingTask}
          onClose={handleCloseModal}
        />
      )}
    </div>
  );
};

export default TaskList;
