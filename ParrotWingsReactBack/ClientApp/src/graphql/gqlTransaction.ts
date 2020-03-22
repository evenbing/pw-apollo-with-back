import gql from 'graphql-tag';

export const GET_CURRENT_USER_TRANSACTIONS = gql`
  query GetCurrentUserTransactions($offset: Int!, $limit: Int!) {
    totalCount {
      count
    }
    transactionInfos(offset: $offset, limit: $limit) {
      date
      correspondentName
      amount
      resultBalance
    }
  }
`;

export const NEW_TRANSACTION = gql`
  mutation NewTransaction($newTransaction: NewTransactionInput!) {
    createTransaction(newTransaction: $newTransaction) {
      amount
    }
  }
`;