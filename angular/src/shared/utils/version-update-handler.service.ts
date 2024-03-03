import { ErrorHandler, Injectable } from "@angular/core";

@Injectable()
export class VersionUpdateHandlerService implements ErrorHandler {
    handleError(error: any): void {
        console.log(error);
        const chunkFailedMessage = /Loading chunk [\d]+ failed/;
        if (chunkFailedMessage.test(error.message)) {
            if (confirm("New version available. Load New Version?")) {
                window.location.reload();
            }
        }
    }
}
