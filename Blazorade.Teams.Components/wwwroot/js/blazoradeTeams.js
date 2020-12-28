
export function initialize(callback) {
    console.log("initializing", callback);
    microsoftTeams.initialize(() => {
        console.log("initialized");
        invokeCallback(callback);
    });
}

export function getContext(callback) {
    console.log("getContext", callback);
    microsoftTeams.getContext((ctx) => {
        console.log("gotContext", ctx);
        invokeCallback(callback, ctx);
    });
}

export function appInitialization_notifyAppLoaded() {
    microsoftTeams.appInitialization.notifyAppLoaded();
}

export function appInitialization_notifyFailure(failedRequest) {
    microsoftTeams.appInitialization.notifyFailure(failedRequest);
}

export function appInitialization_notifySuccess() {
    microsoftTeams.appInitialization.notifySuccess();
}

export function authentication_getAuthToken(request, successCallback, failureCallback) {
    console.log("getAuthToken", request);
    request.successCallback = function (token) {
        console.log("gotAuthToken", token);
        invokeCallback(successCallback, token);
    };
    request.failureCallback = function (reason) {
        console.error("gotAuthToken", reason);
        invokeCallback(failureCallback, reason);
    }
    microsoftTeams.authentication.getAuthToken(request);
}

function invokeCallback(callback, ...args) {
    console.log("invokeCallback", callback, args);
    if (callback && callback.target && callback.methodName) {
        callback.target.invokeMethodAsync(callback.methodName, ...args);
    }
    else {
        console.error("invokeCallbck", "Given callback cannot be used for invoking a callback.", callback, args);
    }
}
