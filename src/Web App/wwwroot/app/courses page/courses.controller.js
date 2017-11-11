"use strict";

(function(){
	var app = angular.module("timetableApp");
    app.controller("CoursesController", function ($scope, $location, TimetableGenerator) {

        // The selected courses
        $scope.selectedCourses = (function () {
            // See if it is saved as session storage
            var savedEntry = JSON.parse(sessionStorage.getItem("selectedCourses"));
            if (savedEntry === undefined || savedEntry === null)
                return [];
            else
                return savedEntry;
        }());

        /**
        * Determines if a course is selected or not
        * @param {Course} course - The course
        * @return {bool} - True if the course is NOT selected; else false
        */
        $scope.isCourseNotSelected = function (course) {
            return !$scope.isCourseSelected(course);
        };

        /**
        * Determines if a course is selected or not
        * @param {Course} course - The course
        * @return {bool} - True if the course is selected; else false
        */
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

        /**
        * Selects a course
        * @param {Course} course - The course to be selected
        */
        $scope.selectCourse = function (course) {
            // Check if it exists
            if ($scope.isCourseSelected(course) === false)
                $scope.selectedCourses.push(course);
            else
                alert("This course was selected already!");
        };

        /**
        * Removes a selected course
        * @param {Course} course - The selected course to remove
        * @return {bool} - True if the selected course is found and has been removed; else false
        */
        $scope.removeSelectedCourse = function (course) {
            var i = $scope.selectedCourses.indexOf(course);
            if (i != -1) {
                $scope.selectedCourses.splice(i, 1);
                return true;
            }
            return false;
        };

        /**
        * Generates the timetables.
        * It also stores the selected courses into a session storage.
        * Moreover, it also navigates to the Timetables page
        */
        $scope.makeTimetables = function () {
            // Save the selected courses in session storage
            sessionStorage.setItem("selectedCourses", JSON.stringify($scope.selectedCourses));

            // Grab only the course codes
            var courseCodes = [];
            for (var i = 0; i < $scope.selectedCourses.length; i++)
                courseCodes.push($scope.selectedCourses[i].code);

            //Generator.courseCodes = courseCodes;
            console.log("Making request for generation");
            console.log(courseCodes);
            TimetableGenerator.generateTimetables(courseCodes, null, null,
                function (data) {
                    console.log(data);
                },
                function (promise) {
                    alert("Unable to create timetables");
                    console.log(promise);
                }
            );

			$location.path("timetables");
        };
	});
}());