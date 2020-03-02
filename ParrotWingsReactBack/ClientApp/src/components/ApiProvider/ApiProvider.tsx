import React, { ReactNode } from 'react';
import Api from '../../api/api';

const api = new Api();

export const ApiContext = React.createContext(api);

export default function ApiProvider({ children }: { children?: ReactNode }) {  
  return (
    <ApiContext.Provider value={api}>
        {children}
    </ApiContext.Provider>
  );
}
