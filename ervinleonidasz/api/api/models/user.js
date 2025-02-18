const { Model } = require("sequelize");

module.exports = (sequelize, DataTypes) =>
{
    class User extends Model {};
    User.init
    (
        {
            ID:
            {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false,
            },

            name:
            {
                type: DataTypes.STRING,
                allowNull: false,
            },

            password:
            {
                type: DataTypes.STRING,
                allowNull: false,
            }
        },

        {
            sequelize,
            modelName: "User",
            timestamps: false,
        }
    )

    return User;
}