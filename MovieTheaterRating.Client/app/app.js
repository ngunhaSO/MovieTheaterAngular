(function () {
    "use strict";

    angular.module('movieTheaterRating', [
                                            'ui.router' //use for routing
                                            , 'ui.mask' //use for mask the data like ****-**01232
                                            , 'ui.bootstrap' //angular ui bostrap stuff
                                            , 'movie' //module for movie
    ]);

    ////====== UNCOMMENT THESE TO REMOVE TEMPLATE CACHE ========////
    //angular.module('movieTheaterRating').run(function ($rootScope, $templateCache) {
    //    $rootScope.$on('$viewContentLoaded', function () {
    //        console.log("ap run");
    //        $templateCache.removeAll();
    //    })
    //});

}());
