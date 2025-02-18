const express = require("express");

const app = express();

app.use(express.json());

app.use(express.urlencoded({extended: true}));

const userRoutes = require("./api/routes/userRoutes");
const characterRoutes = require("./api/routes/characterRoutes");

// ROUTES

app.use("/users", userRoutes);
app.use("/characters", characterRoutes);

module.exports = app;