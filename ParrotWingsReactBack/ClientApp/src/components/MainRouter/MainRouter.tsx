import React from 'react'
import { BrowserRouter as Router, Switch, Route } from "react-router-dom";

import Layout from '../Layout/Layout';
import LoginForm from '../LoginForm/LoginForm';
import PrivateRoute from '../PrivateRoute/PrivateRoute';
import Home from '../Home/Home';
import TransactionHistory from '../TransactionHistory/TransactionHistory';
import NewTransaction from '../NewTransaction/NewTransaction';
import NotFound from '../NotFound/NotFound';
import SignUpForm from '../SignUpForm/SignUpForm';

export enum NavRoute {
  Login = '/login',
  SignUp = '/signUp',
  Home = '/',
  TransHistory = '/transaction/history',
  TransNew = '/transaction/new',
  NotFound = '*'
}

export default function MainRouter() {
  return (
    <Router>
      <Switch>
        <Route path={NavRoute.Login}>
          <LoginForm/>
        </Route>
        <Route path={NavRoute.SignUp}>
          <SignUpForm/>
        </Route>
        <PrivateRoute exact path={NavRoute.Home}>
          <Layout>
            <Home/>
          </Layout>
        </PrivateRoute>
        <PrivateRoute path={NavRoute.TransHistory}>
          <Layout>
            <TransactionHistory/>
          </Layout>
        </PrivateRoute>
        <PrivateRoute path={NavRoute.TransNew}>
          <Layout>
            <NewTransaction/>
          </Layout>
        </PrivateRoute>
        <PrivateRoute path={NavRoute.NotFound}>
          <Layout>
            <NotFound/>
          </Layout>
        </PrivateRoute>
      </Switch>
    </Router>
  )
}