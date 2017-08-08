"use strict";

(function(){
    var app = angular.module("timetableApp", ["ngRoute"]);
    app.config(function($routeProvider){
        $routeProvider
        .when("/", {
            templateUrl: "courses.html",
            controller: "CoursesController"
        })
        .when("/timetables", {
            templateUrl: "timetables.html",
            controller: "TimetablesController"
        })
        .otherwise({ redirectTo: "/" });
    });
}());