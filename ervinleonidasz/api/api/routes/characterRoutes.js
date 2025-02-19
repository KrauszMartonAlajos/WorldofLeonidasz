const express = require('express');

const router = express.Router();

const characterController = require("../controllers/characterController");

router.get("/",characterController.getAll);

router.get("/get-all-characters-by-user",characterController.getAllCharacters);

router.post("/create",characterController.createCharacter);

module.exports = router;