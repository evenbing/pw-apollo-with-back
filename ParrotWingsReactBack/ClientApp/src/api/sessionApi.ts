import { AxiosInstance } from 'axios';

import { ILoginOptions, ISignUpOptions, ISessionInfo } from '../models/backendModels';

const LOGIN_URL = '/api/session/login';
const SINGUP_URL = '/api/session/signUp';
const LOGOUT_URL = '/api/session/logout';
const SESSION_INFO_URL = '/api/session/getSessionInfo';

export default class SessionApi {  
  constructor(private readonly client: AxiosInstance) {}

  async login(options: ILoginOptions): Promise<ISessionInfo> {
    const { data } = await this.client.post(LOGIN_URL, options);        
    return { ...data };
  }

  async signUp(options: ISignUpOptions): Promise<ISessionInfo> {
    const { data } = await this.client.post(SINGUP_URL, options);    
    return { ...data };
  }

  async logout(): Promise<void> {
    await this.client.post(LOGOUT_URL);
  }

  async getSessionInfo(): Promise<ISessionInfo> {
    const { data } = await this.client.get(SESSION_INFO_URL);    
    return { ...data };
  }
}