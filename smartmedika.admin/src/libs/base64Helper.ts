export class Base64Helper{
    static toBlob(base64:string):Blob{
        var BASE64_MARKER = ';base64,';
        if (base64.indexOf(BASE64_MARKER) == -1) {
            var parts = base64.split(',');
            var contentType = parts[0].split(':')[1];
            var raw = parts[1];
            
            return new Blob([raw], {type: contentType});
        }
        else {
            var parts = base64.split(BASE64_MARKER);
            var contentType = parts[0].split(':')[1];
            var raw = window.atob(parts[1]);
            var rawLength = raw.length;
            
            var uInt8Array = new Uint8Array(rawLength);
            
            for (var i = 0; i < rawLength; ++i) {
                uInt8Array[i] = raw.charCodeAt(i);
            }
            
            return new Blob([uInt8Array], {type: contentType});
        }
    }
}