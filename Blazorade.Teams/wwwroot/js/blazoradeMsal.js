import {
    invokeCallback
} from "./blazoradeTeams.js";

export function acquireTokenSilent(args) {
    console.debug("acquireTokenSilent", args);

    let msalClient = createMsalClient(args);
    let request = createTokenRequest(msalClient, args);

    msalClient.acquireTokenSilent(request)
        .then(result => {
            console.debug("acquireTokenSilent.succeeded", result);
            invokeCallback(args.successCallback, result);
        })
        .catch(err => {
            console.warn("acquireTokenSilent.failed", "must fall back to using popup", err);
            invokeCallback(args.successCallback, null);
        })
        ;
}

export function acquireTokenRedirect(args) {
    console.debug("acquireTokenRedirect", args);

    let msalClient = createMsalClient(args);
    let request = createTokenRequest(msalClient, args);

    msalClient.acquireTokenRedirect(request)
        .then(result => {
            console.debug("acquireTokenRedirect.succeeded", result);
            invokeCallback(args.successCallback, result);
        })
        .catch(err => {
            console.error("acquireTokenRedirect.failed", err);
            invokeCallback(args.successCallback, null);
        })
        ;
}



function createMsalClient(args) {
    args.data.config.auth.redirectUri = window.location.origin;
    args.data.config.cache = {
        cacheLocation: "localStorage"
    };

    let msalClient = new msal.PublicClientApplication(args.data.config);

    console.debug("createMsalClient", args, msalClient);

    return msalClient;
}

function createTokenRequest(msalClient, args) {
    let homeId = args.data.context.userObjectId + "." + args.data.context.tid;
    let account = null;//msalClient.getAccountByHomeId(homeId);

    return {
        scopes: [".default"],
        account: account
    };
}
