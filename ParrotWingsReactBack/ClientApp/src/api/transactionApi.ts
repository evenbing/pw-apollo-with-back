import { AxiosInstance } from 'axios';

import { ITransactionInfo, INewTransaction } from '../models/backendModels';

const TRANSACTION_LIST_URL = '/api/transactions/getAllForCurrentUser';
const NEW_TRANSACTION_URL = '/api/transactions/create';

export default class TransactionsApi {
  constructor(private readonly client: AxiosInstance) {}
   
  async newTransaction(newTransaction: INewTransaction): Promise<void> {
    await this.client.post(NEW_TRANSACTION_URL, newTransaction);
  }

  async getTransactions(): Promise<Array<ITransactionInfo>> {
    const { data } = await this.client.get(TRANSACTION_LIST_URL);
    return data;
  }
}