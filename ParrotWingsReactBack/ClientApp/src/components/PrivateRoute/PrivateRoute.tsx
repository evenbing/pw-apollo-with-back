import React, { ReactNode, useContext } from 'react'
import { Route, Redirect, RouteProps } from 'react-router-dom';
import { SessionContext } from '../SessionProvider/SessionProvider';
import { NavRoute } from '../MainRouter/MainRouter';

interface IPrivateRouteProps extends RouteProps {
  children: ReactNode;
}
  
export default function PrivateRoute({ children, ...rest } : IPrivateRouteProps) {
  const {session} = useContext(SessionContext);
  
  return (
    <Route
      {...rest}
      render={({ location }) =>
        session !== null ? (
          children
        ) : (
          <Redirect
            to={{
              pathname: NavRoute.Login,
              state: { from: location.pathname }
            }}
          />
        )
      }
    />
  );
}