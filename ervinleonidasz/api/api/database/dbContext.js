const { Sequelize, DataTypes } = require("sequelize");


const sequelize = new Sequelize
(
    process.env.DB_NAME,
    process.env.DB_USERNAME,
    process.env.DB_PASSWORD,

    {
        host: process.env.DB_HOST,
        dialect: process.env.DB_DIALECT,
        logging: false,
    }
)

try
{
    sequelize.authenticate();

    console.log("Database Connection Successful");
}
catch(error)
{
    console.log(error.message);
}

const db = {};

db.Sequelize = Sequelize;

db.sequelize = sequelize;

const{User, Character} = require("../models")(sequelize, DataTypes);

db.users = User;
db.characters = Character;

db.sequelize.sync({ alter: true });

module.exports = db;