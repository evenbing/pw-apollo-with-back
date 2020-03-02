import React, { useEffect, useState, useContext, ReactNode } from 'react';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'

import { ApiContext } from '../ApiProvider/ApiProvider';
import { ISessionInfo, ILoginOptions, ISignUpOptions } from '../../models/backendModels';
import { toastResponseErrors } from '../../api/api';

export interface ISessionContext {
  session: ISessionInfo | null;
  refreshSession: () => void;
  login: (creds: ILoginOptions) => Promise<void>;
  signUp: (creds: ISignUpOptions) => Promise<void>;
  logout: () => Promise<void>;
}

export const SessionContext = React.createContext<ISessionContext>({
  session: null,
  refreshSession: () => {},
  login: () => Promise.reject(),
  signUp: () => Promise.reject(),
  logout: () => Promise.reject(),
});

export default function SessionProvider({ children }: { children?: ReactNode}) {
  const api = useContext(ApiContext);
  const [session, setSession] = useState<ISessionInfo | null>(null);  
  const [hubConnection, setHubConnection] = useState<HubConnection | null>(null);

  const updateBalance = (data: number) => {
    setSession(session === null ? null : {userName: session.userName, balance : data});
  }

  const refreshSession = () => {
    fetchSession();
  }

  const login = async (options: ILoginOptions) => {    
    const data = await api.session.login(options);
    setSession(data);
    setHubConnection(new HubConnectionBuilder()
      .withUrl('/balance')
      .build());
  };

  const signUp = async (options: ISignUpOptions) => {
    const data = await api.session.signUp(options);
    setSession(data);
  };

  const logout = async () => {
    try {
      await api.session.logout();
    } catch (ex) {
      toastResponseErrors(ex.response?.data);
    } finally {
      setSession(null);
    }
  };

  const fetchSession = async () => {
    try {
      const data = await api.session.getSessionInfo();
      setSession(data);
    } catch {
      //ignore
    }
  };

  useEffect(() => {
    try {
        fetchSession();
    } catch {
        setSession(null);
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (hubConnection) {
      hubConnection.on('UpdateBalance', updateBalance);
      hubConnection.start();
    }
  }, [hubConnection]);

  return (
    <SessionContext.Provider value={{ session, refreshSession, login, signUp, logout }}>
        {children}
    </SessionContext.Provider>
  );
}
