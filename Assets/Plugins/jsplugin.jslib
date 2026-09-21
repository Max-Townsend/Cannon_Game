mergeInto(LibraryManager.library, {
    // The WebGL build hook loads CannonStorage before Unity starts.
    RegisterStorageStatus: function (objectName) {
        var target = Pointer_stringify(objectName);
        window.CannonStorage.onStatus(function (text) {
            SendMessage(target, 'StringCallback', text);
        });
    },
    InsertData: function (
        tableName,
        ppID,
        trialNum,
        keyPresses,
        timeStamps,
        aimAngles,
        tarHit,
        endPointFB,
        token,
        pertMag,
        valArray,
        tarPos
    ) {
        window.CannonStorage.write('trial', Pointer_stringify(tableName), {
            ppID: Pointer_stringify(ppID),
            trial: Pointer_stringify(trialNum),
            keyPresses: Pointer_stringify(keyPresses),
            timeStamps: Pointer_stringify(timeStamps),
            aimAngles: Pointer_stringify(aimAngles),
            tarHit: Pointer_stringify(tarHit),
            endPointFB: Pointer_stringify(endPointFB),
            tokenId: Pointer_stringify(token),
            pertMag: Pointer_stringify(pertMag),
            valArray: Pointer_stringify(valArray),
            tarPos: Pointer_stringify(tarPos)
        });
    }
});
