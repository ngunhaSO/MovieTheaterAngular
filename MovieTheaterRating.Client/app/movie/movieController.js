(function () {
    'use strict';
    angular.module('movie')
                //.filter('movieFilter', function () {
                //    return function (number) {

                //        // Ensure that the passed in data is a number
                //        if (isNaN(number) || number < 1) {
                //            return number;
                //        } else {
                //            return number;

                //        }
                //    }
                //})
               .controller('MoviesCtrl', ['movieService', MoviesCtrl]);

    angular.module('movieTheaterRating')
               .controller('MoviesCtrl', ['movieService', '$scope', MoviesCtrl]);

            //.controller('MoviesCtrl', ['moviesResource', MoviesCtrl]); ////Uncomment his one to use $resource

    function MoviesCtrl(movieService, $scope) {
        var vm = this;
        movieService.getMovies()
                    .then(getMoviesSuccess, null, getMoviesNotification)
                    .catch(errorCallback)
                    .finally(getMoviesComplete);

        function getMoviesSuccess(data) {
            vm.movies = data;
        }

        function getMoviesNotification(notification) {
            console.log('Promise Notification: ' + notification);
        }

        function errorCallback(errorMsg) {
            console.log('Error msg: ' + errorMsg);
        }

        function getMoviesComplete() {
            console.log('completed!');
        }

        vm.max = 10;
        vm.isReadonly = true;


        vm.showImage = false;

        vm.toggleImage = function () {
            vm.showImage = !vm.showImage;
        }

        vm.showAdvancedSearch = false;
        vm.advancedSearch = function () {
            vm.showAdvancedSearch = !vm.showAdvancedSearch;
        }
        
        vm.showClearSearch = false;
        vm.hoveringOver = function (value) {
            vm.showClearSearch = true;
        }

        vm.leave = function (value) {
            vm.showClearSearch = false;
        }

        //vm.matchSearch = function (input) {
        //    console.log('search');
        //    if (isNaN(input)) {
        //        return input;
        //    } else {
        //        return $filter('number')(input, fractionSize);
        //    };
        //}

        //vm.myForm = {
        //    firstName: '',
        //    lastName: ''
        //};
        //vm.submitForm = function () {
        //    vm.myForm.firstName = vm.testForm.first; //assign value from input to our vm.myForm variable
        //    vm.myForm.lastName = vm.testForm.last;
        //    console.log("fname: " + vm.myForm.firstName);
        //    console.log("lname: " + vm.myForm.lastName);

        //    if (vm.myForm.firstName == 'Jacky') {
        //        vm.movies = vm.movies.slice(Math.max(vm.movies.length - 5, 1));
        //    }
        //    else {
        //        vm.movies = vm.moviesBackup;
        //    }
        //}

        vm.myForm = {
            firstName: '',
            lastName: ''
        };

        vm.submitForm = function () {
            vm.myForm.firstName = vm.testForm.first; //assign value from input to our vm.myForm variable
            vm.myForm.lastName = vm.testForm.last;
            console.log("fname: " + vm.myForm.firstName);
            console.log("lname: " + vm.myForm.lastName);

            if (vm.myForm.firstName == 'Jacky') {
                vm.movies = vm.movies.slice(Math.max(vm.movies.length - 5, 1));
            }
        }
    }

    //// ==== UNCOMMENT THE FOLLOWING TO USE $resource ===
    //function MoviesCtrl(moviesResource) {
    //    var vm = this;
    //    moviesResource.query(function (data) {
    //        vm.movies = data;
    //    });

    //    vm.showImage = false;

    //    vm.toggleImage = function () {
    //        vm.showImage = !vm.showImage;
    //    }
    //}
}());