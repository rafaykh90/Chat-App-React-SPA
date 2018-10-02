import React, { Component } from 'react';
import axios from 'axios';

export class Register extends Component {
	displayName = Register.name;

	constructor(props) {
		super(props);
		this.state = {
			name: ''
		};

		this.onSubmit = this.onSubmit.bind(this);
	}

	onChange = (event) => {
		this.setState({
			name: event.target.value
		});
	};

	onSubmit(event) {
		event.preventDefault();
		axios.get('/api/users/exists?name=' + this.state.name)
			.then(this.onSuccess)
			.catch(function (error) {
				console.log(error);
				//Update UI with appropriate labels...
				alert("Username already exists");
			});
	}

	onSuccess = (response) => {
		window.Username = this.state.name;
		this.props.history.push('/chatScreen');
	};

	render() {
		return <div className='panel panel-default'>
			<div className='panel-body'>

				<form className='register-form'>
					<label>
						Name:
					</label>
					<input type='text'
						value={this.state.name}
						onChange={this.onChange}
						className='form-control'
						id='name'
						placeholder='Your Name' />
					<button onClick={this.onSubmit}>Send</button >
				</form>
			</div>
		</div>;
	}
}