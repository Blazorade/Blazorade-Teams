
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



export function invokeCallback(callback, ...args) {
    console.log("invokeCallback", callback, args);
    if (callback && callback.target && callback.methodName) {
        callback.target.invokeMethodAsync(callback.methodName, ...args);
    }
    else {
        console.error("invokeCallbck", "Given callback cannot be used for invoking a callback.", callback, args);
    }
}
