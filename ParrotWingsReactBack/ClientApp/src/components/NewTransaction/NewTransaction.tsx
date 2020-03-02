import React, { useState, ChangeEvent, useContext, useEffect, SyntheticEvent } from 'react'
import { useLocation } from 'react-router-dom';
import { Form, Button, Dropdown, DropdownProps } from 'semantic-ui-react'
import { toast } from 'react-toastify';

import { ApiContext } from '../ApiProvider/ApiProvider';
import { SessionContext } from '../SessionProvider/SessionProvider';
import { toastResponseErrors } from '../../api/api';

export default function NewTransaction() {
  const api = useContext(ApiContext);
  const {refreshSession} = useContext(SessionContext);
  const query = new URLSearchParams(useLocation().search);    
  const [recipient, setRecipient] = useState(query.get('username') !== null ? query.get('username')! : '');
  const queryAmount = Number.parseInt(query.get('amount') !== null ? query.get('amount')! : '0');
  const [amount, setAmount] = useState(queryAmount);
  const [recipientOptions, setRecipientOptions] = useState(new Array<string>());

  const fetchRecipientOptions = async () => {
    try {
      const result = await api.users.getUsernameOptions();
      setRecipientOptions(result);
    } catch (ex) {
      toastResponseErrors(ex.response?.data);
    }
  }

  useEffect(() => {
    fetchRecipientOptions();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []) 

  const handleAmountChange = (e: ChangeEvent<HTMLInputElement>) => {
    const value = Number.parseInt(e.target.value);
    if (!Number.isNaN(value)) {
      setAmount(value);
    }
  }

  const handleSubmitClick = async () => {
    try {
      await api.transaction.newTransaction({recipient: recipient, amount: amount})
      refreshSession();
      toast.success('Transaction remited')
    } catch (ex) {
      toastResponseErrors(ex.response?.data);
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
      <Button primary type='submit' onClick={handleSubmitClick}>Submit</Button>
    </Form>
  )
}