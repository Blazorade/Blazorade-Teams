import { invokeCallback } from "./blazoradeTeams.js";

export function getTokenSilent(args) {
    args.data.config.auth.redirectUri = `${window.location.protocol}//${window.location.host}/`;
    args.data.config.cache = {
        cacheLocation: "localStorage"
    };

    console.debug("getTokenSilent", args);

    let msalClient = new msal.PublicClientApplication(args.data.config);

    let homeId = args.data.context.userObjectId + "." + args.data.context.tid;
    let account = msalClient.getAccountByHomeId(homeId);

    msalClient
        .acquireTokenSilent({
            scopes: [".default"],
            account: account
        })
        .then(result => {
            invokeCallback(args.successCallback, result);
        })
        .catch(err => {
            console.warn("failed getting token silently", "Falling back to popup", err);
            getTokenWithPopup(args.successCallback, args.failureCallback, msalClient, args.data.context.loginHint);
        })
        ;
}

function getTokenWithPopup(successCallback, failureCallback, msalClient, loginHint) {
    console.debug("getTokenWithPopup", msalClient, loginHint);

    try {
        msalClient
            .loginPopup({
                scopes: [".default"],
                loginHint
            })
            .then(result => {
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