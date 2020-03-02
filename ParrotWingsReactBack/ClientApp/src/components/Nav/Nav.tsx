import React, { useContext, useState, useEffect } from 'react'
import { useHistory, useLocation } from 'react-router-dom';
import {Menu, Button, Icon} from 'semantic-ui-react'

import UserBalance from '../UserBalance/UserBalance';
import { NavRoute } from '../MainRouter/MainRouter'
import { SessionContext } from '../SessionProvider/SessionProvider';

export default function Nav() {
  const history = useHistory();
  const location = useLocation();
  const { logout } = useContext(SessionContext);
  const [activeNavItem, setActiveNavItem] = useState(location.pathname as NavRoute);  

  useEffect(() => {
    if (location.pathname !== activeNavItem) {
      setActiveNavItem(location.pathname as NavRoute);
    }
  });

  function handleMenuClick(route: NavRoute) {    
    history.push(route)    
  }

  return (
    <Menu>
      <Menu.Item header>
        <Icon name='money bill alternate' size='big'/>
        Parrot Wings
      </Menu.Item>
      <Menu.Item 
        name='Home'
        active={activeNavItem === NavRoute.Home}
        onClick={() => handleMenuClick(NavRoute.Home)}
      />
      <Menu.Item 
        name='Transactions'
        active={activeNavItem === NavRoute.TransHistory}
        onClick={() => handleMenuClick(NavRoute.TransHistory)}
      />
      <Menu.Item 
        name='New transaction'
        active={activeNavItem === NavRoute.TransNew}
        onClick={() => handleMenuClick(NavRoute.TransNew)}
      />
      <UserBalance />
      <Menu.Item>
        <Button secondary as='a' onClick={logout}>
          Logout
        </Button>
      </Menu.Item>
    </Menu>
  );
}