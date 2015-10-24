(function () {

    angular.module('movie').factory('movieService', ['appSettings', '$q', '$timeout', '$http', movieService]);

    function movieService(appSettings, $q, $timeout, $http) {
        return {
            getMovies: getMovies,
            getMovieByID: getMovieByID,
            updateMovie : updateMovie
        };

        //========== GET ALL MOVIES ==========================//
        function getMovies() {
            return $http({
                method: 'GET',
                url: appSettings.serverPath + '/api/Movie',
                transformResponse: transformGetMovies,
                cache : true
            }).then(sendResponseData).catch(sendResponseError);
        }

        function transformGetMovies(data, headersGetter) {
            var transformed = angular.fromJson(data);         
            return transformed;
        }

        function sendResponseData(response) {
            return response.data;
        }


        function sendResponseError(response) {
            return $q.reject('error retrieving movies. (HTTP status: ' + response.status + ')');
        }

        //========== GET MOVIE BY ID ==========================//
        function getMovieByID(movieId) {
            return $http.get(appSettings.serverPath + '/api/Movie/' + movieId)
                        .then(sendResponseData)
                        .catch(sendResponseError);
        }

        //========== UPDATE A MOVIE BY ID =====================//
        function updateMovie(movie) {
            return $http({
                method: 'PUT',
                url: appSettings.serverPath + '/api/Movie/' + movie.Id,
                data: movie
            })
            .then(updateSuccess)
            .catch(updateError);
        }

        function updateSuccess(response) {
            return 'Movie updated: ' + response.config.data.title;
        }

        function updateError(response) {
            return $q.reject('Error updating a movie. (HTTP Status: ' + response.status + ')');
        }

    }



}())