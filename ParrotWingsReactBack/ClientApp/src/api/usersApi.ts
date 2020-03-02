import { AxiosInstance } from 'axios';

const USERNAME_OPTIONS_URL = '/api/users/getUsernameOptions';

export default class UsersApi {  
  constructor(private readonly client: AxiosInstance) {}

  async getUsernameOptions(): Promise<Array<string>> {
    const { data } = await this.client.get(USERNAME_OPTIONS_URL);    
    return data;
  }
}