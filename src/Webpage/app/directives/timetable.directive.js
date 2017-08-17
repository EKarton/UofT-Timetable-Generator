"use strict";

(function () {
    var app = angular.module("timetableApp");
    app.directive("timetable", function ($window) {

        var link = function (scope, element, attributes) {
            var headers = ["", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            var blocks = [];

            var namespace = "http://www.w3.org/2000/svg";
            var columnWidth = null;
            var headerHeight = 10;
            var dataRowIntervals = 30;
            var timesColWidth = 20;
            var svgWidth = null;
            var svgHeight = null;

            var svgElement = element.find("svg")[0];
            var headersContainer = svgElement.querySelector(".headers");
            var gridlinesContainer = svgElement.querySelector(".gridlines");
            var timesContainer = svgElement.querySelector(".times");
            var blocksContainer = svgElement.querySelector(".blocks");

            var addHeaders = function () {
                // Add the background
                var rect = document.createElementNS(namespace, "rect");
                rect.setAttribute("x", 0);
                rect.setAttribute("y", 0);
                rect.setAttribute("width", svgWidth);
                rect.setAttribute("height", headerHeight);
                rect.setAttribute("fill", "grey");
                headersContainer.appendChild(rect);

                // Add the days
                for (var i = 0; i < scope.days.length; i++) {
                    var text = document.createElementNS(namespace, "text");
                    text.textContent = scope.days[i];
                    text.setAttribute("x", i * columnWidth + timesColWidth + (columnWidth / 2));
                    text.setAttribute("y", headerHeight - 2);
                    text.setAttribute("font-size", "7px");
                    text.setAttribute("text-anchor", "middle");
                    text.setAttribute("stroke-width", "1");
                    text.setAttribute("stroke", "black;");
                    headersContainer.appendChild(text);
                }
            };

            var addGridlines = function () {
                // Creating horizontal lines
                for (var curHr = scope.earliestTime; curHr <= scope.latestTime; curHr++) {
                    var line = document.createElementNS(namespace, "line");
                    line.setAttribute("x1", timesColWidth);
                    line.setAttribute("y1", (curHr - scope.earliestTime) * dataRowIntervals + headerHeight);
                    line.setAttribute("x2", svgWidth);
                    line.setAttribute("y2", (curHr - scope.earliestTime) * dataRowIntervals + headerHeight);
                    line.setAttribute("style", "stroke:#c5c8cc; stroke-width:0.5");
                    gridlinesContainer.appendChild(line);
                }

                // Creating vertical lines
                for (var i = 0; i <= scope.days.length; i++) {
                    var line = document.createElementNS(namespace, "line");
                    line.setAttribute("x1", i * columnWidth + timesColWidth);
                    line.setAttribute("y1", headerHeight);
                    line.setAttribute("x2", i * columnWidth + timesColWidth);
                    line.setAttribute("y2", svgHeight);
                    line.setAttribute("style", "stroke:#c5c8cc; stroke-width:0.5");
                    gridlinesContainer.appendChild(line);
                }
            };

            var addTimes = function () {
                // Add background
                var rect = document.createElementNS(namespace, "rect");
                rect.setAttribute("x", 0);
                rect.setAttribute("y", headerHeight);
                rect.setAttribute("width", timesColWidth);
                rect.setAttribute("height", svgHeight);
                rect.setAttribute("fill", "grey");
                timesContainer.appendChild(rect);

                // Add the times
                for (var curHr = scope.earliestTime; curHr < scope.latestTime; curHr++) {
                    var text = document.createElementNS(namespace, "text");
                    text.setAttribute("x", timesColWidth - 2);
                    text.setAttribute("y", (curHr - scope.earliestTime) * dataRowIntervals + headerHeight + (dataRowIntervals / 2));
                    text.setAttribute("font-size", "7px");
                    text.setAttribute("text-anchor", "end ");
                    text.setAttribute("stroke-width", "1");
                    text.setAttribute("stroke", "black;");
                    text.textContent = curHr + ":00";

                    timesContainer.appendChild(text);
                }
            };

            var getBlockUI = function(block, color) {
                // Get the times and days
                var startTime = scope.getStartTime(block);
                var endTime = scope.getEndTime(block);
                var startDay = scope.getStartDay(block);

                var rect = document.createElementNS(namespace, "rect");
                rect.setAttribute("fill", color);
                rect.setAttribute("x", columnWidth * (startDay - 1) + timesColWidth);
                rect.setAttribute("y", headerHeight + (dataRowIntervals * (startTime - scope.earliestTime)));
                rect.setAttribute("width", columnWidth);
                rect.setAttribute("height", (endTime - startTime) * dataRowIntervals);
                return rect;
            }

            var getBlockInfoUI = function (block) {

            };

            var setSize = function () {
                // Get the width of parent
                svgWidth = svgElement.getBoundingClientRect().width;
                columnWidth = (svgWidth - timesColWidth) / scope.days.length;

                // Compute the height
                svgHeight = dataRowIntervals * (scope.latestTime - scope.earliestTime) + headerHeight;

                // Set the width and height
                svgElement.setAttribute("viewBox", "0, 0, " + svgWidth + ", " + svgHeight);
                svgElement.setAttribute("width", "100%");
                console.log(columnWidth, svgWidth, svgHeight);
            };

            var splitUpBlock = function (block) {
                var startDay = scope.getStartDay(block);
                var endDay = scope.getEndDay(block);
                var newBlocks = [];
                for (var j = startDay; j < endDay; j++) {
                    var computedStartTime = block.startTime;
                    var computedEndTime = block.endTime;

                    if (j === startDay && j < endDay)
                        computedEndTime = scope.latestTime;
                    else if (startDay < j && j == endDay)
                        computedStartTime = scope.earliestTime;
                    else {
                        computedStartTime = scope.earliestTime;
                        computedEndTime = scope.latestTime;
                    }

                    var newBlock = {
                        courseCode: block.courseCode,
                        sectionCode: block.sectionCode,
                        instructors: block.instructors,
                        location: block.location,
                        startTime: computedStartTime,
                        endTime: computedEndTime
                    };
                    newBlocks.push(newBlock);
                }
                return newBlocks;
            };

            scope.$watchCollection("[sessions, term, colorscheme]", function (newValues, oldValues) {
                var sessions = newValues[0];
                var term = newValues[1];
                var colorScheme = newValues[2];

                console.log("Data binding!!", sessions, term, colorScheme);

                if (sessions == null || term == null || colorScheme == null)
                    return;

                blocks = sessions.fallTimetableBlocks;
                if (term == "Winter")
                    blocks = sessions.winterTimetableBlocks;

                // Split the blocks if it spans for more than 1 day
                var newBlocks = [];
                for (var i = 0; i < blocks.length; i++) {
                    newBlocks.concat(splitUpBlock(blocks[i]));
                }
                blocks.concat(newBlocks);

                // Remove the old blocks
                while (blocksContainer.firstChild)
                    blocksContainer.removeChild(blocksContainer.firstChild);

                // Add the blocks
                for (var i = 0; i < blocks.length; i++) {
                    var block = blocks[i];
                    var color = colorScheme[block.courseCode + "|" + block.activityType];
                    blocksContainer.appendChild(getBlockUI(block, color));
                }
            });

            setSize();
            addHeaders();
            addTimes();
            addGridlines();
        };

        var controller = function ($scope, $element) {
            $scope.headers = ["", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.days = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
            $scope.earliestTime = 9.00;
            $scope.latestTime = 23.00;

            $scope.getStartDay = function (block) {
                return parseInt(block.startTime / 100);
            }

            $scope.getEndDay = function (block) {
                return parseInt(block.endTime / 100);
            }

            $scope.getStartTime = function (block) {
                return block.startTime % 100;
            }

            $scope.getEndTime = function (block) {
                return block.endTime % 100;
            }
        }

        return {
            restrict: "E",
            scope: { sessions: "=", term: "=", colorscheme: "=" },
            templateUrl: "./app/templates/timetable.template.html",
            controller: controller,
            link: link
        };
    });
}());