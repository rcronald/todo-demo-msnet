import api from '../api';

export const taskService = {
  // Get all tasks with optional filters
  getTasks: async (filters = {}) => {
    const params = new URLSearchParams();
    if (filters.category) params.append('category', filters.category);
    if (filters.tag) params.append('tag', filters.tag);
    if (filters.isCompleted !== undefined && filters.isCompleted !== null) params.append('isCompleted', filters.isCompleted);

    console.log('Fetching tasks with filters:', params.toString());
    
    const response = await api.get(`/api/tasks?${params}`);
    return response.data;
  },

  // Get task by ID
  getTask: async (id) => {
    const response = await api.get(`/api/tasks/${id}`);
    return response.data;
  },

  // Create new task
  createTask: async (taskData) => {
    const response = await api.post('/api/tasks', taskData);
    return response.data;
  },

  // Update task
  updateTask: async (id, taskData) => {
    const response = await api.put(`/api/tasks/${id}`, taskData);
    return response.data;
  },

  // Update task status
  updateTaskStatus: async (id, isCompleted) => {
    const response = await api.patch(`/api/tasks/${id}/status`, { isCompleted });
    return response.data;
  },

  // Delete task
  deleteTask: async (id) => {
    const response = await api.delete(`/api/tasks/${id}`);
    return response.data;
  },

  // Get categories
  getCategories: async () => {
    const response = await api.get('/api/categories');
    return response.data;
  },

  // Get tags
  getTags: async () => {
    const response = await api.get('/api/tags');
    return response.data;
  },

  // Create tag
  createTag: async (tagData) => {
    const response = await api.post('/api/tags', tagData);
    return response.data;
  }
};

export const userService = {
  // Get user profile
  getProfile: async () => {
    const response = await api.get('/api/users/profile');
    return response.data;
  },

  // Create user profile
  createProfile: async (profileData) => {
    const response = await api.post('/api/users/profile', profileData);
    return response.data;
  },

  // Update user profile
  updateProfile: async (profileData) => {
    const response = await api.put('/api/users/profile', profileData);
    return response.data;
  }
};
