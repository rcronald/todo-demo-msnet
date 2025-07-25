import React, { useState, useEffect, useRef } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { Toaster } from 'react-hot-toast';
import keycloak from './keycloak';
import { TaskProvider } from './contexts/TaskContext';
import ProtectedRoute from './components/ProtectedRoute';
import Layout from './components/Layout';
import Dashboard from './pages/Dashboard';
import Login from './pages/Login';
import './App.css';

function App() {
  const [keycloakInitialized, setKeycloakInitialized] = useState(false);
  const [authenticated, setAuthenticated] = useState(false);
  const initializationAttempted = useRef(false);

  useEffect(() => {
    const initializeKeycloak = async () => {
      if (initializationAttempted.current) return;
      initializationAttempted.current = true;

      try {
        const auth = await keycloak.init({
          onLoad: 'check-sso',
          checkLoginIframe: false,
          pkceMethod: 'S256',
          silentCheckSsoRedirectUri: window.location.origin + '/silent-check-sso.html'
        });

        setAuthenticated(auth);
        setKeycloakInitialized(true);

        // Set up event listeners
        keycloak.onAuthSuccess = () => {
          console.log('Auth success');
          setAuthenticated(true);
        };

        keycloak.onAuthLogout = () => {
          console.log('Auth logout');
          setAuthenticated(false);
        };

        keycloak.onAuthError = (error) => {
          console.error('Auth error:', error);
          setAuthenticated(false);
        };

        // Handle token refresh
        keycloak.onTokenExpired = () => {
          console.log('Token expired, refreshing...');
          keycloak.updateToken(30).catch(() => {
            console.log('Failed to refresh token');
            setAuthenticated(false);
          });
        };

      } catch (error) {
        console.error('Keycloak initialization failed', error);
        setKeycloakInitialized(true);
        setAuthenticated(false);
      }
    };

    initializeKeycloak();
  }, []);

  if (!keycloakInitialized) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="loading">
          <div className="spinner"></div>
          <span>Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <TaskProvider>
      <div className="App">
        <Router>
          <Routes>
            <Route path="/login" element={<Login />} />
            <Route path="/" element={
              <ProtectedRoute>
                <Layout>
                  <Dashboard />
                </Layout>
              </ProtectedRoute>
            } />
            <Route path="*" element={<Navigate to="/" replace />} />
          </Routes>
        </Router>
        <Toaster 
          position="top-right"
          toastOptions={{
            duration: 4000,
            style: {
              background: '#363636',
              color: '#fff',
            },
            success: {
              duration: 3000,
              theme: {
                primary: '#4aed88',
              },
            },
          }}
        />
      </div>
    </TaskProvider>
  );
}

export default App;