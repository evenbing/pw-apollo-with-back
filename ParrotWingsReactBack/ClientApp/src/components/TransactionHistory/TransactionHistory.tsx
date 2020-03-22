import React, { useState } from 'react'
import { Table, Button, Icon, Pagination } from 'semantic-ui-react'
import { useQuery } from '@apollo/react-hooks';
import { ApolloError } from 'apollo-boost';

import { useHistory } from 'react-router';
import { NavRoute } from '../MainRouter/MainRouter';
import { ITransactionInfo } from '../../models/backendModels';
import { toastResponseErrors } from '../../graphql/utils';
import { GET_CURRENT_USER_TRANSACTIONS } from '../../graphql/gqlTransaction';

const ITEMS_PER_PAGE = 10;
const DEFAULT_PAGE = 1;

export default function TransactionHistory() {
  const history = useHistory();    
  const [ transactions, setTransactions ] = useState(new Array<ITransactionInfo>());
  const [ totalPages, setTotalPages ] = useState(0);
  const [ currentPage, setCurrentPage ] = useState(DEFAULT_PAGE);
  
  useQuery(GET_CURRENT_USER_TRANSACTIONS, {
    variables: {
      offset: ((currentPage || DEFAULT_PAGE) - 1) * ITEMS_PER_PAGE,
      limit: ITEMS_PER_PAGE
    },
    fetchPolicy: 'cache-and-network',
    notifyOnNetworkStatusChange: true,
    onCompleted: ({transactionInfos, totalCount}) => {
      setTransactions(transactionInfos);
      setTotalPages(Math.ceil(totalCount.count / ITEMS_PER_PAGE));
    },
    onError: (error: ApolloError) => toastResponseErrors(error)
  });

  const handleCopyClick = (transaction: ITransactionInfo) => {
    const absAmount = Math.abs(transaction.amount);
    history.push(`${NavRoute.TransNew}?username=${transaction.correspondentName}&amount=${absAmount}`)
  }

  const handleChangePage = (newPage: string | number | undefined) => {
    const pageNumber = Number(newPage);
    if (isNaN(pageNumber)) {
      return;
    }
    setCurrentPage(pageNumber);    
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
        <Table.Row style={{minHeight: '61px'}} key={`${transaction.date}_${transaction.resultBalance}`}>
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

      <Table.Footer>
        <Table.Row>
          <Table.HeaderCell colSpan="8">
            <Pagination
              totalPages={totalPages}
              activePage={currentPage}
              onPageChange={(_, data) => handleChangePage(data.activePage)}
            />
          </Table.HeaderCell>
        </Table.Row>
      </Table.Footer>
    </Table>
  )
}