"use strict";

(function(){
    var app = angular.module("timetableApp");
    app.service("CourseSearch", function ($http) {

        /**
         * A class used to represent a course
         * @constructor
         * @param {String} code - The course code
         * @param {String} term - The term, either "Fall", "Winter", or "Year"
         * @param {String} description - A brief description of the course
         * @param {String} campus - Which campus the course is being taught at.
         */
        var Course = function (code, term, description, campus) {
            this.code = code;
            this.term = term;
            this.description = description;
            this.campus = campus;
            this.isSelected = false;
        };

        // Course results and the query for that course result, cached, from calling th getUoftCourses()
        var cachedCourseResults = [];
        var oldQuery = "";

        /**
         * Get uoft courses, given the course code
         * Pre-condition: the given course code must be at least 3 chars in length.
         * If not, it will not get the course results
         * @param {string} query - Complete / incomplete UofT course codes
         * @param {method(courseResults)} onSuccess - The callback that recieves UofT course results
         * @param {method(promise)} onFail - The callback that handles when it is uable to get course results
         */
        this.getUoftCourses = function (query, onSuccess, onError) {

            // Avoid making an http request if the user enters the same department code
            if (this.oldQuery === query)
                return cachedCourseResults;

            // Check if the new department code is >= 3 chars long
            if (query.length != 3)
                return;

            var obj = this;
            var url = "http://uofttimetablegenerator.azurewebsites.net/api/courses?query=" + query; // "http://localhost:53235/api/courses?query=" + query; // 

            $http.get(url).then(
                function (promise) {
                    // Get the course results and parse them
                    var results = promise.data;
                    obj.courseResults = [];
                    for (var i = 0; i < results.length; i++) {
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
                        obj.courseResults.push(new Course(code, term, description, campus));
                    }

                    // Cache the course results
                    obj.cachedCourseResults = obj.courseResults;

                    onSuccess(obj.courseResults);
                },
                function (promise) {
                    onError(promise);
                }
            );
        };  
    });
}());