import { toast } from 'react-toastify';

import SessionApi from './sessionApi';
import TransactionsApi from './transactionApi';
import UsersApi from './usersApi';

const defaultErrorMessage = 'Internal server error';

interface IResponseData {
  errorMessage?: string;
  errors?: any;
}

export function toastResponseErrors(response: IResponseData) {
  if (!response) {
    toast.error(defaultErrorMessage);
    return;
  }
  if (response.errors !== undefined) {
    for (const error in response.errors) {
      if (response.errors.hasOwnProperty(error)) {
        for (const message of (response.errors[error] as Array<string>)) {
          toast.error(message);
        }
      }
    }
    return;
  }
  if (response.errorMessage !== undefined) {
    toast.error(response.errorMessage);
    return;
  }
  toast.error(defaultErrorMessage);
}

export default class Api {    
  public readonly session: SessionApi;
  public readonly transaction: TransactionsApi;
  public readonly users: UsersApi;  

  constructor() {
    this.session = new SessionApi(null);
    this.transaction = new TransactionsApi(null);
    this.users = new UsersApi(null);
  }  
}
