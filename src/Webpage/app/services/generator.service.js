"use strict";
(function(){
	var timetableGenerator = angular.module("timetableApp");
    timetableGenerator.service("Generator", function ($http) {	

        var Timetable = function (fallSessions, winterSessions, name) {
            this.fallTimetableBlocks = fallSessions;
            this.winterTimetableBlocks = winterSessions;
            this.name = name;
        };

        var Restrictions = function (earliestClass, latestClass, walkDurationInBackToBackClasses) {
            this.earliestClass = earliestClass;
            this.latestClass = latestClass;
            this.walkDurationInBackToBackClasses = walkDurationInBackToBackClasses;
        };

        var Preferences = function (classType, walkDistance, numDaysInClass, timeBetweenClasses, lunchPeriod) {
            this.classType = classType;
            this.walkDistance = walkDistance;
            this.numDaysInClass = numDaysInClass;
            this.timeBetweenClasses = timeBetweenClasses;
            this.lunchPeriod = lunchPeriod;
        };

        var Data = function (courseCodes, timetables, restrictions, preferences) {
            this.courseCodes = courseCodes;
            this.timetables = timetables;
            this.bookmarkedTimetables = [];
            this.sectionColors = {};
            this.restrictions = restrictions;
            this.preferences = preferences;

            this.generateNewColorScheme();
        };
        Data.prototype.addBookmark = function (timetable) {
            for (var i = 0; i < this.bookmarkedTimetables.length; i++) {
                if (this.bookmarkedTimetables[i].name === timetable.name)
                    return false;
                else if (this.bookmarkedTimetables[i] === timetable)
                    return false;
            }

            this.bookmarkedTimetables.push(timetable);
            return true;
        };
        Data.prototype.removeBookmark = function (timetable) {
            for (var i = 0; i < this.bookmarkedTimetables.length; i++) {
                if (this.bookmarkedTimetables[i].name === timetable.name) {
                    this.bookmarkedTimetables.splice(i, 1);
                    return true;
                }
            }
            return false;
        };
        Data.prototype.isBookmarked = function (timetable) {
            for (var i = 0; i < this.bookmarkedTimetables.length; i++)
                if (this.bookmarkedTimetables[i].name === timetable.name)
                    return true;
            return false;
        };
        Data.prototype.generateRandomColor = function () {
            var red = Math.floor(Math.random() * (255 - 100) + 100);
            var green = Math.floor(Math.random() * (255 - 100) + 100);
            var blue = Math.floor(Math.random() * (255 - 100) + 100);
            return "rgb(" + red + ", " + green + ", " + blue + ")";
        };
        Data.prototype.generateNewColorScheme = function () {
            this.sectionColors = {};

            // For the fall timetable
            if (this.timetables.length > 0) {
                var fallBlocks = this.timetables[0].fallTimetableBlocks;
                for (var i = 0; i < fallBlocks.length; i++) {
                    var key = fallBlocks[i].courseCode + "|" + fallBlocks[i].activityType;
                    if (this.sectionColors[key] == undefined)
                        this.sectionColors[key] = this.generateRandomColor();
                }
            }

            // For the winter timetable
            if (this.timetables.length > 0) {
                var winterBlocks = this.timetables[0].winterTimetableBlocks;
                for (var i = 0; i < winterBlocks.length; i++) {
                    var key = winterBlocks[i].courseCode + "|" + winterBlocks[i].activityType;
                    if (this.sectionColors[key] == undefined)
                        this.sectionColors[key] = this.generateRandomColor();
                }
            }

            console.log("Updated color scheme", this.sectionColors);
        };

        var defaultRestrictions = {
            earliestClass: 7,
            latestClass: 23,
            walkDurationInBackToBackClasses: 10
        };
        var defaultPreferences = {
            classType: "min",
            walkDistance: "min",
            numDaysInClass: "min",
            timeBetweenClasses: "min",
            lunchPeriod: 60
        };
        this.generatedTimetables = new Data([], [], defaultRestrictions, defaultPreferences);

        var handleErrorInTimetablesRequest = function (promise) {
            this.data = null;
        };

        this.getCourses = function (query) {
            var url = "http://localhost:53235/api/courses?query=" + query;
            return $http.get(url);
        };  

        this.generateTimetables = function (courseCodes) {
            var obj = this;

            var url = "http://localhost:53235/api/timetables/getuofttimetables";
            $http.put(url, courseCodes).then(
                function (response) {
                    // Parse the timetables
                    var newTimetables = [];
                    var rawTimetables = response.data;
                    for (var i = 0; i < rawTimetables.length; i++) {
                        var rawT = rawTimetables[i];
                        var parsedT = new Timetable(rawT.fallTimetableBlocks, rawT.winterTimetableBlocks, rawT.name);
                        newTimetables.push(parsedT);
                    }

                    // Update the generated timetables singleton obj
                    obj.generatedTimetables.courseCodes = courseCodes;
                    obj.generatedTimetables.timetables = newTimetables;
                    obj.generatedTimetables.generateNewColorScheme();
                },
                function (response) {

                }
            );
        };
	});
}());