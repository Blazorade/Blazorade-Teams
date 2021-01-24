
export function initialize(args) {
    microsoftTeams.initialize(() => {
        invokeCallback(args.successCallback, null);
    });
}

export function getContext(args) {
    microsoftTeams.getContext((ctx) => {
        invokeCallback(args.successCallback, ctx);
    });
}

export function isTeamsHostAvailable() {
    let isHostAvailable = window.parent !== window.self && microsoftTeams !== undefined;
    return isHostAvailable;
}



export function appInitialization_notifyAppLoaded() {
    microsoftTeams.appInitialization.notifyAppLoaded();
}

export function appInitialization_notifyFailure(failedRequest) {
    consol.error("appInitialization.notifyFailure", failedRequest);
    microsoftTeams.appInitialization.notifyFailure(failedRequest);
}

export function appInitialization_notifySuccess() {
    microsoftTeams.appInitialization.notifySuccess();
}



export function settings_getSettings(args) {
    microsoftTeams.settings.getSettings((settings) => {
        invokeCallback(args.successCallback, settings);
    });
}

export function settings_setValidityState(validityState) {
    microsoftTeams.settings.setValidityState(validityState);
}

export function settings_registerOnSaveHandler(args) {
    microsoftTeams.settings.registerOnSaveHandler((evt) => {
        let saveSettings = () => {
            microsoftTeams.settings.setSettings(args.data.settings);
            invokeCallback(args.successCallback);
            evt.notifySuccess();
        };

        try {
            if (isCallbackValid(args.data.savingCallback)) {
                args.data.savingCallback.target.invokeMethodAsync(args.data.savingCallback.methodName, args.data.savingCallbackData)
                    .then(() => {
                        saveSettings();
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
    microsoftTeams.settings.registerOnRemoveHandler((evt) => {
        let doRemove = () => {
            invokeCallback(args.successCallback);
            evt.notifySuccess();
        };

        try {
            if (isCallbackValid(args.data.removingCallback)) {
                args.data.removingCallback.target.invokeMethodAsync(args.data.removingCallback.methodName, args.data.removingCallbackData)
                    .then(() => {
                        doRemove();
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

export function getAuthToken(args) {
    var authTokenRequest = {
        successCallback: function (result) {
            invokeCallback(args.successCallback, result);
        },
        failureCallback: function(error) {
            invokeCallback(arg.failureCallback, error);
        }
    };

    microsoftTeams.authentication.getAuthToken(authTokenRequest);
}

export function showConsentDialog(args) {
    var queryString = "?scopes=" + args.data.scopes + (args.data.api ? "&api=" + args.data.api : "");

    microsoftTeams.authentication.authenticate({
        url: window.location.origin + "/auth-start" + queryString,
        width: 600,
        height: 535,
        successCallback: (result) => invokeCallback(args.successCallback, result),
        failureCallback: (reason) => invokeCallback(args.failureCallback, reason)
    });
}

export function notifyConsentSuccess(args) {
    microsoftTeams.authentication.notifySuccess({ consented: true, token: args.data.token });
}

export function notifyConsentFailure(args) {
    microsoftTeams.authentication.notifySuccess({ consented: false, error: args.data.error });
}