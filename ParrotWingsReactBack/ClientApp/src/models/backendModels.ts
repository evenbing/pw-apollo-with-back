export interface ITransactionInfo {  
  date: string;
  correspondentName: string;
  amount: number;
  resultBalance: number;
}

export interface INewTransaction {
  recipient: string;
  amount: number;  
}

export interface ISessionInfo {
  userName: string;
  balance: number;
}

export interface ILoginOptions {
  email: string;
  password: string;  
}

export interface ISignUpOptions {
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
}

export interface IUserInfo {
  email: string;
  userName: string;
  password: string;
  balance: number;
}

