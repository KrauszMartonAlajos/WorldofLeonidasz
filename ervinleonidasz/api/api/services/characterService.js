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
}    
module.exports = new CharacterService();