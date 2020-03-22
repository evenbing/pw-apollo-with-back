import React, { useEffect, useContext, useState } from 'react'
import { Table, Button, Icon } from 'semantic-ui-react'
import { useQuery } from '@apollo/react-hooks';
import { ApolloError } from 'apollo-boost';

import { useHistory } from 'react-router';
import { NavRoute } from '../MainRouter/MainRouter';
import { ITransactionInfo } from '../../models/backendModels';
//import { toastResponseErrors } from '../../graphql/api';
import { GET_ALL_FOR_CURRENT_USER } from '../../graphql/gqlTransaction';

export default function TransactionHistory() {
  const history = useHistory();    
  const [ transactions, setTransactions ] = useState(new Array<ITransactionInfo>());
  
  useQuery(GET_ALL_FOR_CURRENT_USER, {
    fetchPolicy: 'cache-and-network',
    onCompleted: ({transactionInfos}) => setTransactions(transactionInfos),
    //onError: () => toastResponseErrors(ex.response?.data);
  });

  const handleCopyClick = (transaction: ITransactionInfo) => {
    const absAmount = Math.abs(transaction.amount);    
    history.push(`${NavRoute.TransNew}?username=${transaction.correspondentName}&amount=${absAmount}`)
  }

  return (
    <Table celled>
      <Table.Header>
      <Table.Row>
        <Table.HeaderCell>Date</Table.HeaderCell>
        <Table.HeaderCell>Correspondent name</Table.HeaderCell>
        <Table.HeaderCell>Amount</Table.HeaderCell>
        <Table.HeaderCell>Balance</Table.HeaderCell>
        <Table.HeaderCell />
      </Table.Row>
      </Table.Header>

      
      <Table.Body>
      {transactions.map((transaction: ITransactionInfo) => 
        <Table.Row key={`${transaction.date}_${transaction.resultBalance}`}>
          <Table.Cell>{transaction.date}</Table.Cell>
          <Table.Cell>{transaction.correspondentName}</Table.Cell>
          <Table.Cell>{transaction.amount}</Table.Cell>
          <Table.Cell>{transaction.resultBalance}</Table.Cell>
          <Table.Cell>
            {transaction.amount < 0 && 
              <Button icon labelPosition='left' onClick={() => handleCopyClick(transaction)}>
                <Icon name='copy outline' />
                Copy
              </Button>}
          </Table.Cell>
        </Table.Row>)
      }
      </Table.Body>
    </Table>
  )
}