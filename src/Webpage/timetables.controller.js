"use strict";

(function(){
	var app = angular.module("timetableApp");
	app.controller("TimetablesController", function($scope, $location, Generator){
        $scope.timetables = "";

        Generator.generateUoftTimetables()
            .then(function (response) {
                console.log("Got data");
                $scope.timetables = response.data;
            }, function (response) {
                $scope.timetables = "ERROR";
            });
	});
}());