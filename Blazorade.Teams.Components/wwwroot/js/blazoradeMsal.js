import { invokeCallback } from "./blazoradeTeams.js";

export function getTokenSilent(config, context, successCallback, failureCallback) {
    config.auth.redirectUri = `${window.location.protocol}//${window.location.host}/`;
    config.cache = {
        cacheLocation: "localStorage"
    };
    
    console.log("getTokenSilent", config, context);


    let msalClient = new msal.PublicClientApplication(config);
    console.log("msalClient", msalClient);

    let homeId = context.userObjectId + "." + context.tid;
    let account = msalClient.getAccountByHomeId(homeId);
    console.log("account", account);

    msalClient
        .acquireTokenSilent({
            scopes: [".default"],
            account: account
        })
        .then(result => {
            console.log("success", result);
            invokeCallback(successCallback, result);
        })
        .catch(err => {
            console.warn("failed getting token silently", "Falling back to popup", err);
            getTokenWithPopup(msalClient, context.loginHint, successCallback, failureCallback);
        })
        ;
}

function getTokenWithPopup(msalClient, loginHint, successCallback, failureCallback) {
    console.log("getTokenWithPopup", msalClient, loginHint);

    try {
        msalClient
            .loginPopup({
                scopes: [".default"],
                loginHint
            })
            .then(result => {
                console.log("success", result);
                invokeCallback(successCallback, result);
            })
            .catch(err => {
                console.error("getTokenWithPopup", err);
                invokeCallback(failureCallback, err);
            })
            ;
    }
    catch (err) {
        console.error("getTokenWithPopup", err);
        invokeCallback(failureCallback, err);
    }
}