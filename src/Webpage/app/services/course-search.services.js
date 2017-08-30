"use strict";

(function(){
    var app = angular.module("timetableApp");
    app.service("CourseSearch", function ($http) {

        /**
        * A class used to represent a course
        * @constructor
        * @param {String} code The course code
        * @param {String} term The term, either "Fall", "Winter", or "Year"
        * @param {String} description A brief description of the course
        * @param {String} campus Which campus the course is being taught at.
        */
        var Course = function (code, term, description, campus) {
            this.code = code;
            this.term = term;
            this.description = description;
            this.campus = campus;
        };


        this.getUoftCourses = function (query, onSuccess, onError) {
            var obj = this;
            var url = "http://localhost:53235/api/courses?query=" + query;

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

                    onSuccess(obj.courseResults);
                },
                function (promise) {
                    onError(promise);
                }
            );
        };  
    });
}());