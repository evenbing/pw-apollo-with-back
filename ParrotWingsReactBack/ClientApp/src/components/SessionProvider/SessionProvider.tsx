import React, { useEffect, useState, useContext, ReactNode } from 'react';
import { useQuery, useMutation } from '@apollo/react-hooks';
import { ApolloError } from 'apollo-boost';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr'

import { ISessionInfo, ILoginOptions, ISignUpOptions } from '../../models/backendModels';
import { toastResponseErrors } from '../../graphql/utils';
import { GET_SESSION_INFO, LOGIN, LOGOUT, SIGNUP } from '../../graphql/gqlSession';

export interface ISessionContext {
  session: ISessionInfo | null;
  refreshSession: () => Promise<void>;
  login: (creds: ILoginOptions) => Promise<void>;
  signUp: (creds: ISignUpOptions) => Promise<void>;
  logout: () => Promise<void>;
}

export const SessionContext = React.createContext<ISessionContext>({
  session: null,
  refreshSession: () => Promise.reject(),
  login: () => Promise.reject(),
  signUp: () => Promise.reject(),
  logout: () => Promise.reject(),
});

export default function SessionProvider({ children }: { children?: ReactNode}) {  
  const [hubConnection, setHubConnection] = useState<HubConnection | null>(null);
  const [session, setSession] = useState<ISessionInfo | null>(null);

  const { refetch: refetchSession } = useQuery(GET_SESSION_INFO, {
    fetchPolicy: 'network-only',
    notifyOnNetworkStatusChange: true,
    onCompleted: ({sessionInfo}) => {
      setSession(sessionInfo)
    },
    onError: () => setSession(null)
  });

  const [gqlLogin] = useMutation(LOGIN, { 
    onCompleted: ({login: sessionInfo}) => {
      setSession(sessionInfo);
      setHubConnection(new HubConnectionBuilder()
        .withUrl('/balance')
        .build());
    },
    onError: (error: ApolloError) => {
      throw error;
    }
  });

  const [gqlSignup] = useMutation(SIGNUP, { 
    onCompleted: ({signUp: sessionInfo}) => setSession(sessionInfo),
    onError: (error: ApolloError) => {
      throw error;
    }
  });

  const [gqlLogout] = useMutation(LOGOUT, { 
    onCompleted: () => setSession(null),
    onError: (error: ApolloError) => {
      setSession(null);
      toastResponseErrors(error);
    }
  }); 

  const updateBalance = (data: number) => {
    setSession(session === null ? null : {userName: session.userName, balance : data});
  }

  const refreshSession = async () => {
    await refetchSession();
  }

  const login = async (loginOptions: ILoginOptions) => {    
    await gqlLogin({ variables: { loginOptions } });
  };

  const signUp = async (signUpOptions: ISignUpOptions) => {
    await gqlSignup({ variables: { signUpOptions } });
  };

  const logout = async () => {
    await gqlLogout();
  };

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
