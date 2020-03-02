import React, { ReactElement } from 'react'
import { Container } from 'semantic-ui-react'

import Nav from '../Nav/Nav'

export default function Layout({children} : {children: ReactElement}) {
  return (
    <>
      <Nav/>
      <Container text>
        {children}
      </Container>
    </>
  )
}