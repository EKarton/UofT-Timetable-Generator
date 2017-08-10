"use strict";
(function(){
	var timetableGenerator = angular.module("timetableApp");
	timetableGenerator.service("Generator", function($http) {	

		this.uoftFallCourseCodes = [];
		this.uoftWinterCourseCodes = [];
		this.genericFallCourses = [];
		this.genericWinterCourses = [];

		this.getCourses = function(query){
			if (query.toString().length < 3)
				return [];
			else
			{
				return [
					{code: "MAT237Y1-Y", term: "Y"},
					{code: "MAT237Y1-Y", term: "Y"},
					{code: "MAT237Y1-Y", term: "Y"},
					{code: "MAT237Y1-Y", term: "Y"}
				];
			}
		};  

		this.generateUoftTimetables = function(courseCodes){
            var url = "http://localhost:53235/api/timetables/getuofttimetables";
            var data = JSON.stringify(["MAT137Y1-Y", "CSC148H1-F", "ENV100H1-F", "COG250Y1-Y"]);

            return $http.put(url, data);
        };
	});
}());