var CDSSession = {}

CDSSession.getSessionStorage = function (key) {
    return sessionStorage.getItem(key);
}

CDSSession.setSessionStorage = function (key, value) {
    sessionStorage.setItem(key, value);
}