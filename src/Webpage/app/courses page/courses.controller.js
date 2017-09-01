"use strict";

(function(){
	var app = angular.module("timetableApp");
    app.controller("CoursesController", function ($scope, $location, TimetableGenerator) {

        $scope.selectedCourses = (function () {
            // See if it is saved as session storage
            var savedEntry = JSON.parse(sessionStorage.getItem("selectedCourses"));
            if (savedEntry === undefined || savedEntry === null)
                return [];
            else
                return savedEntry;
        }());

        $scope.isCourseNotSelected = function (course) {
            return !$scope.isCourseSelected(course);
        };

        $scope.isCourseSelected = function (course) {
            for (var i = 0; i < $scope.selectedCourses.length; i++)
            {
                var s = $scope.selectedCourses[i];
                if (s.code == course.code && s.term == course.term)
                    if (s.description === course.description && s.campus === course.campus)
                        return true;
            }
            return false;
        };

        $scope.selectCourse = function (course) {
            // Check if it exists
            if ($scope.isCourseSelected(course) === false)
                $scope.selectedCourses.push(course);
            else
                alert("This course was selected already!");
            console.log($scope.selectedCourses);
        };

        $scope.removeSelectedCourse = function (course) {
            var i = $scope.selectedCourses.indexOf(course);
            if (i != -1) {
                $scope.selectedCourses.splice(i, 1);
                return true;
            }
            return false;
        };

        $scope.makeTimetables = function () {
            // Save the selected courses in session storage
            sessionStorage.setItem("selectedCourses", JSON.stringify($scope.selectedCourses));

            // Grab only the course codes
            var courseCodes = [];
            for (var i = 0; i < $scope.selectedCourses.length; i++)
                courseCodes.push($scope.selectedCourses[i].code);

            //Generator.courseCodes = courseCodes;
            TimetableGenerator.generateTimetables(courseCodes);

			$location.path("timetables");
        };
	});
}());