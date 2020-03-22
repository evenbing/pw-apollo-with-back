import React, { useState, ChangeEvent, useContext, useEffect, SyntheticEvent } from 'react'
import { useLocation } from 'react-router-dom';
import { useQuery, useMutation } from '@apollo/react-hooks';
import { ApolloError } from 'apollo-boost';
import { Form, Button, Dropdown, DropdownProps } from 'semantic-ui-react'
import { toast } from 'react-toastify';

import { SessionContext } from '../SessionProvider/SessionProvider';
//import { toastResponseErrors } from '../../graphql/api';
import { GET_USERNAME_OPTIONS } from '../../graphql/gqlUsers';
import { NEW_TRANSACTION } from '../../graphql/gqlTransaction';
import { IUserNameOption } from '../../models/backendModels';

export default function NewTransaction() {  
  const {refreshSession} = useContext(SessionContext);
  const query = new URLSearchParams(useLocation().search);    
  const [recipient, setRecipient] = useState(query.get('username') !== null ? query.get('username')! : '');
  const queryAmount = Number.parseInt(query.get('amount') !== null ? query.get('amount')! : '0');
  const [amount, setAmount] = useState(queryAmount);
  const [recipientOptions, setRecipientOptions] = useState(new Array<string>());

  useQuery(GET_USERNAME_OPTIONS, {
    fetchPolicy: 'cache-and-network',
    onCompleted: ({userNameOptions}) => setRecipientOptions(userNameOptions.map((x: IUserNameOption) => x.userName)),
    //onError: () => toastResponseErrors(ex.response?.data);
  });

  const [createTransaction] = useMutation(NEW_TRANSACTION, { 
    onCompleted: async () => {
      await refreshSession();
      toast.success('Transaction remited');
    },
    //onError: () => toastResponseErrors(ex.response?.data);
  });

  const handleAmountChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = Number.parseInt(e.target.value);
    if (!Number.isNaN(value)) {
      setAmount(value);
    }
  }

  return (
    <Form size='big'>
      <Form.Field>
        <label>Recipient</label>
        <Dropdown
          placeholder='Recipient'
          fluid
          search
          selection
          options={recipientOptions.map((value) => ({key: value, text: value, value}))}
          value={recipient}
          onChange={(e: SyntheticEvent<HTMLElement, Event>, data: DropdownProps) => setRecipient(data.value as string)}
        />
      </Form.Field>
      <Form.Field>
        <label>Amount</label>
        <input 
          placeholder='Amount' 
          value={amount}
          type='number'
          min='1'
          step='1'
          onChange={handleAmountChange}
        />
      </Form.Field>
      <Button primary type='submit' onClick={() => createTransaction({ variables: { newTransaction: {recipient, amount} } })}>Submit</Button>
    </Form>
  )
}