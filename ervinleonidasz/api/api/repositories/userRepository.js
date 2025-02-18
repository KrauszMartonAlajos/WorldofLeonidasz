const db = require("../database/dbContext");

class UserRepository
{
    constructor(db)
    {
        this.Users = db.users;
    }

    async createUser(user)
    {
        const newUser = await this.Users.build(user);

        await newUser.save();
        
        return newUser;
    }

    async getUsers()
    {
        return await this.Users.findAll();
    }

    async getUser(name)
    {
        return await this.Users.findOne
        (
            {
                where:
                {
                    name: name,
                }
            }
        )
    }


}

module.exports = new UserRepository(db);