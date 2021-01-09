
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
            microsoftTeams.settings.setSettings(args.args.settings);
            invokeCallback(args.successCallback);
            evt.notifySuccess();
        };

        try {
            if (isCallbackValid(args.args.savingCallback)) {
                args.args.savingCallback.target.invokeMethodAsync(args.args.savingCallback.methodName, args.args.savingCallbackData)
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
            if (isCallbackValid(args.args.removingCallback)) {
                args.args.removingCallback.target.invokeMethodAsync(args.args.removingCallback.methodName, args.args.removingCallbackData)
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
