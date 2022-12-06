//#region imports
const fs = require('fs')
const bodyParser = require('body-parser')
const jsonServer = require('json-server')
const jwt = require('jsonwebtoken')
const cors = require("cors");

//#endregion

//#region server settings
const server = jsonServer.create();
const router = jsonServer.router('./db/db.json');
const userdb = JSON.parse(fs.readFileSync('./db/loginUsers.json', 'UTF-8'));

server.use(bodyParser.json());
server.use(cors());
//#endregion

//#region JwtToken
const SECRET_KEY = 'qwertyuiopasdfghjklzxcvbnm';
const expiresIn = '1h';

function createToken(payload) {
    return jwt.sign(payload, SECRET_KEY, { expiresIn });
}

function verifyToken(token) {
    return jwt.verify(token, SECRET_KEY, (err, decode) => { if(err) throw 'invalid token' } )
}

function getInfoFromToken(token) {
    return jwt.decode(token);
}

function loginUser({ email, password }) {
    return userdb.users.findIndex(user => user.email === email && user.password === password) !== -1
}

function getUserClaims({email, password}) {
    const user = userdb.users.find(user => user.email === email && user.password === password);
    return { id: user.id, email: user.email, role: user.role };
}
//#endregion

//#region Controllers

//#region Identity
server.post('/Identity/Login', (req, res) => {
    const {email, password} = req.body;
    if (loginUser({email, password}) === false) {
        const status = 401;
        const message = 'Incorrect email or password';
        res.status(status).json({ message: message });
        return;
    }
    const claims = getUserClaims({ email, password });
    const accessToken = createToken(claims);

    res.status(200).json({ accessToken: accessToken });
})
//#endregion

//#region Middlewares

//#region Authorization
server.use(/^(?!\/Identity).*$/, (req, res, next) => {
    try {
        verifyToken(req.headers.authorization.split(' ')[1]);
        next();
    } catch (err) {
        const status = 401;
        const message = 'Error: accessToken is not valid';
        res.status(status).json({ message: message });
    }
})
//#endregion

//#region LimitAccess to all except EntityTwo and EntityThree
server.use(/^(?!\/\b(EntityTwo|EntityThree)\b).*$/, (req, res, next) => {
    let tokenInfo = getInfoFromToken(req.headers.authorization.split(' ')[1]);
    if (tokenInfo.role == "Admin") {
        next();
    } else {
        const status = 403;
        const message = 'Error: unauthorized access';
        res.status(status).json({ message: message });
    }
})
//#endregion

//#region Server start
server.use(router);
server.listen(5001, () => {
    console.log('server started');
})
//#endregion
