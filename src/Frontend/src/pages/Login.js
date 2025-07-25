import React, { useEffect, useRef } from 'react';
import { Navigate } from 'react-router-dom';
import { CheckSquare } from 'lucide-react';
import keycloak from '../keycloak';

const Login = () => {
  const loginAttempted = useRef(false);

  useEffect(() => {
    if (!keycloak.authenticated && !loginAttempted.current) {
      loginAttempted.current = true;
      const timer = setTimeout(() => {
        keycloak.login();
      }, 100); // Small delay to prevent immediate loop

      return () => clearTimeout(timer);
    }
  }, []);

  if (keycloak.authenticated) {
    return <Navigate to="/" replace />;
  }

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <div className="max-w-md w-full space-y-8">
        <div className="text-center">
          <div className="flex justify-center">
            <CheckSquare size={64} className="text-blue-500" />
          </div>
          <h2 className="mt-6 text-3xl font-bold text-gray-900">
            Welcome to TODO App
          </h2>
          <p className="mt-2 text-sm text-gray-600">
            Sign in to manage your tasks efficiently
          </p>
        </div>
        
        <div className="bg-white py-8 px-6 shadow-sm rounded-lg">
          <div className="text-center">
            <div className="loading mb-4">
              <div className="spinner"></div>
              <span>Redirecting to login...</span>
            </div>
            <button
              onClick={() => {
                if (!loginAttempted.current) {
                  loginAttempted.current = true;
                  keycloak.login();
                }
              }}
              className="btn btn-primary btn-lg w-full"
            >
              Sign In Manually
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Login;