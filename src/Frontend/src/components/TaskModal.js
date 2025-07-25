import React, { useState, useEffect } from 'react';
import { useTask } from '../contexts/TaskContext';
import { useForm, Controller } from 'react-hook-form';
import DatePicker from 'react-datepicker';
import CreatableSelect from 'react-select/creatable';
import { X, Calendar } from 'lucide-react';
import 'react-datepicker/dist/react-datepicker.css';

const TaskModal = ({ task, onClose }) => {
  const { createTask, updateTask, categories, tags } = useTask();
  const [loading, setLoading] = useState(false);
  
  const {
    register,
    handleSubmit,
    control,
    formState: { errors },
    reset,
    watch
  } = useForm({
    defaultValues: {
      title: task?.title || '',
      description: task?.description || '',
      dueDate: task?.dueDate ? new Date(task.dueDate) : null,
      categoryId: task?.category?.id || '',
      priority: task?.priority || 'Medium',
      tags: task?.tags?.map(tag => ({ value: tag.name, label: tag.name })) || []
    }
  });

  const isEditing = !!task;

  useEffect(() => {
    if (task) {
      reset({
        title: task.title,
        description: task.description || '',
        dueDate: task.dueDate ? new Date(task.dueDate) : null,
        categoryId: task.category?.id || '',
        priority: task.priority || 'Medium',
        tags: task.tags?.map(tag => ({ value: tag.name, label: tag.name })) || []
      });
    }
  }, [task, reset]);

  const onSubmit = async (data) => {
    try {
      setLoading(true);
      
      const taskData = {
        title: data.title,
        description: data.description,
        dueDate: data.dueDate?.toISOString() || null,
        categoryId: data.categoryId || null,
        priority: data.priority,
        tags: data.tags.map(tag => tag.value)
      };

      if (isEditing) {
        await updateTask(task.id, taskData);
      } else {
        await createTask(taskData);
      }
      
      onClose();
    } catch (error) {
      console.error('Error saving task:', error);
    } finally {
      setLoading(false);
    }
  };

  const tagOptions = tags.map(tag => ({
    value: tag.name,
    label: tag.name
  }));

  const categoryOptions = categories.map(category => ({
    value: category.id,
    label: category.name
  }));

  return (
    <div className="modal-overlay" onClick={onClose}>
      <div className="modal" onClick={e => e.stopPropagation()}>
        <div className="modal-header">
          <h2 className="modal-title">
            {isEditing ? 'Edit Task' : 'Create New Task'}
          </h2>
          <button
            onClick={onClose}
            className="btn btn-ghost btn-sm"
          >
            <X size={16} />
          </button>
        </div>

        <form onSubmit={handleSubmit(onSubmit)}>
          <div className="modal-content">
            {/* Title */}
            <div className="form-group">
              <label className="form-label">Title *</label>
              <input
                type="text"
                className="form-input"
                {...register('title', { 
                  required: 'Title is required',
                  maxLength: { value: 255, message: 'Title must be less than 255 characters' }
                })}
                placeholder="Enter task title..."
              />
              {errors.title && (
                <div className="form-error">{errors.title.message}</div>
              )}
            </div>

            {/* Description */}
            <div className="form-group">
              <label className="form-label">Description</label>
              <textarea
                className="form-textarea"
                rows={3}
                {...register('description', {
                  maxLength: { value: 1000, message: 'Description must be less than 1000 characters' }
                })}
                placeholder="Enter task description..."
              />
              {errors.description && (
                <div className="form-error">{errors.description.message}</div>
              )}
            </div>

            {/* Due Date */}
            <div className="form-group">
              <label className="form-label">Due Date</label>
              <Controller
                name="dueDate"
                control={control}
                render={({ field }) => (
                  <DatePicker
                    selected={field.value}
                    onChange={field.onChange}
                    dateFormat="MMMM d, yyyy"
                    placeholderText="Select due date..."
                    className="form-input"
                    minDate={new Date()}
                    showPopperArrow={false}
                    customInput={
                      <div className="relative">
                        <input
                          className="form-input pr-10"
                          placeholder="Select due date..."
                          readOnly
                        />
                        <Calendar 
                          size={16} 
                          className="absolute right-3 top-1/2 transform -translate-y-1/2 text-gray-400"
                        />
                      </div>
                    }
                  />
                )}
              />
            </div>

            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              {/* Category */}
              <div className="form-group">
                <label className="form-label">Category</label>
                <select
                  className="form-select"
                  {...register('categoryId')}
                >
                  <option value="">Select category...</option>
                  {categories.map(category => (
                    <option key={category.id} value={category.id}>
                      {category.name}
                    </option>
                  ))}
                </select>
              </div>

              {/* Priority */}
              <div className="form-group">
                <label className="form-label">Priority</label>
                <select
                  className="form-select"
                  {...register('priority')}
                >
                  <option value="Low">Low</option>
                  <option value="Medium">Medium</option>
                  <option value="High">High</option>
                </select>
              </div>
            </div>

            {/* Tags */}
            <div className="form-group">
              <label className="form-label">Tags</label>
              <Controller
                name="tags"
                control={control}
                render={({ field }) => (
                  <CreatableSelect
                    {...field}
                    isMulti
                    options={tagOptions}
                    placeholder="Select or create tags..."
                    className="react-select-container"
                    classNamePrefix="react-select"
                    formatCreateLabel={(inputValue) => `Create "${inputValue}"`}
                  />
                )}
              />
            </div>
          </div>

          <div className="modal-footer">
            <button
              type="button"
              onClick={onClose}
              className="btn btn-secondary"
              disabled={loading}
            >
              Cancel
            </button>
            <button
              type="submit"
              className="btn btn-primary"
              disabled={loading}
            >
              {loading ? (
                <>
                  <div className="spinner" />
                  {isEditing ? 'Updating...' : 'Creating...'}
                </>
              ) : (
                isEditing ? 'Update Task' : 'Create Task'
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default TaskModal;
