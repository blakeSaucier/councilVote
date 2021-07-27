import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import CreateMeasure from './components/CreateMeasure';
import Measure from "./components/Measure";
import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route exact path='/create' component={CreateMeasure} />
        <Route exact path='/measure/:id' component={Measure} />
      </Layout>
    );
  }
}
