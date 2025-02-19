const characterRepository  = require("../repositories/characterRepository");

class CharacterService
{
    async getAllCharacters(ID)
    {
        return await characterRepository.getAllCharacters(ID);
    }
    
    async getAll()
    {
        return await characterRepository.getAll();
    }

    async createCharacter(data)
    {
        return await characterRepository.createCharacter(data);
    }
}    
module.exports = new CharacterService();