
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



export function authentication_authenticate(args) {
    console.debug("authentication_authenticate", args);

    microsoftTeams.authentication.authenticate({
        url: args.data.url,
        successCallback: function (result) {
            console.debug("authentication_authenticate", "success", result);
            invokeCallback(args.successCallback, result);
        },
        failureCallback: function (reason) {
            console.error("authentication_authenticate", "failure", reason);
            invokeCallback(args.failureCallback, reason);
        }
    });
}

export function authentication_getAuthToken(args) {
    console.debug("authentication_getAuthToken", args);

    try {
        microsoftTeams.authentication.getAuthToken({
            successCallback: (token) => {
                console.debug("authentication_getAuthToken", "success", token);
                invokeCallback(args.successCallback, token);
            },
            failureCallback: (err) => {
                console.error("authentication_getAuthToken", "failure", err);
                invokeCallback(args.failureCallback, err);
            }
        });
    }
    catch (err) {
        invokeCallback(args.failureCallback, err);
    }

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
