import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Home } from './components/Home';
import { Register } from './components/Register';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
		<Layout>
			<Route exact path='/' component={Register} />
			<Route path='/chatScreen' component={Home} />
      </Layout>
    );
  }
}
