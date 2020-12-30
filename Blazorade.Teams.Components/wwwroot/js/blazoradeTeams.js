
export function initialize(args) {
    console.log("initializing", args);
    microsoftTeams.initialize(() => {
        console.log("initialized");
        invokeCallback(args.successCallback, null);
    });
}

export function getContext(args) {
    console.log("getContext", args);
    microsoftTeams.getContext((ctx) => {
        console.log("gotContext", ctx);
        invokeCallback(args.successCallback, ctx);
    });
}

export function isTeamsHostAvailable() {
    let isHostAvailable = window.parent !== window.self && microsoftTeams !== undefined;
    console.log("isTeamsHostAvailable", isHostAvailable);
    return isHostAvailable;
}



export function appInitialization_notifyAppLoaded()
{
    microsoftTeams.appInitialization.notifyAppLoaded();
}

export function appInitialization_notifyFailure(failedRequest)
{
    microsoftTeams.appInitialization.notifyFailure(failedRequest);
}

export function appInitialization_notifySuccess()
{
    microsoftTeams.appInitialization.notifySuccess();
}



export function invokeCallback(callback, ...args) {
    console.log("invokeCallback", callback, args);
    if (callback && callback.target && callback.methodName) {
        callback.target.invokeMethodAsync(callback.methodName, ...args);
    }
    else {
        console.error("invokeCallbck", "Given callback cannot be used for invoking a callback.", callback, args);
    }
}
