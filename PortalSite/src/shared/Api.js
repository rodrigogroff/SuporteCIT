
export const ApiLocation = {
    //api_host: 'http://192.168.254.42',
    //api_port: '18523',
    api_host: 'http://localhost',
    api_port: '18524',
    api_portal: '/api/v1/portal/',
}

export class Api {
    isAuthenticated = () => localStorage.getItem('token');

    loggedUserName = () => localStorage.getItem('user_name');
    loggedUserSocialID = () => localStorage.getItem('socialID');

    cleanLogin() {
        localStorage.setItem('token', null)
    }

    loginOk = (token, nome) => {
        localStorage.setItem('token', token)
        localStorage.setItem('user_name', nome)
    }

    ping = () => {
        return new Promise((resolve, reject) => {
            fetch(ApiLocation.api_host + ':' + ApiLocation.api_port + ApiLocation.api_portal + 'ping',
                {
                    method: 'GET', headers:
                    {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + localStorage.getItem('token'),
                        'Sessao': localStorage.getItem('sessao'),
                    }
                })
                .then((res) => {
                    if (res.status === 401)
                        reject({ ok: false })
                    else
                        resolve({ ok: true })
                })
                .catch(() => {
                    reject({ ok: false })
                });
        })
    }

    getTokenPortal = (location, parameters) => {
        return new Promise((resolve, reject) => {
            var _params = '';

            if (parameters !== null)
                _params = '?' + parameters;

            fetch(ApiLocation.api_host + ':' + ApiLocation.api_port + ApiLocation.api_portal + location + _params,
                {
                    method: 'GET', headers:
                    {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + localStorage.getItem('token'),
                    }
                })
                .then((res) => {
                    if (res.status === 401) {
                        reject({
                            ok: false,
                            unauthorized: true
                        })
                    }
                    else if (res.status === 400) {
                        reject({
                            ok: false,
                            msg: 'Ops. Houve um erro e não pudemos concluir sua transação'
                        })
                    }
                    else if (res.ok === true) {
                        res.json().then((data) => {
                            resolve({
                                ok: true,
                                payload: data,
                            })
                        })
                    }
                    else res.json().then((data) => {
                        var jData = JSON.parse(data.value)

                        resolve({
                            ok: false,
                            msg: jData.message,
                        })
                    });
                })
                .catch((errorMsg) => {
                    resolve({
                        ok: false,
                        msg: errorMsg.toString(),
                    })
                });
        })
    }

    postTokenPortal = (location, data) => {
        return new Promise((resolve, reject) => {
            fetch(ApiLocation.api_host + ':' + ApiLocation.api_port +
                ApiLocation.api_portal + location,
                {
                    method: 'POST', headers:
                    {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + localStorage.getItem('token'),
                    },
                    body: data
                })
                .then((res) => {
                    if (res.status === 401) {
                        reject({
                            ok: false,
                            unauthorized: true
                        })
                    }
                    else if (res.status === 400) {
                        reject({
                            ok: false,
                            msg: 'Ops. Houve um erro e não pudemos concluir sua transação'
                        })
                    }
                    else if (res.ok === true) {
                        res.json().then((data) => {
                            resolve({
                                ok: true,
                                payload: data,
                            })
                        })
                    }
                    else res.json().then((data) => {
                        var jData = JSON.parse(data.value)

                        resolve({
                            ok: false,
                            msg: jData.message,
                        })
                    });
                })
                .catch((errorMsg) => {
                    resolve({
                        ok: false,
                        msg: errorMsg.toString(),
                    })
                });
        })
    }

    postPublicLoginPortal = (loginInfo) => {
        return new Promise((resolve, reject) => {
            fetch(ApiLocation.api_host + ':' + ApiLocation.api_port + ApiLocation.api_portal + 'authenticate',
                {
                    method: 'POST', headers: { 'Content-Type': 'application/json', }, body: loginInfo
                })
                .then((res) => {
                    if (res.ok === true) {
                        res.json().then((data) => {
                            resolve({
                                ok: true,
                                payload: data,
                            })
                        })
                    }
                    else {
                        res.json().then((data) => {
                            resolve({
                                ok: false,
                                msg: data.message,
                            })
                        })
                    }
                })
                .catch((errorMsg) => {
                    resolve({
                        ok: false,
                        msg: errorMsg.toString(),
                    })
                });
        })
    }
}