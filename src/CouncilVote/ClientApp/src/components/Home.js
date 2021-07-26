import React, { Component } from 'react';
import { Jumbotron, Button, Nav, NavLink, NavItem } from 'reactstrap';
import { Link } from 'react-router-dom';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <Jumbotron>
          <h1>Coucil Vote</h1>
          <p className="lead">This is a simple app to help strata councils vote on measures.</p>
          <Nav>
            <NavItem>
              <NavLink tag={Link} to="/create">Create New</NavLink>
            </NavItem>
          </Nav>
        </Jumbotron>
      </div>
    );
  }
}
