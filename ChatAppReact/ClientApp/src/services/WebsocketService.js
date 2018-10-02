import * as signalR from "@aspnet/signalr";

class ChatWebsocketService {
	_connection = null;

	constructor() {
		let url = `https://${document.location.host}/chat`;
		this._connection = new signalR.HubConnectionBuilder()
			.withUrl(url)
			.build();
		this._connection.start().then(() => {
			this._connection.invoke("UserConnected", window.Username);
		}).catch(err => document.write(err));
	}

	registerMessageAdded(messageAdded) {
		// get other client chat message from the server
		this._connection.on('MessageAdded', (message) => {
			messageAdded(message);
		});
	}
	sendMessage(message) {
		// send the chat message to the server
		this._connection.invoke('AddMessage', window.Username, message);
	}

	registerUserLoggedOn(userLoggedOn) {
		// get new user from the server
		this._connection.on('UserLoggedOn', (user) => {
			userLoggedOn(user);
		});
	}

	registerUserLoggedOff(userLoggedOff) {
		// get logged off user from the server
		this._connection.on('UserLoggedOff', (user) => {
			userLoggedOff(user);
			this._connection.invoke('AddMessage', user.name, 'LEFT');
		});
	}
}
export default ChatWebsocketService;