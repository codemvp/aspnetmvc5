'use strict';

//following is our application module.ngGrid is the angular grid that we need to use to display data.
var movieApp = angular.module('movieApp', ['ngGrid'])
movieApp.directive('datepicker', function () {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: function (scope, element, attrs, ngModelCtrl) {
            element.datepicker();
            element.bind('blur keyup change', function () {
                var model = attrs.ngModel;
                if (model.indexOf(".") > -1) scope[model.replace(/\.[^.]*/, "")][model.replace(/[^.]*\./, "")] = element.val();
                else scope[model] = element.val();
            });
        }
    };
});

var url = 'api/MovieApi';

//the factory object for the webAPI call.
movieApp.factory('movieRepository', function ($http) {
    return {
        getMovies: function (callback) {
            $http.get(url).success(callback);
        },
        //method for insert
        insertMovie: function (callback, movie) {
            var movie = {
                "Id": movie.Id,
                "Title": movie.Title,
                "YearReleased": movie.YearReleased,
                "Rating": movie.Rating
            };
            $http.post(url, movie).success(callback);
        },
        //method for update
        updateMovie: function (callback, movie) {
            var movie = {
                "Id": movie.Id,
                "Title": movie.Title,
                "YearReleased": movie.YearReleased,
                "Rating": movie.Rating
            };
            $http.put(url + '/' + movie.Id, movie).success(callback);
        },
        //method for delete
        deleteMovie: function (callback, id) {
            $http.delete(url + '/' + id)
                .success(callback)
            ;
        }
    }
});


//controller   
movieApp.controller('movieController', function ($scope, movieRepository) {
    console.log($scope);
    //getMovies();

    function getMovies() {
        movieRepository.getMovies(function (results) {
            $scope.movieData = results;
        })
    }

    $scope.setScope = function (obj, action) {
        $scope.action = action;
        $scope.New = obj;
    }

    $scope.gridOptions = {
        data: 'movieData',
        showGroupPanel: true,
        columnDefs:
            [
                { field: 'Id', displayName: 'Id' },
                { field: 'Title', displayName: 'Title' },
                { field: 'YearReleased', displayName: 'Year Released' },
                { field: 'Rating', displayName: 'Rating' },
                { displayName: 'Options', cellTemplate: '<input type="button" ng-click="setScope(row.entity,\'edit\')" name="edit"  value="Edit">&nbsp;<input type="button" ng-click="DeleteMovie(row.entity.Id)" name="delete"  value="Delete">' }
            ]
    };

    $scope.update = function () {
        if ($scope.action == 'edit') {
            movieRepository.updateMovie(function () {
                $scope.status = 'movie updated successfully';
                alert('movie updated successfully');
                getMovies();
            }, $scope.New)
            $scope.action = '';
        }
        else {
            movieRepository.insertMovie(function () {
                alert('movie inserted successfully');
                getMovies();
            }, $scope.New)
        }
    }

    $scope.DeleteMovie = function (id) {
        if (IsAdmin() == "false") {
            alert('You do not have sufficient right to delete record. ');
            return;
        }
        movieRepository.deleteMovie(function () {
            alert('Movie deleted');
            getMovies();
        }, id)
    }

    var ratings = [
    {
        0: "G"
    },
    {
        1: "PG"
    },
    {
        2: "M"
    },
    {
        3: "MA"
    },
    {
        4: "R"
    }
    ];

    $scope.ratings = ratings.reduce(function (memo, obj) {
        return angular.extend(memo, obj);
    }, {});
});