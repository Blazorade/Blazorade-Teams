import { invokeCallback } from "./blazoradeTeams.js";

export function getTokenSilent(args, context) {
    args.args.config.auth.redirectUri = `${window.location.protocol}//${window.location.host}/`;
    args.args.config.cache = {
        cacheLocation: "localStorage"
    };
    
    let msalClient = new msal.PublicClientApplication(args.args.config);

    let homeId = args.args.context.userObjectId + "." + args.args.context.tid;
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
            getTokenWithPopup(args.successCallback, args.failureCallback, msalClient, args.args.context.loginHint);
        })
        ;
}

function getTokenWithPopup(successCallback, failureCallback, msalClient, loginHint) {
    console.log("getTokenWithPopup", msalClient, loginHint);

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