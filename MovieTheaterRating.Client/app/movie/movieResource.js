(function () {
    'use strict';
    angular.module('movie').factory('moviesResource', ['$resource', 'appSettings', moviesResource]);

    function moviesResource( $resource, appSettings) {
        return $resource(appSettings.serverPath + '/api/Movie/:movieId', null, {
            'update': { method: 'PUT' }
        });
    }

}());