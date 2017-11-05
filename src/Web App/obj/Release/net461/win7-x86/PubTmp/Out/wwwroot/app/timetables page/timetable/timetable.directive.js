"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.directive("timetable", function ($window) {

        /**
         * A class used to contain graphic and content information about a rect. block in the timetable
         * @param {int} x - The x coordinate of the top-left corner of this block
         * @param {int} y - The y coordinate of the top-left corner of this block
         * @param {int} width - The width of the block
         * @param {int} height - The height of the block
         * @param {string} color - The CSS color of this block
         * @param {string[]} content -The content in the block
         */
        var TimetableBlock = function (x, y, width, height, color, content) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            this.content = content;
        };

        /**
         * A class used to represent a grid line in the timetable
         * @param {int} x1 - The x coordinate of the first point
         * @param {int} y1 - The y coordinate of the first point
         * @param {int} x2 -The x coordinate of the second point
         * @param {int} y2 - The y coordinate of the second point
         */
        var GridLine = function (x1, y1, x2, y2) {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        };

        /**
         * A class used to represent a label in the timetable
         * @param {string} label - The text of this label
         * @param {int} x - The x coordinate of this label
         * @param {int} y - The y coordinate of this label
         */
        var Label = function (x, y, label) {
            this.x = x;
            this.y = y;
            this.label = label;
        }

        /**
         * Stores all the important properties / variables / constants for this timetable directive
         * @param {any} $scope - The $scope of this directive
         * @param {any} $element - The element of this directive
         */
        var constants = function ($scope, $element) {
            $scope.days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.earliestTime = 9.00;
            $scope.latestTime = 23.00;

            // The width/height of the SVG element
            $scope.svgWidth = null;
            $scope.svgHeight = null;

            // The height of the labels that show the days of the week
            $scope.headerHeight = 20;

            // The width of the column containing the time labels in the timetable
            $scope.timesColumnWidth = 40;

            // The height of each row in the timetable
            $scope.dataRowHeight = 70;

            /**
             * Returns the height for each row in the timetable
             */
            $scope.getRowHeight = function(){
                return 70;
            };
        }

        /**
         * Creates all the SVG elements used to render the timetable
         * @param {any} scope - The scope of this directive
         * @param {any} element - The elements in this directive
         * @param {any} attributes - The attributes attached to this directive
         */
        var createTimetableElements = function (scope, element, attributes) {

            // The width for each column representing the days of the week
            var columnWidth = null;

            /**
             * Create the labels that show the days of the week in the timetable,
             * and saves them in scope.headerLabels[]
             */
            function createHeaderLabels() {
                scope.headerLabels = [];

                for (var i = 0; i < scope.days.length; i++) {
                    var x = i * columnWidth + scope.timesColumnWidth + (columnWidth / 2);
                    var y = scope.headerHeight - 2;
                    var label = scope.days[i];
                    scope.headerLabels.push(new Label(x, y, label));
                }
            }

            /**
             * Returns a string list of all the content in a timetable block
             * @param {SimplifiedTimetableBlock} block -The block
             * @return {string[]} The content in the timetable block
             */
            function getTimetableBlocksContent(block) {
                // Get the instructors to a string
                var instructors = "";
                for (var i = 0; i < block.instructors.length; i++)
                    instructors += block.instructors[i] + ", ";
                if (instructors.length > 0)
                    instructors.substring(0, instructors.length - 1);

                // Put the location into a string
                var location = "";
                if (block.buildingCode != undefined && block.buildingCode != null && block.buildingCode != "")
                    location += block.buildingCode;
                if (location.length > 0)
                    location += " ";
                if (block.roomNumber != undefined && block.roomNumber != null && block.roomNumber != "")
                    location += block.roomNumber;

                var content = [];
                if (block.courseCode != "")
                    content.push(block.courseCode);
                if (block.activityType != "")
                    content.push(block.activityType);
                if (instructors != "")
                    content.push(instructors);
                if (location != "")
                    content.push(location);
                content.push(block.startTime + ":00 - " + block.endTime + ":00");

                return content;
            }

            /**
             * Creates and populates the scope.timetableBlocks for the timetable blocks in this timetable
             * @param {SimplifiedTimetableBlock} blocks - The blocks in the current timetable
             * @param {any} colorScheme - The color scheme for the timetable blocks
             */
            function createTimetableBlocks(blocks, colorScheme){
                scope.timetableBlocks = [];

                for (var i = 0; i < blocks.length; i++) {
                    var x = (columnWidth * (blocks[i].startDay)) + scope.timesColumnWidth;
                    var y = scope.headerHeight + (scope.dataRowHeight * (blocks[i].startTime - scope.earliestTime));
                    var width = columnWidth;
                    var height = (blocks[i].endTime - blocks[i].startTime) * scope.dataRowHeight;
                    var color = colorScheme[blocks[i].courseCode + "|" + blocks[i].activityType];
                    var content = getTimetableBlocksContent(blocks[i]);                    
                    scope.timetableBlocks.push(new TimetableBlock(x, y, width, height, color, content));
                }
            }

            /**
             * Creates vertical and horizontal grid lines and saves them in
             * scope.gridLines[] for the timetable directive
             */
            function createGridLines() {
                scope.gridLines = [];

                // Making horizontal lines
                var numHorizontalLines = scope.latestTime - scope.earliestTime;
                for (var i = 0; i <= numHorizontalLines; i++) {
                    var x1 = scope.timesColumnWidth;
                    var y1 = scope.headerHeight + (i * scope.dataRowHeight);
                    var x2 = scope.svgWidth;
                    var y2 = y1;
                    scope.gridLines.push(new GridLine(x1, y1, x2, y2));
                }

                // Making the vertical lines
                for (var i = 0; i <= scope.days.length; i++) {
                    var x1 = i * columnWidth + scope.timesColumnWidth;
                    var y1 = scope.headerHeight;
                    var x2 = x1;
                    var y2 = scope.svgHeight;
                    scope.gridLines.push(new GridLine(x1, y1, x2, y2));
                }
            }

            /**
             * Creates the labels that show the times for each row in the timetable,
             * and saves them in scope.timeLabels
             */
            function createTimeLabels() {
                scope.timeLabels = [];

                for (var curHr = scope.earliestTime; curHr < scope.latestTime; curHr++) {
                    var x = scope.timesColumnWidth - 2;
                    var y = (curHr - scope.earliestTime) * scope.dataRowHeight + scope.headerHeight + (scope.dataRowHeight / 2);
                    var label = curHr + ":00";
                    scope.timeLabels.push(new Label(x, y, label));
                }
            }

            /**
             * Set the size of the SVG element
             */
            function setSize() {
                var svgElement = element.find("svg")[0];

                // Get the width of parent
                scope.svgWidth = svgElement.getBoundingClientRect().width;
                columnWidth = (scope.svgWidth - scope.timesColumnWidth) / scope.days.length;

                // Compute the height
                scope.svgHeight = scope.dataRowHeight * (scope.latestTime - scope.earliestTime) + scope.headerHeight;

                // Set the width and height
                svgElement.setAttribute("viewBox", "0, 0, " + scope.svgWidth + ", " + scope.svgHeight);
                svgElement.setAttribute("preserveAspectRatio", "xMaxYMin slice");
            };

            // Each time the data's value is changed, it updates the graph
            scope.$watchCollection("[blocks, colorscheme]", function (newValues, oldValues) {
                var blocks = newValues[0];
                var colorScheme = newValues[1];

                if (blocks == null || colorScheme == null)
                    return;

                setSize();
                createHeaderLabels();
                createGridLines();
                createTimeLabels();
                createTimetableBlocks(blocks, colorScheme);
            });

            setSize();
        };

        return {
            restrict: "E",
            scope: { blocks: "=", colorscheme: "=" },
            templateUrl: "app/timetables page/timetable/timetable.directive.html",
            controller: constants,
            link: createTimetableElements
        };
    });
}());