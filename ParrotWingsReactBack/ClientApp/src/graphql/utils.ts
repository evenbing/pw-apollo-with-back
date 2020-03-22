import { ApolloError } from 'apollo-boost';
import { toast } from 'react-toastify';

const defaultErrorMessage = 'Internal server error';

export function toastResponseErrors(error: ApolloError) {
  if (!error || !error.graphQLErrors) {
    toast.error(defaultErrorMessage);
    return;
  }
 
  error.graphQLErrors.forEach((error) => toast.error(error.message));
}