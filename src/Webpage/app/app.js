"use strict";

(function(){
    var app = angular.module("timetableApp", ["ngRoute"]);
    app.config(function ($routeProvider, $compileProvider){
        $routeProvider
            .when("/", {
                templateUrl: "app/views/courses.html",
                controller: "CoursesController",
                css: "app/styles/homepage.css"
            })
            .when("/timetables", {
                templateUrl: "app/views/timetables.html",
                controller: "TimetablesController",
                css: "app/styles/timetable.css"
            })
            .otherwise({ redirectTo: "/" });

        // Allow controllers to have access to the bindings in components
        $compileProvider.preAssignBindingsEnabled(true);
    });

    app.controller("appController", function ($scope, $route) {

        // A variable holding the css file reference
        $scope.css = "";

        // Make a watch where when a css reference changes in its route, change the href in the html
        $scope.$watch(function () {
            return $route.current && $route.current.css;
        }, function (value) {
            $scope.css = value;
        });
    });
}());