const userService = require("../services/userService");

const bcrypt = require("bcrypt");

const salt = 10;

const jwt = require("jsonwebtoken");

exports.getUsers = async (req, res, next) =>
{
    res.status(200).send(await userService.getUsers());
}

exports.createUser = async (req, res, next) =>
{
    const { ID, name, password } = req.body;

    const newUser =
    {
        ID: ID,
        name: name,
        password: await bcrypt.hash(password, salt),
    }

    const result = await userService.createUser(newUser);

    if(result)
    {
        res.status(201).json(result);
    }
    else
    {
        res.status(400).send("Fail");
    }
}

exports.loginUser = async (req, res, next) =>
{
    const { name, password } = req.body;

    const user = await userService.getUser(name);

    if(await bcrypt.compare(password, user.password))
    {
        const token = jwt.sign({ user }, process.env.JWT_KEY, { expiresIn: "30m" });

        res.status(200).json(token);
    }
    else
    {
        res.status(400).send("Wrong password");
    }
}

