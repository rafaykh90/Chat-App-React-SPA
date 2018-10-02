import React, { Component } from 'react';

import { UsersService } from '../../services/UsersService';

export class Users extends Component {
	displayName = Users.name;
	userService = null;

	constructor(props) {
		super(props);
		this.state = {
			users: []
		};
	}

	handleOnSocket = (user) => {
		let users = this.state.users;
		users.push(user);
		this.setState({
			users: users
		});
	};

	handleOnSocketClose = (user) => {
		let users = this.state.users;
		var index = users.indexOf(user);
		users.splice(index, 1);
		this.setState({
			users: users
		});
	}

	componentDidMount() {
		this.userService = new UsersService(this.props.websocketService, this.handleOnSocket, this.handleOnSocketClose);
		this.userService.fetchLogedOnUsers(this.handleOnLogedOnUserFetched);
	}

	//private ;

	handleOnLogedOnUserFetched = (users) => {
		this.setState({
			users: users
		});
	};

	render() {
		return <div className='panel panel-default'>
			<div className='panel-body'>
				<h3>Users online:</h3>
				<ul className='chat-users'>

					{this.state.users.map(user =>
						<li key={user.id}>{user.name}</li>
					)}
				</ul>
			</div>
		</div>;
	}
}