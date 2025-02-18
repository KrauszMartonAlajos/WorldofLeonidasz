const db = require("../database/dbContext");

class CharacterRepository {

    constructor(db)
    {
        this.Characters = db.characters;
    }

    async getAllCharacters(data) {
        return await this.Characters.findAll({
            where: {
                userId: data.ID,
            },
        });
    }

    async getAll() {
        return await this.Characters.findAll();
    }
}

module.exports = new CharacterRepository(db);