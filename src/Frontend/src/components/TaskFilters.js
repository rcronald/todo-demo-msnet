import React from 'react';
import { useTask } from '../contexts/TaskContext';
import { FilterX } from 'lucide-react';

const TaskFilters = () => {
  const { categories, tags, filters, updateFilters, clearFilters } = useTask();

  const handleCategoryChange = (categoryName) => {
    updateFilters({ category: categoryName === filters.category ? null : categoryName });
  };

  const handleTagChange = (tagName) => {
    updateFilters({ tag: tagName === filters.tag ? null : tagName });
  };

  const handleStatusChange = (status) => {
    updateFilters({ isCompleted: status === filters.isCompleted ? null : status });
  };

  const hasActiveFilters = filters.category || filters.tag || filters.isCompleted !== null;

  return (
    <div className="filters">
      <div className="flex items-center justify-between mb-4">
        <h3 className="text-lg font-semibold text-gray-900">Filters</h3>
        {hasActiveFilters && (
          <button
            onClick={clearFilters}
            className="btn btn-ghost btn-sm"
          >
            <FilterX size={16} />
            Clear Filters
          </button>
        )}
      </div>

      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {/* Status Filter */}
        <div className="filter-group">
          <span className="filter-label">Status</span>
          <div className="flex flex-wrap gap-2">
            <button
              onClick={() => handleStatusChange(false)}
              className={`btn btn-sm ${
                filters.isCompleted === false 
                  ? 'btn-primary' 
                  : 'btn-secondary'
              }`}
            >
              Active
            </button>
            <button
              onClick={() => handleStatusChange(true)}
              className={`btn btn-sm ${
                filters.isCompleted === true 
                  ? 'btn-primary' 
                  : 'btn-secondary'
              }`}
            >
              Completed
            </button>
          </div>
        </div>

        {/* Category Filter */}
        <div className="filter-group">
          <span className="filter-label">Category</span>
          <div className="flex flex-wrap gap-2">
            {categories.map((category) => (
              <button
                key={category.id}
                onClick={() => handleCategoryChange(category.name)}
                className={`btn btn-sm ${
                  filters.category === category.name 
                    ? 'btn-primary' 
                    : 'btn-secondary'
                }`}
                style={{
                  backgroundColor: filters.category === category.name 
                    ? category.color 
                    : undefined
                }}
              >
                {category.name}
              </button>
            ))}
          </div>
        </div>

        {/* Tag Filter */}
        <div className="filter-group">
          <span className="filter-label">Tags</span>
          <div className="flex flex-wrap gap-2">
            {tags.length > 0 ? (
              tags.map((tag) => (
                <button
                  key={tag.id}
                  onClick={() => handleTagChange(tag.name)}
                  className={`btn btn-sm ${
                    filters.tag === tag.name 
                      ? 'btn-primary' 
                      : 'btn-secondary'
                  }`}
                >
                  {tag.name}
                </button>
              ))
            ) : (
              <span className="text-sm text-gray-500">No tags available</span>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskFilters;
