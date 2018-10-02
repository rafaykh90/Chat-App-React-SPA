import React, { Component } from 'react';
import { NavMenu } from './NavMenu';

export class Layout extends Component {
  displayName = Layout.name

	render() {
		return <div>
			<NavMenu />
			<div className='container-fluid'>
				{this.props.children}
			</div>
		</div>;
  }
}
