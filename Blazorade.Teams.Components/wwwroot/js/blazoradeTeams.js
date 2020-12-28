
export function initialize(callback) {
    microsoftTeams.initialize(() => {
        if (callback && callback.target && callback.methodName) {
            callback.target.invokeMethodAsync(callback.methodName);
        }
        else {
            microsoftTeams.appInitialization.notifySuccess();
        }
    });
}

export function getContext(callback) {
    microsoftTeams.getContext((ctx) => {
        if (callback && callback.target && callback.methodName) {
            callback.target.invokeMethodAsync(callback.methodName, ctx);
        }
        else {
            microsoftTeams.appInitialization.notifySuccess();
        }
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

console.log("Blazorade Teams scripts loaded.");
console.log("microsoftTeams", microsoftTeams);