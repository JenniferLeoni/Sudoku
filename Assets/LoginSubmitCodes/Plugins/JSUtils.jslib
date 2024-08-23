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

    }
};
mergeInto(LibraryManager.library, plugin);