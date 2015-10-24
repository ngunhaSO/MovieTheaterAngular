(function () {
    "use strict";

    angular
        .module("constantServices", [])
    	.constant("appSettings",
        {
            serverPath: "http://localhost:54817" //url path of our web api project
        });
}());