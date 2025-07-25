import React from 'react';
import { LogOut, User } from 'lucide-react';
import keycloak from '../keycloak';

const Layout = ({ children }) => {
  const handleLogout = () => {
    keycloak.logout();
  };

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="header">
        <div className="container">
          <div className="header-content">
            <div className="logo">
              TODO App
            </div>
            <div className="user-menu">
              <div className="user-info">
                <User size={16} />
                <span>{keycloak.tokenParsed?.preferred_username || 'User'}</span>
              </div>
              <button 
                onClick={handleLogout}
                className="btn btn-ghost btn-sm"
                title="Logout"
              >
                <LogOut size={16} />
                Logout
              </button>
            </div>
          </div>
        </div>
      </header>
      <main className="container py-8">
        {children}
      </main>
    </div>
  );
};

export default Layout;
