"use strict";

(function(){
	var app = angular.module("timetableApp");
	app.controller("CoursesController", function($scope, $location, Generator){

		// Courses which were found by the query in textbox
		$scope.courseResults = [];

		// Stores which courses (by term) were selected
		$scope.fallCourses = [];
		$scope.winterCourses = [];

		// Called when the user types in the textbox
		$scope.changeCourseResults = function(){
			$scope.courseResults = Generator.getCourses($scope.query);
		};

		// Called when the user clicks on a search result
		$scope.selectCourse = function(course){
			var code = course.code;
			var term = course.term;

			console.log("Adding a course, " + code + ", " + term);

			if (term === "F")
				$scope.fallCourses.push(course);
			else if (term === "S")
				$scope.winterCourses.push(course);
			else if (term === "Y")
			{
				$scope.winterCourses.push(course);
				$scope.fallCourses.push(course);
			}
		};

		$scope.makeTimetables = function(){
			Generator.uoftFallCourseCodes = $scope.fallCourses;
			Generator.uoftWinterCourseCodes = $scope.winterCourses;

			$location.path("timetables");
		};
	});
}());