import React, { useContext } from 'react'
import { Icon, Menu } from 'semantic-ui-react'
import { SessionContext } from '../SessionProvider/SessionProvider'

export default function UserBalance() {
  const {session} = useContext(SessionContext);

  return (
    <Menu.Menu position='right'>
      <Menu.Item header>
        <Icon name='user' />
        Username: {session ? session.userName : 'unknown'}
      </Menu.Item>
      <Menu.Item header>
        Balance: {session ? session.balance : 0} pw
      </Menu.Item>
    </Menu.Menu>
  )
}