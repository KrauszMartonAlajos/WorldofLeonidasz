const express = require("express");

const router = express.Router();

const userController = require("../controllers/userController");

const userAuth = require("../middlewares/userAuth");

router.get("/", [ userAuth.verifyToken ], userController.getUsers);

router.post("/register", userController.createUser);

router.post("/login", userController.loginUser);

module.exports = router;