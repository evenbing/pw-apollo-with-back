import React, {Suspense} from 'react';
import { ToastContainer, toast, Slide } from 'react-toastify';
import ApolloClient from 'apollo-boost';
import { ApolloProvider } from '@apollo/react-hooks';

import MainRouter from '../MainRouter/MainRouter';
import SessionProvider from '../SessionProvider/SessionProvider';

const client = new ApolloClient();

export default function App() {
  return (
    <Suspense fallback = 'loading...'>
      <ApolloProvider client={client}>
        <SessionProvider>
          <ToastContainer 
            position={toast.POSITION.TOP_RIGHT} 
            autoClose={2000} 
            transition={Slide}
            hideProgressBar={true}
          />
          <MainRouter />
        </SessionProvider>
      </ApolloProvider>
    </Suspense>    
  );
}