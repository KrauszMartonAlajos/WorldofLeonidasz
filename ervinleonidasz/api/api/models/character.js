const { Model } = require("sequelize");



module.exports = (sequelize, DataTypes) =>
{
    class Character extends Model {};
    Character.init
    (
        {
            id: {
                type: DataTypes.INTEGER,
                primaryKey: true,
                autoIncrement: true,
                allowNull: false
            },
            name: {
                type: DataTypes.STRING,
                allowNull: false
            },
            lvl: {
                type: DataTypes.INTEGER,
                allowNull: false
            },
            utokepesseg: {
                type: DataTypes.INTEGER,
                allowNull: false
            },
            type: {
                type: DataTypes.INTEGER,
                allowNull: false
            }
        },
        {
            sequelize,
            modelName: "Character",
            timestamps: false,
        }
    )
    return Character;
}