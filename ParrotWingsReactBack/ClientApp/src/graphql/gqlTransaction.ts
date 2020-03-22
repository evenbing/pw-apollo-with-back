import gql from 'graphql-tag';

export const GET_ALL_FOR_CURRENT_USER = gql`
  query GetAllForCurrentUser {
    transactionInfos {
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