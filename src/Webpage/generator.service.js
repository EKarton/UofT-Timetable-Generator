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

		this.wasd = "";
		this.generateUoftTimetables = function(courseCodes){

			var request = {
				method: "PUT",
				url: "http://localhost:53235/api/timetables/getuofttimetables",
				data: JSON.stringify(["MAT137Y1-Y", "CSC148H1-F"])
			};

			return $http(request).then(function(response){

			}, function(response){
				console.log("ERROR");
				console.log(JSON.stringify(response));
			});
		};
	});
}());