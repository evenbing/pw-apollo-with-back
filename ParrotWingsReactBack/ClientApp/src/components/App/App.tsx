import React, {Suspense} from 'react';
import { ToastContainer, toast, Slide } from 'react-toastify';

import MainRouter from '../MainRouter/MainRouter';
import ApiProvider from '../ApiProvider/ApiProvider';
import SessionProvider from '../SessionProvider/SessionProvider';

export default function App() {
  return (
    <Suspense fallback = 'loading...'>
      <ApiProvider>
        <SessionProvider>
          <ToastContainer 
            position={toast.POSITION.TOP_RIGHT} 
            autoClose={2000} 
            transition={Slide}
            hideProgressBar={true}
          />
          <MainRouter />
        </SessionProvider>
      </ApiProvider>      
    </Suspense>    
  );
}