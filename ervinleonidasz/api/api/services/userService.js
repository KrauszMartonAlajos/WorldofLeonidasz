const userRepository  = require("../repositories/userRepository");

class UserService
{
    async createUser(user)
    {
        return await userRepository.createUser(user);
    }

    async getUsers()
    {
        return await userRepository.getUsers();
    }

    async getUser(name)
    {
        return await userRepository.getUser(name);
    }

}

module.exports = new UserService();