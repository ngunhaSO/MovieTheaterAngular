(function () {
    'use strict';

    angular.module('movie')
            .controller('MovieDetailCtrl', ['movie', MovieDetailCtrl]);

    function MovieDetailCtrl(movie) {
        var vm = this;
        vm.movie = movie;
        vm.Title = "Movie: " + vm.movie.title;

        vm.max = 10;
        vm.rate = vm.movie.audienceRating / 100 * 10;
        vm.isReadonly = true;

        vm.hoveringOver = function (value) {
            vm.overStar = value;
        }
    }

}());