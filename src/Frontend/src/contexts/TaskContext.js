import React, { createContext, useContext, useState, useEffect } from 'react';
import { taskService } from '../services/apiService';
import toast from 'react-hot-toast';

const TaskContext = createContext();

export const useTask = () => {
  const context = useContext(TaskContext);
  if (!context) {
    throw new Error('useTask must be used within a TaskProvider');
  }
  return context;
};

export const TaskProvider = ({ children }) => {
  const [tasks, setTasks] = useState([]);
  const [categories, setCategories] = useState([]);
  const [tags, setTags] = useState([]);
  const [loading, setLoading] = useState(false);
  const [filters, setFilters] = useState({
    category: null,
    tag: null,
    isCompleted: null
  });

  // Load initial data
  useEffect(() => {
    loadTasks();
    loadCategories();
    loadTags();
  }, []);

  // Reload tasks when filters change
  useEffect(() => {
    loadTasks();
  }, [filters]);

  const loadTasks = async () => {
    try {
      setLoading(true);
      const response = await taskService.getTasks(filters);
      if (response.success) {
        setTasks(response.data);
      }
    } catch (error) {
      toast.error('Failed to load tasks');
      console.error('Error loading tasks:', error);
    } finally {
      setLoading(false);
    }
  };

  const loadCategories = async () => {
    try {
      const response = await taskService.getCategories();
      if (response.success) {
        setCategories(response.data);
      }
    } catch (error) {
      console.error('Error loading categories:', error);
    }
  };

  const loadTags = async () => {
    try {
      const response = await taskService.getTags();
      if (response.success) {
        setTags(response.data);
      }
    } catch (error) {
      console.error('Error loading tags:', error);
    }
  };

  const createTask = async (taskData) => {
    try {
      const response = await taskService.createTask(taskData);
      if (response.success) {
        await loadTasks(); // Reload tasks
        await loadTags(); // Reload tags in case new ones were created
        toast.success('Task created successfully');
        return response.data;
      }
    } catch (error) {
      toast.error('Failed to create task');
      throw error;
    }
  };

  const updateTask = async (id, taskData) => {
    try {
      const response = await taskService.updateTask(id, taskData);
      if (response.success) {
        await loadTasks(); // Reload tasks
        await loadTags(); // Reload tags in case new ones were created
        toast.success('Task updated successfully');
        return response.data;
      }
    } catch (error) {
      toast.error('Failed to update task');
      throw error;
    }
  };

  const toggleTaskStatus = async (id, isCompleted) => {
    try {
      const response = await taskService.updateTaskStatus(id, isCompleted);
      if (response.success) {
        await loadTasks(); // Reload tasks
        toast.success(`Task marked as ${isCompleted ? 'completed' : 'incomplete'}`);
        return response.data;
      }
    } catch (error) {
      toast.error('Failed to update task status');
      throw error;
    }
  };

  const deleteTask = async (id) => {
    try {
      const response = await taskService.deleteTask(id);
      if (response.success) {
        await loadTasks(); // Reload tasks
        toast.success('Task deleted successfully');
      }
    } catch (error) {
      toast.error('Failed to delete task');
      throw error;
    }
  };

  const updateFilters = (newFilters) => {
    setFilters(prev => ({ ...prev, ...newFilters }));
  };

  const clearFilters = () => {
    setFilters({
      category: null,
      tag: null,
      isCompleted: null
    });
  };

  const value = {
    // Data
    tasks,
    categories,
    tags,
    loading,
    filters,

    // Actions
    createTask,
    updateTask,
    toggleTaskStatus,
    deleteTask,
    updateFilters,
    clearFilters,
    refreshTasks: loadTasks,
    refreshTags: loadTags
  };

  return (
    <TaskContext.Provider value={value}>
      {children}
    </TaskContext.Provider>
  );
};
