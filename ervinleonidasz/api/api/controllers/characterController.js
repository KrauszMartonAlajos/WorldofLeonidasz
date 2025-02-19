const user = require("../models/user");
const characterService = require("../services/characterService");

exports.getAllCharacters = async (req, res, next) =>
{
    const ID = req.body;

    res.status(200).send(await characterService.getAllCharacters(ID));
}

exports.getAll = async (req, res, next) =>
{
    res.status(200).send(await characterService.getAll());
}

exports.createCharacter = async (req, res, next) =>
{
    const {ID,name,lvl,utokepesseg,type,userId} = req.body;

    const newCharacter = 
    {
        ID: ID,
        name: name,
        lvl: lvl,
        utokepesseg: utokepesseg,
        type: type,
        userId: userId,
    }

    const result = await characterService.createCharacter(newCharacter);

    if(result)
    {
        res.status(201).json(result);
    }
    else
    {
        res.status(400).send("Fail");
    }
}