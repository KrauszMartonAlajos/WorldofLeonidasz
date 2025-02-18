const jwt = require("jsonwebtoken");

exports.verifyToken = (req, res, next) =>
{
    var token = req.headers["authorization"]?.split(" ")[1]; // Bearer <token>

    if(!token)
    {
        res.status(403).send("Access denied");

        return;
    }

    jwt.verify(token, process.env.JWT_KEY, function(error, decoded)
    {
        if(!decoded)
        {
            res.status(400).send("Invalid token");

            return;
        }
    });

    next();
}