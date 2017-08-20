"use strict";

(function(){
	var app = angular.module("timetableApp");
    app.controller("CoursesController", function ($scope, $location, Generator) {

        /**
        * A class used to represent a course
        * @constructor
        * @param {String} code The course code
        * @param {String} term The term, either "Fall", "Winter", or "Year"
        * @param {String} description A brief description of the course
        * @param {String} campus Which campus the course is being taught at.
        */
        // A class used to represent the course results
        var Course = function (code, term, description, campus) {
            this.code = code;
            this.term = term;
            this.description = description;
            this.campus = campus;
        };

		// The query used to get the course results
        $scope.oldQuery = "";

        // The course results, got from making the query
		$scope.courseResults = [];

		// Stores which courses were selected
        $scope.selectedCourses = [];

        /**
        * Updates the course results by getting the courses from the server.
        * It makes an HTTP request by specifying at least the first three letters of the course code
        * Pre-condition: "newQuery" must not be null.
        * @method updateCourseResults
        * @param {String} newQuery The first three or more letters of the course code
        */
        $scope.updateCourseResults = function (newQuery) {

            // Avoid making an http request if the user enters the same department code
            if ($scope.oldQuery === newQuery)
                return;

            // Check if the new department code is >= 3 chars long
            if (newQuery.length != 3)
                return;

            // Make a http request to get the courses
            Generator.getCourses(newQuery)
                .then(function (promise) {

                    // Get the course results and parse them
                    var results = promise.data;
                    $scope.courseResults = [];
                    for (var i = 0; i < results.length; i++)
                    {
                        var code = results[i].courseCode;
                        var term = "";
                        if (results[i].term === "F")
                            term = "Fall";
                        else if (results[i].term === "S")
                            term = "Winter";
                        else if (results[i].term == "Y")
                            term = "Year";

                        var description = "";
                        var campus = "St. George";
                        $scope.courseResults.push(new Course(code, term, description, campus));
                    }

                    // Update the department code to match the course results
                    $scope.oldQuery = newQuery;
                }, function (promise) {
                    alert("Unable to get course list from server");
                }
            );
        }

        /**
        * Returns true if a course is not found in $scope.selectedCourses[]; else false.
        * Pre-condition: "course" must not be null.
        * @method isCourseSelected
        * @param {Course} course - A not-null course to search up
        * @return {bool} True if it is not found; else false.
        */
        $scope.isCourseNotSelected = function (course) {
            return !$scope.isCourseSelected(course);
        };

        /**
        * Returns true if a course is found in $scope.selectedCourses[]; else false.
        * Pre-condition: "course" must not be null.
        * @method isCourseSelected
        * @param {Course} course - A not-null course to search up
        * @return {bool} True if it is found; else false.
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
        * Adds a course to $scope.selectedCourses[] if it does not exist already
        * Pre-condition: "course" must be not null.
        * @method selectCourse
        * @param {Course} course - A course to add
        */
        $scope.selectCourse = function (course) {
            // Check if it exists
            if ($scope.isCourseSelected(course) === false)
                $scope.selectedCourses.push(course);
        };

        /**
        * Removes a course from $scope.selectedCourses[]
        * Pre-condition: "course" must be not null.
        * @method removeSelectedCourse
        * @param {Course} course - A course to remove
        * @return {bool} Returns true if it was removed; else false
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
        * Creates the timetables and redirects the page to the Timetables page
        * Pre-condition: $scope.selectedCourses must be defined, containing Course objects
        * @method makeTimetables
        */
        $scope.makeTimetables = function () {
            // Grab only the course codes
            var courseCodes = [];
            for (var i = 0; i < $scope.selectedCourses.length; i++)
                courseCodes.push($scope.selectedCourses[i].code);

            Generator.courseCodes = courseCodes;

			$location.path("timetables");
        };
	});
}());