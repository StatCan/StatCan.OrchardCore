/*
** NOTE: This file is generated by Gulp and should not be edited directly!
** Any changes made directly to this file will be overwritten next time its asset group is processed by Gulp.
*/

function _createForOfIteratorHelper(o, allowArrayLike) { var it = typeof Symbol !== "undefined" && o[Symbol.iterator] || o["@@iterator"]; if (!it) { if (Array.isArray(o) || (it = _unsupportedIterableToArray(o)) || allowArrayLike && o && typeof o.length === "number") { if (it) o = it; var i = 0; var F = function F() {}; return { s: F, n: function n() { if (i >= o.length) return { done: true }; return { done: false, value: o[i++] }; }, e: function e(_e) { throw _e; }, f: F }; } throw new TypeError("Invalid attempt to iterate non-iterable instance.\nIn order to be iterable, non-array objects must have a [Symbol.iterator]() method."); } var normalCompletion = true, didErr = false, err; return { s: function s() { it = it.call(o); }, n: function n() { var step = it.next(); normalCompletion = step.done; return step; }, e: function e(_e2) { didErr = true; err = _e2; }, f: function f() { try { if (!normalCompletion && it.return != null) it.return(); } finally { if (didErr) throw err; } } }; }

function _unsupportedIterableToArray(o, minLen) { if (!o) return; if (typeof o === "string") return _arrayLikeToArray(o, minLen); var n = Object.prototype.toString.call(o).slice(8, -1); if (n === "Object" && o.constructor) n = o.constructor.name; if (n === "Map" || n === "Set") return Array.from(o); if (n === "Arguments" || /^(?:Ui|I)nt(?:8|16|32)(?:Clamped)?Array$/.test(n)) return _arrayLikeToArray(o, minLen); }

function _arrayLikeToArray(arr, len) { if (len == null || len > arr.length) len = arr.length; for (var i = 0, arr2 = new Array(len); i < len; i++) { arr2[i] = arr[i]; } return arr2; }

var attachSorter = function attachSorter() {
  var tables = document.querySelectorAll("#entity-table");

  var _iterator = _createForOfIteratorHelper(tables),
      _step;

  try {
    var _loop = function _loop() {
      var table = _step.value;
      var headers = table.querySelectorAll("th");
      var tableBody = table.querySelector("tbody");
      var rows = tableBody.querySelectorAll("tr"); // Track sort directions

      var directions = Array.from(headers).map(function (header) {
        return "";
      }); // Transform the content of given cell in given column

      var transform = function transform(index, content) {
        // Get the data type of column
        var type = headers[index].getAttribute("data-type");

        if (type === "name") {
          content = content.childNodes[0].innerHTML;
        } else if (type === "number") {
          content = content.childNodes[0].childNodes[2].innerHTML;
        } else if (type === "date-range") {
          content = content.childNodes[0].innerHTML.split("-")[0];
        }

        switch (type) {
          case "number":
            return parseFloat(content);

          case "date":
            return Date.parse(content);

          case "string":
          default:
            return content;
        }
      };

      var sortColumn = function sortColumn(index) {
        // Get the current direction
        var direction = directions[index] || "asc"; // A factor based on the direction

        var multiplier = direction === "asc" ? 1 : -1;
        var newRows = Array.from(rows);
        newRows.sort(function (rowA, rowB) {
          var cellA = rowA.querySelectorAll("td")[index];
          var cellB = rowB.querySelectorAll("td")[index];
          var a = transform(index, cellA);
          var b = transform(index, cellB);

          switch (true) {
            case a > b:
              return 1 * multiplier;

            case a < b:
              return -1 * multiplier;

            case a === b:
              return 0;
          }
        }); // Remove old rows

        [].forEach.call(rows, function (row) {
          tableBody.removeChild(row);
        }); // Reverse the direction

        directions[index] = direction === "asc" ? "desc" : "asc"; // Append new row

        newRows.forEach(function (newRow) {
          tableBody.appendChild(newRow);
        });
      };

      [].forEach.call(headers, function (header, index) {
        header.addEventListener("click", function () {
          sortColumn(index);
        });
      });
    };

    for (_iterator.s(); !(_step = _iterator.n()).done;) {
      _loop();
    }
  } catch (err) {
    _iterator.e(err);
  } finally {
    _iterator.f();
  }
};

var attachSearchClear = function attachSearchClear() {
  // Hook into the rendered button
  document.getElementById("search-clear-button").addEventListener("click", function () {
    document.getElementById("search-input").value = "";
    document.getElementById("search-form").submit();
  });
};