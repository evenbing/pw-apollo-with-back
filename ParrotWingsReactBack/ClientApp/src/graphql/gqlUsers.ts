import gql from 'graphql-tag';

export const GET_USERNAME_OPTIONS = gql`
  query GetUsernameOptions {
    userNameOptions {
      userName
    }
  }
`;