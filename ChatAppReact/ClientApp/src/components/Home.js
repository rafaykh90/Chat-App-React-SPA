import React, { Component } from 'react';
import ChatWebsocketService from '../services/WebsocketService';
import { Users } from './home/Users';
import { Chat } from './home/Chat';

export class Home extends Component {
	displayName = Home.name;
	websocketService = null;

	constructor(props) {
		super(props);
		this.websocketService = new ChatWebsocketService();
	}

	render() {
		return <div className='row'>
			<div className='col-sm-3'>
				<Users websocketService={this.websocketService} />
			</div>
			<div className='col-sm-8'>
				<Chat websocketService={this.websocketService} />
			</div>
		</div>;
	}
}