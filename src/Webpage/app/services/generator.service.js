"use strict";
(function(){
	var timetableGenerator = angular.module("timetableApp");
    timetableGenerator.service("Generator", function ($http, $q) {	

        this.courseCodes = [];

        this.getCourses = function (query) {
            var url = "http://localhost:53235/api/courses?query=" + query;
            return $http.get(url);
		};  

		this.generateUoftTimetables = function(){
            var url = "http://localhost:53235/api/timetables/getuofttimetables";

            return $http.put(url, this.courseCodes);
        };
	});
}());