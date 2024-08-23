var plugin = {
	OpenTab : function(url)
	{
		url = Pointer_stringify(url);
		window.open(url,'_blank');
	},
	GetURLFromPage: function () {

        var returnStr = "not found";

        try{

            returnStr = (window.location != window.parent.location)

            ? document.referrer : document.location.href;

        } catch (error) {

            console.error('Error while getting Url: '+ error);

        }

       

        var bufferSize = lengthBytesUTF8(returnStr) + 1;

        var buffer = _malloc(bufferSize);

        stringToUTF8(returnStr, buffer, bufferSize);

        return buffer;

    },
    SetCookie: function(namePtr, valuePtr, days) {
        var name = UTF8ToString(namePtr);
        var value = UTF8ToString(valuePtr);
        console.log("Setting cookie:", name, value, days); // Log to verify the function is called correctly
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        var secure = location.protocol === 'https:' ? "; Secure" : "";
        document.cookie = name + "=" + (value || "") + expires + "; path=/" + secure; // Path is set to root, Secure attribute added if HTTPS
        console.log("Cookie set:", document.cookie); // Log the current cookies
    },
    GetCookie: function(namePtr) {
        var name = UTF8ToString(namePtr);
        console.log("Getting cookie:", name); // Log the cookie name being requested
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        console.log("Current cookies:", document.cookie); // Log all current cookies
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i].trim();
            console.log("Checking cookie:", c); // Log each cookie being checked
            if (c.indexOf(nameEQ) === 0) {
                var cookieValue = c.substring(nameEQ.length, c.length);
                console.log("Found cookie:", cookieValue); // Log the found cookie value
                // Allocate memory for the cookie value string
                var bufferSize = lengthBytesUTF8(cookieValue) + 1;
                var buffer = _malloc(bufferSize);
                stringToUTF8(cookieValue, buffer, bufferSize);
                return buffer;
            }
        }
        console.log("Cookie not found:", name); // Log if the cookie is not found
        return null;
    }
};
mergeInto(LibraryManager.library, plugin);