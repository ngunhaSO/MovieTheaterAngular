(function () {
    "use strict";

    angular.module("movieTheaterRating")
        .config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {

        $urlRouterProvider.otherwise('/');
        $stateProvider
            .state('home', {
                url: '/',
                templateUrl: 'app/home/homeTemplate.html'
            })
            .state('movies', {
                url: '/movies',
                templateUrl: 'app/movie/movieTemplate.html',
                controller: 'MoviesCtrl as vm'
            })
            .state('movieDetail', {
                url: '/movies/:movieId',
                templateUrl: 'app/movie/movieDetailTemplate.html',
                controller: 'MovieDetailCtrl as vm',
                resolve: {
                    movieService: 'movieService',
                    movie: function (movieService, $stateParams) {
                        var movieId = $stateParams.movieId;
                        return movieService.getMovieByID(movieId).then(getMovieSuccess)
                                .catch(getMovieError);
                        function getMovieSuccess(response) {
                            return response;
                        }
                        function getMovieError(errorMsg) {
                            console.log('Error msg: ' + errorMsg);
                        }
                    }
                }
            })
            .state('movieEdit', {
                url: '/movies/edit/:movieId',
                templateUrl: 'app/movie/movieEditTemplate.html',
                controller: 'MovieEditCtrl as vm',
                resolve: {
                    movieService: 'movieService',
                    movie: function (movieService, $stateParams) {
                        var movieId = $stateParams.movieId;
                        return movieService.getMovieByID(movieId)
                                            .then(getMovieSuccess)
                                            .catch(getMovieError);
                        function getMovieSuccess(response) {
                            return response;
                        }
                        function getMovieError(errorMsg) {
                            console.log('error msg: ' + errorMsg);
                        }
                    }
                }
            })
    }]);
}());