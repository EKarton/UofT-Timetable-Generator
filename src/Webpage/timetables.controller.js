"use strict";

(function(){
	var app = angular.module("timetableApp");
	app.controller("TimetablesController", function($scope, $location, Generator){
		$scope.timetables = Generator.generateUoftTimetables();
		$scope.wasd = Generator.wasd;
	});
}());