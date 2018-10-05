import 'isomorphic-fetch';

export class UsersService {
	_userLoggedOn = null;
	_userLoggedOff = null;
	_websocketService = null;

	constructor(websocketService, socketCallback, socketCloseCallback) {
		this._userLoggedOn = socketCallback;
		this._userLoggedOff = socketCloseCallback;
		this._websocketService = websocketService;

		// Receive Chat Users from server
		websocketService.registerUserLoggedOn((user) => {
			this._userLoggedOn(user);
		});

		websocketService.registerUserLoggedOff((user) => {
			this._userLoggedOff(user);
		});
	}

	fetchLogedOnUsers = (fetchUsersCallback) => {
		fetch('api/Users/LoggedOnUsers')
			.then(response => response.json())
			.then(data => {
				fetchUsersCallback(data);
			});
	}
}