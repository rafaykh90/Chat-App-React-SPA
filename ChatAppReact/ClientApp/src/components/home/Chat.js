import React, { Component } from 'react';
import * as moment from 'moment';

import { ChatService } from '../../services/ChatService';

export class Chat extends Component {
	displayName = Chat.name;
	msg;
	panel;

	_chatService;

	constructor(props) {
		super(props);
		this.state = {
			messages: [],
			currentMessage: ''
		};
	}

	componentDidMount() {
		let that = this;
		this._chatService = new ChatService(this.props.websocketService, (msg) => {
			this.handleOnSocket(that, msg);
		});

		this.handleOnInitialMessagesFetched = this.handleOnInitialMessagesFetched.bind(this);
		this.handlePanelRef = this.handlePanelRef.bind(this);
		//this.handleMessageRef = this.handleMessageRef.bind(this);
		this.handleMessageChange = this.handleMessageChange.bind(this);
		this.onSubmit = this.onSubmit.bind(this);

		this._chatService.fetchInitialMessages(this.handleOnInitialMessagesFetched);
	}

	handlePanelRef = (div) => {
		this.panel = div;
	};

	handleMessageRef = (input) => {
		this.msg = input;
	};

	handleOnInitialMessagesFetched(messages) {
		this.setState({
			messages: messages
		});

		this.scrollDown(this);
	}

	handleOnSocket(that, message) {
		let messages = that.state.messages;
		messages.push(message);
		that.setState({
			messages: messages,
			currentMessage: ''
		});
		that.scrollDown(that);
		that.focusField(that);
	}

	handleMessageChange(event) {
		this.setState({
			currentMessage: event.target.value
		});
	}

	onSubmit(event) {
		event.preventDefault();
		this.addMessage(this);
	}

	addMessage = (that) => {
		let currentMessage = that.state.currentMessage;
		if (currentMessage.length === 0) {
			return;
		}

		this._chatService.addMessage(currentMessage);
	}

	focusField(that) {
		that.msg.focus();
	}

	scrollDown(that) {
		let div = that.panel;
		div.scrollTop = div.scrollHeight;
	}

	render() {
		return <div className='panel panel-default'>
			<div className='panel-body panel-chat'
				ref={this.handlePanelRef}>
				<ul>
					{this.state.messages.map(message =>
						<li key={message.id}><strong>{message.sender} </strong>
							({moment(message.date).format('HH:mm:ss')})<br />
							{message.message}</li>
					)}
				</ul>
			</div>
			<div className='panel-footer'>
				<form className='form-inline' onSubmit={this.onSubmit}>
					<label className='sr-only' htmlFor='msg'></label>
					<div className='input-group col-md-12'>
						<input type='text'
							value={this.state.currentMessage}
							onChange={this.handleMessageChange}
							className='form-control'
							id='msg'
							placeholder='Your message'
							ref={this.handleMessageRef} />
						<button>Send</button >
					</div>
				</form>
			</div>
		</div>;
	}
}