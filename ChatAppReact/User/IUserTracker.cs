using System;
using System.Collections.Generic;
using ChatAppReact.Models;

namespace ChatAppReact.User
{
    public interface IUserTracker
    {
        IEnumerable<UserDetails> UsersOnline();
        void AddUser(string sid, string name);
        void RemoveUser(string sid);
    }
}
