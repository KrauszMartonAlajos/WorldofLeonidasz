module.exports = (sequelize, DataTypes) => {
    const User = require("./user")(sequelize, DataTypes);
    const Character = require("./character")(sequelize, DataTypes);

    User.hasMany(Character, {
        foreignKey: "userId",
    });

    return { User, Character };
}