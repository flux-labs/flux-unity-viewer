var MyPlugin = {
    CheckLocation: function()
    {
        console.log(window.location.hash.match(/access_token/));
    },
    StringReturnValueFunction: function()
    {
        var returnStr = "bla";
        var buffer = _malloc(lengthBytesUTF8(returnStr) + 1);
        writeStringToMemory(returnStr, buffer);
        return buffer;
    },
};

mergeInto(LibraryManager.library, MyPlugin);