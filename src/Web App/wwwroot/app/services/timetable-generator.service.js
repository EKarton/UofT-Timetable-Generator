"use strict";
(function(){
	var timetableGenerator = angular.module("timetableApp");
    timetableGenerator.service("TimetableGenerator", function ($http) {	

        /**
         * Constructs and stores timetable generated in an object
        * @constructor
         * @param {SimplifiedTimetableBlock} fallSessions - The timetable blocks for the fall session
         * @param {SimplifiedTimetableBlock} winterSessions - The timetable blocks for the winter session
         * @param {string} name - The name of this timetable
         */
        var Timetable = function (fallSessions, winterSessions, name) {
            this.fallTimetableBlocks = fallSessions;
            this.winterTimetableBlocks = winterSessions;
            this.name = name;
        };

        /**
         * Constructs and stores the restrictions for the timetable generator
         * @constructor
         * @param {string} earliestClass - The 24-hr start time for the earliest class of the week
         * @param {string} latestClass - The 24-hr end time for the latest class of the week
         * @param {int} walkDurationInBackToBackClasses - The max. walking duration for back-to-back classes
         */
        var Restrictions = function (earliestClass, latestClass, walkDurationInBackToBackClasses) {
            this.earliestClass = earliestClass;
            this.latestClass = latestClass;
            this.walkDurationInBackToBackClasses = walkDurationInBackToBackClasses;
        };

        /**
         * Constructs and stores the preferences for the timetable generator
         * @constructor
         * @param {string} classType - The class type ('Morning', 'Afternoon', 'Evening', 'Night', 'Undefined')
         * @param {string} walkDistance - The walking distance between classes ('Minimum', 'Maximum', 'Undefined')
         * @param {string} numDaysInClass - The number of days spent for class ('Minimum', 'Maximum', 'Undefined')
         * @param {string} timeBetweenClasses - The amount of spare time between classes ('Minimum', 'Maximum', 'Undefined')
         * @param {int} lunchPeriod - The number of minutes spent for lunch (must be >= 0)
         */
        var Preferences = function (classType, walkDistance, numDaysInClass, timeBetweenClasses, lunchPeriod) {
            this.classType = classType;
            this.walkDistance = walkDistance;
            this.numDaysInClass = numDaysInClass;
            this.timeBetweenClasses = timeBetweenClasses;
            this.lunchPeriod = lunchPeriod;
        };

        /**
         * Stores the generated timetables, course codes, and the restrictions/preferences
         * @constructor
         * @param {string} courseCodes - The UofT course codes
         * @param {Timetable} timetables - The generated timetables
         * @param {Restrictions} restrictions - The restrictions applied to each timetable
         * @param {Preferences} preferences - The preferences applied to each timetabe
         */
        var Data = function (courseCodes, timetables, restrictions, preferences) {
            this.courseCodes = courseCodes;
            this.timetables = timetables;
            this.areTimetablesBeingGenerated = false;
            this.bookmarkedTimetables = [];
            this.sectionColors = {};
            this.restrictions = restrictions;
            this.preferences = preferences;

            this.generateNewColorScheme();
        };

        /**
        * Adds a timetable to the bookmarkedTimetables[].
        * Will add the timetable if it is not bookmarked already; else it will not
        * @param {Timetable} timetable - The timetable to bookmark
        * @return {bool} - True if the timetable has been bookmarked; else false
        */
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

        /**
        * Removes a timetable from the bookmarkedTimetables[].
        * Will remove the timetable if it is bookmarked; else it will not
        * @param {Timetable} timetable - A bookmarked timetable
        * @return {bool} - True if the timetable has been un-bookmarked; else false
        */
        Data.prototype.removeBookmark = function (timetable) {
            for (var i = 0; i < this.bookmarkedTimetables.length; i++) {
                if (this.bookmarkedTimetables[i].name === timetable.name) {
                    this.bookmarkedTimetables.splice(i, 1);
                    return true;
                }
            }
            return false;
        };

        /**
        * Determines if a timetable has been bookmarked or not
        * @param {Timetable} timetable - A timetable
        * @return {bool} - True if it is bookmarked; else false
        */
        Data.prototype.isBookmarked = function (timetable) {
            for (var i = 0; i < this.bookmarkedTimetables.length; i++)
                if (this.bookmarkedTimetables[i].name === timetable.name)
                    return true;
            return false;
        };

        /**
        * Generates a random color, each RGB value is between 100 and 255 inclusive.
        * It will return it as 'rgb({R}, {G}, {B})' where {R}, {G}, and {B} are the red, green, blue colors respectedly
        * @return {string} - The css color
        */
        Data.prototype.generateRandomColor = function () {
            var red = Math.floor(Math.random() * (255 - 100) + 100);
            var green = Math.floor(Math.random() * (255 - 100) + 100);
            var blue = Math.floor(Math.random() * (255 - 100) + 100);
            return "rgb(" + red + ", " + green + ", " + blue + ")";
        };

        /**
        * Generates a new color schema for the timetable blocks for each timetable
        * Note that a color schema is a collection of colors where each color is
        * designated for 1 section in the timetable.
        */
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
        };

        // Default preferences
        var defaultRestrictions = {
            earliestClass: 7,
            latestClass: 23,
            walkDurationInBackToBackClasses: 10
        };

        // Default preferences
        var defaultPreferences = {
            classType: "undefined",
            walkDistance: "undefined",
            numDaysInClass: "undefined",
            timeBetweenClasses: "undefined",
            lunchPeriod: null
        };

        this.generatedTimetables = new Data([], [], defaultRestrictions, defaultPreferences);

        /**
        * Creates an HTTP request to the server to generate timetables
        * @param {string[]} courseCodes - A set of complete UofT course codes
        * @param {Preferences = undefined} preferences - The preferences for the timetable generator
        * @param {Restrictions = undefined} restrictions - The restrictions for the timetable generator
        * @param {method(generatedTimetables)} onSuccess - A handler which will be called when the timetables are generated and returned from the server
        * @param {method(promise)} onFailure - A handler which will be called when the call to the server has failed.
        */
        this.generateTimetables = function (courseCodes, preferences, restrictions, onSuccess, onFailure) {

            // Create the timetable request
            var request = {
                courseCodes: courseCodes,
                preferences: preferences,
                restrictions: restrictions
            };
            if (preferences === undefined)
                request.preferences = defaultPreferences;
            if (restrictions === undefined)
                request.restrictions = defaultRestrictions;

            var obj = this;
            var url = "http://localhost:53235/api/timetables/getuofttimetables"; // "http://uofttimetablegenerator.azurewebsites.net/api/timetables/getuofttimetables"; 

            // Clear the timetables displayed on the webpage
            obj.generatedTimetables.courseCodes = courseCodes;
            obj.generatedTimetables.timetables = [];
            obj.generatedTimetables.bookmarkedTimetables = [];
            obj.generatedTimetables.areTimetablesBeingGenerated = true;

            $http.put(url, request).then(
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
                    obj.generatedTimetables.timetables = newTimetables;
                    obj.generatedTimetables.generateNewColorScheme();
                    obj.generatedTimetables.areTimetablesBeingGenerated = false;

                    if (onSuccess != undefined)
                        onSuccess();
                },
                function (response) {
                    if (onFailure != undefined)
                        onFailure(response);
                    obj.generatedTimetables.areTimetablesBeingGenerated = false;
                }
            );
        };
	});
}());