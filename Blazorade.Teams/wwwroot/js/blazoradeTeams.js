
export function acquireToken(args) {
    console.debug("acquireToken", args);
    let msalClient = createMsalClient(args);
    let request = createMsalTokenRequest(msalClient, args);

    console.debug("acquireToken", "request", request);

    msalClient.acquireTokenSilent(request)
        .then(result => {
            console.debug("acquireTokenSilent.succeeded", result);
            invokeCallback(args.successCallback, result);
        })
        .catch(err => {
            console.warn("acquireTokenSilent.failed", "must fallback to using dialog.", err);
            launchAuthDialog(args);
        })
        ;

}

export function acquireTokenPopup(args) {
    launchAuthDialog(args);
}

export function msal_acquireTokenSilent(args) {
    console.debug("msal_acquireTokenSilent", args);
    let msalClient = createMsalClient(args);
    let request = createMsTokenRequest(msalClient, args);

    console.debug("msal_acquireTokenSilent", "request", request);

    msalClient.acquireTokenSilent(request)
        .then(result => {
            console.debug("msal_acquireTokenSilent", "result", result);
            invokeCallback(args.successCallback, result);
        })
        .catch(err => {
            console.warn("msal_acquireTokenSilent", err);
            invokeCallback(args.successCallback, null);
        })
        ;
}

export function msal_loginRedirect(args, msalConfig, loginHint, state) {
    console.debug("msal_loginRedirect", args);

    setMsalConfigDefault(args.msalConfig);
    let msalClient = new msal.PublicClientApplication(args.msalConfig);

    let request = {
        scopes: [".default"],
        authority: args.msalConfig.auth.authority,
        loginHint: args.loginHint,
        redirectUri: window.location.origin,
        state: args.state
    };

    console.debug("msal_loginRedirect", "request", request);

    msalClient.loginRedirect(request)
        .then(result => {
            console.debug("msal_loginRedirect.succeeded", result);
        })
        .catch(err => {
            console.error("msal_loginRedirect.failed", err);

            microsoftTeams.initialize(() => {
                microsoftTeams.authentication.notifyFailure(err);
            })
        })
        ;

    return true;
}



export function initialize(args) {
    console.debug("initialize");

    microsoftTeams.initialize(() => {
        console.debug("initialize.complete");
        invokeCallback(args.successCallback, null);
    });
}

export function getContext(args) {
    console.debug("getContext");
    microsoftTeams.getContext((ctx) => {
        console.debug("getContext.complete", ctx);
        invokeCallback(args.successCallback, ctx);
    });
}

export function isTeamsHostAvailable() {
    let isHostAvailable = window.parent !== window.self && microsoftTeams !== undefined;
    console.debug("isTeamsHostAvailable", isHostAvailable);
    return isHostAvailable;
}



export function authentication_authenticate(authParams) {
    console.debug("authentication_authenticate", authParams);
    microsoftTeams.authentication.authenticate(authParams);
}

export function authentication_notifySuccess(result, callbackUrl) {
    console.debug("authentication_notifySuccess", result, callbackUrl);
    microsoftTeams.authentication.notifySuccess(result, callbackUrl);
}

export function authentication_notifyFailure(reason, callbackUrl) {
    console.debug("authentication_notifyFailure", reason, callbackUrl);
    microsoftTeams.authentication.notifyFailure(reason, callbackUrl);
}

export function appInitialization_notifyAppLoaded() {
    console.debug("appInitialization.notifyAppLoaded");
    microsoftTeams.appInitialization.notifyAppLoaded();
}

export function appInitialization_notifyFailure(failedRequest) {
    consol.error("appInitialization.notifyFailure", failedRequest);
    microsoftTeams.appInitialization.notifyFailure(failedRequest);
}

export function appInitialization_notifySuccess() {
    console.debug("appInitialization.notifySuccess");
    microsoftTeams.appInitialization.notifySuccess();
}



export function settings_getSettings(args) {
    console.debug("settings.getSettings");
    microsoftTeams.settings.getSettings((settings) => {
        console.debug("settings.getSettings.complete", settings);
        invokeCallback(args.successCallback, settings);
    });
}

export function settings_setValidityState(validityState) {
    microsoftTeams.settings.setValidityState(validityState);
    console.debug("settings.setValidityState", validityState);
}

export function settings_registerOnSaveHandler(args) {
    console.debug("settings.registerOnSaveHandler", args);
    microsoftTeams.settings.registerOnSaveHandler((evt) => {
        console.debug("saving settings", evt);
        let saveSettings = () => {
            microsoftTeams.settings.setSettings(args.data.settings);
            invokeCallback(args.successCallback);
            evt.notifySuccess();
        };

        try {
            if (isCallbackValid(args.data.savingCallback)) {
                args.data.savingCallback.target.invokeMethodAsync(args.data.savingCallback.methodName, args.data.savingCallbackData)
                    .then(() => {
                        console.debug("saving callback completed successfully");
                        saveSettings();
                    })
                    .catch((err) => {
                        console.error("saving callback failed", err);
                    });
            }
            else {
                saveSettings();
            }
        }
        catch (err) {
            console.error("saving settings", err, args);
            evt.notifyFailure(err);
            invokeCallback(args.failureCallback);
        }
    });
}

export function settings_registerOnRemoveHandler(args) {
    console.debug("settings.registerOnRemoveHandler", args);
    microsoftTeams.settings.registerOnRemoveHandler((evt) => {
        console.debug("removing settings");
        let doRemove = () => {
            invokeCallback(args.successCallback);
            evt.notifySuccess();
        };

        try {
            if (isCallbackValid(args.data.removingCallback)) {
                args.data.removingCallback.target.invokeMethodAsync(args.data.removingCallback.methodName, args.data.removingCallbackData)
                    .then(() => {
                        doRemove();
                    })
                    .catch((err) => {
                        console.error("removing callback failed", err);
                    });
            }
            else {
                doRemove();
            }
        }
        catch (err) {
            console.error("removing settings", err, args);
            evt.notifyFailure(err);
            invokeCallback(args.failureCallback);
        }
    });
}


export function isCallbackValid(callback) {
    return callback && callback.target && callback.methodName && true;
}

export function invokeCallback(callback, ...args) {
    if (callback && callback.target && callback.methodName) {
        callback.target.invokeMethodAsync(callback.methodName, ...args);
    }
    else {
        console.error("invokeCallbck", "Given callback cannot be used for invoking a callback.", callback, args);
    }
}



function createMsalClient(args) {
    setMsalConfigDefault(args.data.msalConfig);

    let msalClient = new msal.PublicClientApplication(args.data.msalConfig);

    console.debug("createMsalClient", args, msalClient);

    return msalClient;
}

function createMsalTokenRequest(msalClient, args) {
    console.debug("createMsalTokenRequest", msalClient, args);

    let homeId = args.data.context.userObjectId + "." + args.data.context.tid;
    let account = msalClient.getAccountByHomeId(homeId);

    console.debug("createMsalTokenRequest", "homeId", homeId);
    console.debug("createMsalTokenRequest", "account", account);

    return {
        scopes: [".default"],
        account: account
    };
}

function launchAuthDialog(args) {
    console.debug("launchAuthDialog", args);

    microsoftTeams.initialize(() => {
        microsoftTeams.authentication.authenticate({
            url: window.location.origin + "?" + args.data.context.userPrincipalName + "#blazorade-login-request",
            successCallback: result => {
                console.debug("launchAuthDialog.succeeded", result);
                invokeCallback(args.successCallback, result);
            },
            failureCallback: err => {
                console.error("launchAuthDialog.failed", err);
                invokeCallback(args.successCallback, null);
            }
        });
    });
}

function setMsalConfigDefault(msalConfig) {
    console.debug("setMsalConfigDefault", msalConfig);

    msalConfig.auth.redirectUri = window.location.origin;
    msalConfig.cache = {
        cacheLocation: "localStorage"
    };
}