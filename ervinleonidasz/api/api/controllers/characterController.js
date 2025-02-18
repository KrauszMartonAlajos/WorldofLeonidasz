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