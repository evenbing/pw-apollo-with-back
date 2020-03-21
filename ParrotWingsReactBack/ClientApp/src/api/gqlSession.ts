import gql from 'graphql-tag';

export const GET_SESSION_INFO = gql`
  query GetSessionInfo {
    sessionInfo {
      userName
      balance
    }
  }
`;

export const LOGIN = gql`
  mutation Login($loginOptions: LoginOptionsInput) {
    sessionInfo(loginOptions: $loginOptions) {
      userName
      balance
    }
  }
`;

export const SIGNUP = gql`
  mutation SignUp($signUpOptions: SignUpOptionsInput) {
    sessionInfo(signUpOptions: $signUpOptions) {
      userName
      balance
    }
  }
`;

export const LOGOUT = gql`
  mutation Logout {
    logout {
      id
    }
  }
`;