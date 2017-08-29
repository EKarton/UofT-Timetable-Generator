"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.directive("timetable", function ($window) {

        var TimetableBlock = function (x, y, width, height, color, content) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.color = color;
            this.content = content;
        };

        var GridLine = function (x1, y1, x2, y2) {
            this.x1 = x1;
            this.y1 = y1;
            this.x2 = x2;
            this.y2 = y2;
        };

        var Label = function (x, y, label) {
            this.x = x;
            this.y = y;
            this.label = label;
        }

        var constants = function ($scope, $element) {
            $scope.days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.earliestTime = 9.00;
            $scope.latestTime = 23.00;

            $scope.svgWidth = null;
            $scope.svgHeight = null;
            $scope.headerHeight = 20;
            $scope.timesColumnWidth = 40;
            $scope.dataRowHeight = 70;

            $scope.getRowHeight = function(){
                return 70;
            };
        }

        var createTimetableElements = function (scope, element, attributes) {
            var columnWidth = null;

            function createHeaderLabels() {
                scope.headerLabels = [];

                for (var i = 0; i < scope.days.length; i++) {
                    var x = i * columnWidth + scope.timesColumnWidth + (columnWidth / 2);
                    var y = scope.headerHeight - 2;
                    var label = scope.days[i];
                    scope.headerLabels.push(new Label(x, y, label));
                }
            }

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

            function createTimeLabels() {
                scope.timeLabels = [];

                for (var curHr = scope.earliestTime; curHr < scope.latestTime; curHr++) {
                    var x = scope.timesColumnWidth - 2;
                    var y = (curHr - scope.earliestTime) * scope.dataRowHeight + scope.headerHeight + (scope.dataRowHeight / 2);
                    var label = curHr + ":00";
                    scope.timeLabels.push(new Label(x, y, label));
                }
            }

            function setSize() {
                var svgElement = element.find("svg")[0];

                // Get the width of parent
                scope.svgWidth = svgElement.getBoundingClientRect().width;
                columnWidth = (scope.svgWidth - scope.timesColumnWidth) / scope.days.length;

                // Compute the height
                scope.svgHeight = scope.dataRowHeight * (scope.latestTime - scope.earliestTime) + scope.headerHeight;
                //console.log("Theoethical", scope.svgWidth, scope.svgHeight, scope.headerHeight, scope.dataRowHeight);

                // Set the width and height
                svgElement.setAttribute("viewBox", "0, 0, " + scope.svgWidth + ", " + scope.svgHeight);
                svgElement.setAttribute("preserveAspectRatio", "xMaxYMin slice");
                //console.log("Actual", svgElement.getBoundingClientRect().width, svgElement.getBoundingClientRect().height, scope.headerHeight);
            };

            scope.$watchCollection("[blocks, colorscheme]", function (newValues, oldValues) {
                var blocks = newValues[0];
                var colorScheme = newValues[1];

                console.log("Updating timetable", blocks, colorScheme);

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
            templateUrl: "./app/templates/timetable.template.html",
            controller: constants,
            link: createTimetableElements
        };
    });
}());