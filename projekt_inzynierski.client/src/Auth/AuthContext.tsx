
/* eslint-disable no-empty */
import React, { createContext, useState, useEffect, useContext } from "react";
import axios from "axios";

interface AuthContextType {
  isLogged: boolean;
  userRole: string | null;
  loading: boolean;
  login: (role: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isLogged, setIsLogged] = useState(false);
  const [userRole, setUserRole] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
    const API_URL = import.meta.env.VITE_API_URL;
  useEffect(() => {
    const checkAuth = async () => {
        try {
            const response = await axios.get(`${API_URL}/User/IsAuthenticated`, {
          withCredentials: true,
        });

            console.log("weryfikacja")
          const rola = await axios.get(`${API_URL}/User/IsAdmin`, {
              withCredentials: true,
          });
      
          
        if (response.data) {
          setIsLogged(true);
            setUserRole(rola.data || null);
          
        } else {
          setIsLogged(false);
          setUserRole(null);
       
        }
      } catch {
        setIsLogged(false);
        setUserRole(null);
      } finally {
        setLoading(false);
      }
    };

    checkAuth();
  }, []);

  const login = (role: string) => {
    setIsLogged(true);
    setUserRole(role);
  };

  const logout = async () => {
      try {
          await axios.post(`${ API_URL }/User/Logout`, {}, { withCredentials: true });
    } catch {}
    setIsLogged(false);
    setUserRole(null);
  };

  return (
    <AuthContext.Provider value={{ isLogged, userRole, loading, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error("useAuth must be used within AuthProvider");
  return context;
};
