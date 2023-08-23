import "./qr-scanner-worker.min.js"
import QrScanner from "./qr-scanner.min.js"

export function createQrScanner(videoElem, successfulScanHandler) {
    var scanner = new QrScanner(
        videoElem,
        result => callback(successfulScanHandler, result),
        { returnDetailedScanResult: true, highlightScanRegion: true }
    );

    scanner.setInversionMode('both');
    return scanner;
}

function callback(successfulScanHandler, result) {
    console.log('decoded qr code:', result)
    successfulScanHandler.invokeMethodAsync ('Callback', result.data)
}

export async function startScanning(qrScannerInstance) {
    await qrScannerInstance.setCamera('environment');
    qrScannerInstance.start();
}

export function stopScanning(qrScannerInstance) {
    qrScannerInstance.stop();
}

export function disposeQrScanner(qrScannerInstance) {
    qrScannerInstance.destroy();
    qrScannerInstance = null;
}